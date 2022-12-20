from enum import Enum

from common.grid_objects import Point, Direction

center = 0

class Cave:
    def __init__(self, rock_walls: [str]):
        self.walls = []
        for rock_wall in rock_walls:
            lines = rock_wall.split('->')
            self.walls.append(Wall(lines))
        self.wall_points = []
        self.cave_right = 0
        self.cave_left = 500
        self.cave_top = 0
        self.cave_bottom = 0
        self.cave_map = {}
        for wall in self.walls:
            for point in wall.points:
                self.cave_map[point] = CaveTypes.Rock
                if point.x < self.cave_left:
                    self.cave_left = point.x
                elif point.x > self.cave_right:
                    self.cave_right = point.x
                if point.y > self.cave_top:
                    self.cave_top = point.y
                elif point.y < self.cave_bottom:
                    self.cave_bottom = point.y
        self.cave_left -= 1
        self.cave_right += 1
        self.cave_top += 3
        for x in range(self.cave_left, self.cave_right):
            for y in range(self.cave_bottom, self.cave_top):
                point = Point(x, y)
                if point not in self.cave_map.keys() and point.y != self.cave_top - 1:
                    self.cave_map[point] = CaveTypes.Air
                elif point.y == self.cave_top - 1:
                    self.cave_map[point] = CaveTypes.Rock
        self.print_cave()

    def start_sand(self):
        sand_point = Point(500, 0)
        next_point = Point(500, 0)
        direction = Direction.Up
        units_of_sand = 0
        while True:
            new_point = Point(next_point.x, next_point.y)
            new_point.move(direction)
            blocked = self.blocked(new_point)
            if blocked is None:
                break
            if direction == Direction.Void:
                self.cave_map[sand_point] = CaveTypes.Sand
                units_of_sand += 1
                sand_point = Point(500, 0)
                next_point = Point(500, 0)
                direction = Direction.Up
                continue
            if blocked:
                direction = self.next_direction(direction)
                continue
            next_point.move(direction)
            sand_point.move(direction)
            direction = Direction.Up
        self.print_cave()
        print(units_of_sand + 1)
                
    def next_direction(self, direction):
        match direction:
            case Direction.Up:
                return Direction.UpLeft
            case Direction.UpLeft:
                return Direction.UpRight
            case Direction.UpRight:
                return Direction.Void
            case Direction.Void:
                return Direction.Up

    def blocked(self, point):
        if point == Point(500, 0):
            return None
        if point not in self.cave_map.keys():
            self.add_width(point)
        cave_type = self.cave_map[point]
        return not cave_type == CaveTypes.Air

    def add_width(self, point):
        for y in range(self.cave_bottom, self.cave_top - 1):
            self.cave_map[Point(point.x, y)] = CaveTypes.Air
        self.cave_map[Point(point.x, self.cave_top - 1)] = CaveTypes.Rock
        if point.x < 500:
            self.cave_left -= 1
        else:
            self.cave_right += 1

    def print_cave(self):
        output = []
        for height in range(self.cave_bottom, self.cave_top):
            row = []
            for width in range(self.cave_left, self.cave_right):
                row.append('')
            output.append(row)
        for point in self.cave_map.keys():
            output[point.y][point.x - self.cave_right] = str(self.cave_map[point])
        print('\n'.join(map(''.join, output)))


class Wall:
    def __init__(self, lines):
        self.points = []
        for line in lines:
            pos = line.split(',')
            x = int(pos[0]) - center
            y = int(pos[1])
            self.points.append(Point(x, y))
        len_check = len(self.points)
        all_points = []
        for idx, point in enumerate(self.points):
            all_points.append(point)
            if idx + 1 == len_check:
                break
            next_point = self.points[idx + 1]
            direction = Direction.Left
            if point.x == next_point.x and point.y < next_point.y:
                direction = Direction.Up
            elif point.x == next_point.x and point.y > next_point.y:
                direction = Direction.Down
            elif point.y == next_point.y and point.x < next_point.x:
                direction = Direction.Right
            x_s = point.x
            x_f = point.x
            y_s = point.y
            y_f = point.y
            vertical = True
            match direction:
                case Direction.Up:
                    y_f = next_point.y
                    vertical = False
                case Direction.Down:
                    y_s = next_point.y
                    vertical = False
                case Direction.Left:
                    x_s = next_point.x
                case Direction.Right:
                    x_f = next_point.x
            if vertical:
                for pos in range(x_s, x_f):
                    all_points.append(Point(pos, point.y))
            else:
                for pos in range(y_s, y_f):
                    all_points.append(Point(point.x, pos))
        self.points = list(set(all_points))


class CaveTypes(Enum):
    Rock = '#'
    Sand = 'O'
    Air = '.'

    def __str__(self):
        return str(self.value)
