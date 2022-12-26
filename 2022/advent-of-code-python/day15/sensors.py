from typing import Type, Set

from common.grid_objects import Point
import math


class Sensor:
    def __init__(self, line: str, grid_size: None | int = None):
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
        self.grid_size = grid_size

    def distance(self, point: Point) -> int:
        return abs(self.sensor_position.x - point.x) + abs(self.sensor_position.y - point.y)

    def could_contain_beacon(self, point: Point) -> bool:
        return self.distance(point) > self.beacon_distance

    def points_that_could_not_contain_beacon(self,
                                             possible_points: set[Point],
                                             impossible_points: set[Point]):
        x_range = self.sensor_position.x - self.beacon_distance
        x_start = 0 if x_range <= 0 else x_range
        x_range += self.beacon_distance * 2
        x_end = self.grid_size + 1 if x_range >= self.grid_size else x_range + 1
        y_range = self.sensor_position.y - self.beacon_distance
        y_start = 0 if y_range <= 0 else y_range
        y_range += self.beacon_distance * 2
        y_end = self.grid_size + 1 if y_range >= self.grid_size else y_range + 1

        for x in range(x_start, x_end):
            for y in range(y_start, y_end):
                point = Point(x, y)
                if point in impossible_points:
                    return
                if not self.could_contain_beacon(point):
                    possible_points.discard(point)
                    impossible_points.add(point)
                elif point not in impossible_points:
                    possible_points.add(point)

    def print_radius(self, sensor_points, beacon_points):
        print(f'printing grid for sensor {self.sensor_position}')
        print(f'with beacon at {self.beacon_position} with a distance of {self.beacon_distance}')
        lines = []
        distance = int(self.beacon_distance)
        x_s = self.sensor_position.x - distance - 1
        x_e = self.sensor_position.x + distance + 1
        y_s = self.sensor_position.y - distance - 1
        y_e = self.sensor_position.y + distance + 1
        for y in range(y_s, y_e):
            line = ''
            for x in range(x_s, x_e):
                point = Point(x, y)
                if point in sensor_points:
                    line += 'S'
                elif point in beacon_points:
                    line += 'B'
                else:
                    line += '.' if self.could_contain_beacon(point) else '#'
            lines.append(line)
        print('\n'.join(lines))
