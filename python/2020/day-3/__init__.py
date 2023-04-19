from common.grid_objects import Point
from common.data import as_lines


width = 0
height = 0

def ride_the_slope(slope: Point, area: dict[Point, bool]) -> int:
    trees = 0
    current_point = Point(0, 0)
    while current_point.y < height:
        current_point + slope
        if current_point.y >= height:
            break
        if current_point.x >= width:
            new_x_position = current_point.x - width
            current_point = Point(new_x_position, current_point.y)
        if area[current_point]:
            trees += 1
    return trees


def prepare_area() -> dict[Point, bool]:
    input_data = as_lines(False)
    global height
    height = len(input_data)
    global width
    width = 0
    area = {}
    for y, line in enumerate(input_data):
        if width == 0:
            width = len(line)
        for x, char in enumerate(line):
            point = Point(x, y)
            area[point] = True if char == '#' else False
    return area


def part_i():
    area = prepare_area()
    answer = ride_the_slope(Point(3,1), area)
    print(f"Part I: {answer}")


def part_ii():
    area = prepare_area()
    slopes = [Point(1, 1), Point(3, 1),
              Point(5, 1), Point(7, 1), Point(1, 2)]
    answer = 1

    for slope in slopes:
        answer *= ride_the_slope(slope, area)

    print(f"Part II: {answer}")


if __name__ == '__main__':
    part_i()
    part_ii()
