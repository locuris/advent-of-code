from common.util import get_lines
from common.grid_objects import Point
from day15.sensors import Sensor
import math

file = 'input'
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

    grid_size = 20 if file == 'test' else key

    sensor_id = 1

    for sensor in sensors:
        print(f'generating x_ranges for sensor {sensor_id}')
        sensor_id += 1
        sensor.generate_x_ranges(grid_size)

    possible_points = set()

    for y in range(0, grid_size + 1):
        range_points_by_start = []
        for sensor in sensors:
            if y not in sensor.x_ranges.keys():
                continue
            x_range = sensor.x_ranges[y]
            range_points_by_start.append(x_range)
        range_points_by_start = sorted(range_points_by_start, key=lambda r: r.start)
        ranges_lengths = len(range_points_by_start)
        stop = range_points_by_start[0].stop
        for idx in range(1, ranges_lengths):
            new_start = range_points_by_start[idx].start
            new_stop = range_points_by_start[idx].stop
            if new_start <= stop + 1 <= new_stop:
                stop = new_stop
        if stop < grid_size:
            possible_points.add(Point(stop + 1, y))

    answer = f'answer: ({len(possible_points)})'
    for point in possible_points:
        freq = (point.x * key) + point.y
        answer += f'| {point} = {freq} '
    print(answer)
