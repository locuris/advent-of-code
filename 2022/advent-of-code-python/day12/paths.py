import copy

from common.grid_objects import Point
from common.util import alphabet_index

debug_print = False
debug = 0
shortest_path = 9999


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
                self.map[point] = alphabet_index(pos)
                self.debug_map[point] = pos
                if pos == 'S':
                    self.start = point
                elif pos == 'E':
                    self.end = point
                x += 1
            y -= 1
        self.paths: [Path] = []
        self.completed_paths: [Path] = []
        self.print_map()

    def start_path_generation(self):
        first_path = Path(self)
        first_path.move_point()

    def print_map(self):
        if not debug_print:
            return
        output = ''
        for y in range(self.height):
            line = ''
            for x in range(self.width):
                point = Point(x, y)
                line += self.debug_map[point]
            output += line[::-1] + '\n'
        output = output[::-1]
        print(output)


class Path:
    def __init__(self, height_map: HeightMap):
        self.paths = []
        self.next_points = []
        self.debug_map = {}
        self.points: [Point] = []
        self.points.append(height_map.start)
        self.map: HeightMap = height_map
        self.current_position: Point = height_map.start
        self.length = 0

    def __eq__(self, other):
        if len(self.points) != len(other.points):
            return False
        for point in self.points:
            if point not in other.points:
                return False
        return True

    def move_point(self):
        global debug
        debug += 1
        up_point = self.up_point()
        self.__move(up_point, '^')
        down_point = self.down_point()
        self.__move(down_point, 'v')
        right_point = self.right_point()
        self.__move(right_point, '>')
        left_point = self.left_point()
        self.__move(left_point, '<')
        for path in self.paths:
            path.move_point()

    def __add_point(self, new_point):
        self.points.append(new_point)
        self.current_position = new_point
        self.length += 1

    def __move(self, new_point, direction):
        global shortest_path
        if self.length > shortest_path:
            return
        if new_point is None or new_point not in self.map.map.keys():
            return
        if new_point == self.map.end:
            if self.length < shortest_path:
                shortest_path = self.length
            new_path = copy.deepcopy(self)
            new_path.__add_point(new_point)
            self.map.completed_paths.append(new_path)
            return
        cp = self.map.map[self.current_position]
        np = self.map.map[new_point]
        if np > cp + 1:
            return
        self._print_path()
        new_path = copy.deepcopy(self)
        new_path.debug_map[new_path.current_position] = direction
        new_path.__add_point(new_point)
        self.paths.append(new_path)

    def __new_point(self, x, y) -> Point | None:
        new_point = Point(self.current_position.x + x, self.current_position.y + y)
        if new_point in self.points:
            return None
        return new_point

    def up_point(self) -> Point | None:
        new_point = self.__new_point(0, 1)
        if new_point is None or new_point.y >= self.map.height:
            return None
        return new_point

    def down_point(self) -> Point | None:
        new_point = self.__new_point(0, -1)
        if new_point is None or new_point.y < 0:
            return None
        return new_point

    def right_point(self) -> Point | None:
        new_point = self.__new_point(1, 0)
        if new_point is None or new_point.x >= self.map.width:
            return None
        return new_point

    def left_point(self) -> Point | None:
        new_point = self.__new_point(-1, 0)
        if new_point is None or new_point.x < 0:
            return None
        return new_point

    def length(self):
        return len(self.points)

    def print_path(self):
        if not debug_print:
            return
        path = ''
        for point in self.points:
            path += self.map.debug_map[point]
            print(point)
        print(path)

    def _print_path(self):
        if not debug_print:
            return
        output = ''
        for y in range(self.map.height):
            line = ''
            for x in range(self.map.width):
                point = Point(x, y)
                if point in self.debug_map.keys():
                    line += self.debug_map[point]
                else:
                    line += '.'
            output += line[::-1] + '\n'
        output = output[::-1]
        print(output)
