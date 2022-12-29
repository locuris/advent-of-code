from typing import Type, Set

from common.grid_objects import Point
import math

max_dist = 0

class Grid:
    def __init__(self, grid_size):
        self.min_x = 0
        self.max_x = 0
        self.min_y = 0
        self.max_y = 0
        self.grid_size = grid_size

    def update_grid(self, min_x, max_x, min_y, max_y):
        if self.min_y > min_y:
            self.min_y = min_y
        if self.min_x > min_x:
            self.min_x = min_x
        if self.max_y < max_y:
            self.max_y = max_y
        if self.max_x < max_x:
            self.max_x = max_x


grid = Grid(4000000)


class Sensor:
    def __init__(self, line: str):
        print(f'Creating Sensor {line}')
        line_components = line.split(':')
        sensor_pos = line_components[0][10:].split(', ')
        beacon_pos = line_components[1][22:].split(', ')
        self.sensor_position: Point = Point(int(sensor_pos[0][2:]), int(sensor_pos[1][2:]))
        self.beacon_position: Point = Point(int(beacon_pos[0][2:]), int(beacon_pos[1][2:]))
        self.min_x: int = self.sensor_position.x if \
            self.sensor_position.x < self.beacon_position.x else \
            self.beacon_position.x
        self.max_x: int = self.sensor_position.x if \
            self.sensor_position.x > self.beacon_position.x else \
            self.beacon_position.x
        self.min_y: int = self.sensor_position.y if \
            self.sensor_position.y < self.beacon_position.y else \
            self.beacon_position.y
        self.max_y: int = self.sensor_position.x if \
            self.sensor_position.y > self.beacon_position.y else \
            self.beacon_position.y

        self.beacon_distance = self.distance(self.beacon_position)
        global max_dist
        if self.beacon_distance > max_dist:
            max_dist = self.beacon_distance

        self.points = []#self.generate_points()
        self.rad_y_min = self.sensor_position.y - self.beacon_distance
        self.rad_x_min = self.sensor_position.x - self.beacon_distance
        self.rad_y_max = self.sensor_position.y + self.beacon_distance
        self.rad_x_max = self.sensor_position.x + self.beacon_distance
        grid.update_grid(self.rad_x_min, self.rad_x_max, self.rad_y_min, self.rad_y_max)
        self.x_ranges: dict[int, range] = {}

    def generate_x_ranges(self, grid_size):
        x_start = self.rad_x_min
        x_end = self.rad_x_max
        y_start = self.rad_y_min
        y_end = self.rad_y_max + 1
        x_ranges = {}
        x_mod = self.beacon_distance
        loop_mod = -1
        for y in range(y_start, y_end):
            if 0 <= y <= grid_size:
                x_s = x_start + x_mod
                x_e = x_end - x_mod
                x_ranges[y] = range(x_s, x_e)
            if x_mod == 0:
                loop_mod = 1
            x_mod += loop_mod
        self.x_ranges = x_ranges


    def between_x(self, x, mod=0) -> bool:
        return self.rad_x_min + mod <= x <= self.rad_x_max - mod

    def between_y(self, y, mod=0) -> bool:
        return self.rad_y_min + mod <= y <= self.rad_y_max - mod

    def distance(self, point: Point) -> int:
        return abs(self.sensor_position.x - point.x) + abs(self.sensor_position.y - point.y)

    def could_contain_beacon(self, point: Point) -> bool:
        return self.distance(point) > self.beacon_distance

    def generate_points(self):
        points = []
        s_x = self.sensor_position.x
        s_y = self.sensor_position.y
        y_mod = 1
        dist = self.beacon_distance
        points.append(Point(s_x - dist, s_y))
        points.append(Point(s_x + dist, s_y))
        reverse = False
        x_size = abs((s_x - dist + 1) - (s_x + dist - 1))
        y_size = abs((s_y - y_mod) - (s_y + y_mod))
        print(f'Generating points for grid size {x_size * y_size}')
        print(f'grid dimensions x:{s_x - dist + 1}-{s_x + dist - 1} | y:{s_y - y_mod}-{s_y + y_mod}')
        point_str = '['
        for x in range(s_x - dist + 1, s_x + dist):
            for y in range(s_y - y_mod, s_y + y_mod + 1):
                point_str += f'Point({x},{y}), '#points.append(Point(x, y))
            if y_mod == dist:
                reverse = True
            y_mod += -1 if reverse else 1
        point_str = point_str[:-2]
        print(point_str)
        print()
        return points

    def points_that_could_not_contain_beacon(self,
                                             points: set[Point]):
        for x in range(0, 21):
            for y in range(0, 21):
                point = Point(x, y)
                if point not in points:
                    return
                if not self.could_contain_beacon(point):
                    points.discard(point)

    def print_radius(self, sensor_points, beacon_points, pre_gen=False):
        print(f'printing grid for sensor {self.sensor_position}')
        print(f'with beacon at {self.beacon_position} with a distance of {self.beacon_distance}')
        lines = []
        distance = int(self.beacon_distance)
        x_s = self.sensor_position.x - distance - 1
        x_e = self.sensor_position.x + distance + 1
        y_s = self.sensor_position.y - distance - 1
        y_e = self.sensor_position.y + distance + 1
        for y in range(y_s, y_e + 1):
            line = ''
            for x in range(x_s, x_e + 1):
                point = Point(x, y)
                if point in sensor_points:
                    line += 'S'
                elif point in beacon_points:
                    line += 'B'
                else:
                    if pre_gen:
                        line += '#' if point in self.points else '.'
                    else:
                        line += '.' if self.could_contain_beacon(point) else '#'
            lines.append(line)
        print('\n'.join(lines))


def __get_grid_labels(value, vertical=True) -> tuple[str, str, str] | str:
    minus = ' '
    if value < 0:
        minus = '-'
        value *= -1
    ten = str(value // 10) if value >= 10 else ' '
    zero = str(value % 10)
    if vertical:
        return minus, ten, zero
    return minus + ten + zero


def print_grid(points, sensor_points, beacon_points, grid_size: None | int = None):
    if grid_size is None:
        y_start = min_y - max_dist
        y_end = max_y + max_dist
        x_start = min_x - max_dist
        x_end = max_x + max_dist
    else:
        y_start = 0
        x_start = 0
        x_end = grid_size
        y_end = grid_size

    lines = []

    minus_line = '    '
    ten_line = '    '
    zero_line = '    '

    for x_p in range(x_start, x_end + 1):
        minus, ten, zero = __get_grid_labels(x_p)
        minus_line += minus
        ten_line += ten
        zero_line += zero

    if x_start < 0:
        lines.append(minus_line)

    lines.append(ten_line)
    lines.append(zero_line)
    for y in range(y_start, y_end + 1):
        label = __get_grid_labels(y, False)
        line = label + ' '
        for x in range(x_start, x_end + 1):
            point = Point(x, y)
            if point in sensor_points:
                line += 'S'
            elif point in beacon_points:
                line += 'B'
            elif point in points:
                line += '#'
            else:
                line += '.'
        lines.append(line + ' ' + label)
    if x_start < 0:
        lines.append(minus_line)
    lines.append(ten_line)
    lines.append(zero_line)
    print('\n'.join(lines))
