from common.util import get_lines
from common.grid_objects import Point
from day15.sensors import Sensor, print_grid
import math

file = 'test'
key = 4000000


def create_sensors():
    sensors = []
    sensor_points = set()
    beacon_points = set()
    lines = get_lines(file, from_main=True)
    for line in lines:
        sensor = Sensor(line)
        sensors.append(sensor)
        sensor_points.add(sensor.sensor_position)
        beacon_points.add(sensor.beacon_position)
    return sensors, sensor_points, beacon_points


def part_i():
    sensors, sensor_points, beacon_points = create_sensors()
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
    sensors, sensor_points, beacon_points = create_sensors()

    points = set()

    for sensor in sensors:
        points.update(sensor.points)

    points.update(sensor_points)
    points.update(beacon_points)

    grid_size = 20 if file == 'test' else key
    print(f'points to check: {len(points)}')
    print(f'grid size: {grid_size * grid_size}')

    possible_points = []
    column = 0
    for x in range(0, grid_size + 1):
        print(f'Checking column {column}')
        column += 1
        for y in range(0, grid_size + 1):
            possible_point = True
            for sensor in sensors:
                if not sensor.between_x(x) or not sensor.between_y(y):
                    continue
                point = sensor.sensor_position
                if point.x == x and point.y == y:
                    possible_point = False
                    break
                y_mod = abs(y - point.y)
                x_mod = abs(x - point.x)
                if sensor.between_y(y, x_mod) and sensor.between_x(x, y_mod):
                    possible_point = False
                    break
            if possible_point:
                possible_points.append(Point(x, y))

    possible_points = [p for p in possible_points if p not in sensor_points and p not in beacon_points]

    answer = 'answer: '
    for point in possible_points:
        freq = (point.x * key) + point.y
        answer += f'| {point} = {freq} '
    print(answer)
