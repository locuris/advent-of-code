from common.util import get_lines
from common.grid_objects import Point
from day15.sensors import Sensor
import math

file = 'test.txt'
grid_size = 20
sensors = []
sensor_points = set()
beacon_points = set()


def create_sensors(use_grid_size=False):
    global sensors, sensor_points, beacon_points
    lines = get_lines(file, from_main=True)
    for line in lines:
        sensor = Sensor(line, grid_size) if use_grid_size else Sensor(line)
        sensors.append(sensor)
        sensor_points.add(sensor.sensor_position)
        beacon_points.add(sensor.beacon_position)


def part_i():
    create_sensors()
    max_distance = math.ceil(max(sensors, key=lambda s: s.beacon_distance).beacon_distance)
    grid_x_max = max(sensors, key=lambda s: s.max_x).max_x + max_distance
    grid_x_min = min(sensors, key=lambda s: s.min_x).min_x - max_distance

    y = 2000000
    points = set([])
    for x in range(grid_x_min, grid_x_max):
        point = Point(x, y)
        if point in sensor_points or point in beacon_points:
            continue
        for sensor in sensors:
            if not sensor.could_contain_beacon(point):
                points.add(point)
                break

    print(len(points))


def part_ii():
    create_sensors(True)

    possible_points = set()
    impossible_points = set()
    for sensor in sensors:
        sensor.points_that_could_not_contain_beacon(possible_points, impossible_points)
