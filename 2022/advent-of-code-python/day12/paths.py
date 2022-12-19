import copy
import threading
from common.grid_objects import Point
from common.util import alphabet_index


class ShortestTracker:
    def __init__(self):
        self.length = 9999999


c = threading.Condition()
debug_print = False
shortest_path = ShortestTracker()
completed_paths = []
longest_path = 0


class HeightMap:
    def __init__(self, string_map: [str]):
        self.map: dict = {}
        self.debug_map: dict = {}
        self.start: Point = Point()
        self.end: Point = Point()
        self.height: int = len(string_map)
        self.width: int = len(string_map[0])
        y = self.height - 1
        for line in string_map:
            x = 0
            for pos in line:
                point = Point(x, y)
                height_value = alphabet_index(pos)
                self.map[point] = height_value if height_value <= 26 else 1
                self.debug_map[point] = pos
                if pos == 'S':
                    self.start = point
                elif pos == 'E':
                    self.end = point
                    self.map[point] = 26
                x += 1
            y -= 1
        self.paths: [Path] = []
        self.completed_paths: [Path] = []

    def start_path_generation(self):
        first_paths = [Path(self.start, self.end, self.width, self.height, self.map)]
        self.generate_paths(first_paths)

    def path_generation_part_ii(self):
        first_paths = []
        for key in self.map.keys():
            if self.map[key] == 1:
                first_paths.append(Path(key, self.end, self.width, self.height, self.map))
        print(f'Total starting positions {len(first_paths)}')
        await self.generate_paths(first_paths)
        print(f'sortest path: {shortest_path}')

    async def generate_paths(self, first_paths):
        paths_attempted = 0
        for first_path in first_paths:
            print(f'paths attempted {paths_attempted} with shortest length {shortest_path.length}')
            paths_attempted += 1
            paths = [first_path]
            while len(paths) != 0:
                current_paths = []
                previous_paths = copy.copy(paths)
                for path in previous_paths:
                    paths.remove(path)
                    new_paths = path.move_point()
                    for new_path in new_paths:
                        current_paths.append(new_path)
                for current_path in current_paths:
                    paths.append(current_path)


class Path:
    def __init__(self, start: Point, end: Point, width: int, height: int, height_map):
        self.points: [Point] = []
        self.points.append(start)
        self.map_width = width
        self.map_height = height
        self.start = start
        self.current_position: Point = start
        self.end = end
        self.length = 0
        self.neighbours = []
        self.paths = []
        self.height_map = height_map
        self.debug_str = 'S'
        self.debug_map = {}

    def __eq__(self, other):
        if len(self.points) != len(other.points):
            return False
        for point in self.points:
            if point not in other.points:
                return False
        return True

    def __str__(self):
        return self.debug_str

    def move_point(self):
        self.paths = []
        self.up_point()
        self.down_point()
        self.right_point()
        self.left_point()
        return self.paths

    def __add_point(self, new_point):
        self.points.append(new_point)
        self.current_position = new_point
        self.length += 1
        global longest_path
        if self.length > longest_path:
            longest_path = self.length

    def __new_point(self, x, y, direction) -> Point | None:
        if self.length > shortest_path.length:
            return None
        new_point = Point(self.current_position.x + x, self.current_position.y + y)
        if new_point.x >= self.map_width or new_point.x < 0 or new_point.y >= self.map_height or new_point.y < 0:
            return None
        cp = self.height_map[self.current_position]
        np = self.height_map[new_point]
        if cp + 1 < np or new_point in self.points:
            return None
        if new_point == self.end:
            self.add_completed_path()
            return None
        new_path = copy.copy(self)
        new_path.__add_point(new_point)
        new_path.debug_str += direction
        new_path.debug_map[self.current_position] = direction
        self.paths.append(new_path)
        return new_point

    def up_point(self):
        self.__new_point(0, 1, '^')

    def down_point(self):
        self.__new_point(0, -1, 'v')

    def right_point(self):
        self.__new_point(1, 0, '>')

    def left_point(self):
        self.__new_point(-1, 0, '<')

    def print_path(self):
        output = ''
        for y in range(self.map_height):
            line = ''
            for x in range(self.map_width):
                point = Point(x, y)
                if point in self.debug_map.keys():
                    line += self.debug_map[point]
                else:
                    line += '.'
            output += line[::-1] + '\n'
        output = output[::-1]
        print(output)

    def add_completed_path(self):
        c.acquire()
        if self.length < shortest_path.length:
            shortest_path.length = self.length
        completed_paths.append(self)
        c.notify_all()
        c.release()
