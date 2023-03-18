from common.grid_objects import Point
from common.data import as_lines


def part_i():
    test_input = as_lines(False)
    height = len(test_input)
    area = {}
    width = 0
    answer = 1

    slopes = [Point(1, 1), Point(3, 1),
              Point(5, 1), Point(7, 1), Point(1, 2)]

    for slope in slopes:
        for y, line in enumerate(test_input):
            if width == 0:
                width = len(line)
            for x, char in enumerate(line):
                point = Point(x, y)
                area[point] = True if char == '#' else False

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
        answer *= trees

        print(trees)
    print("answer is " + str(answer))


if __name__ == '__main__':
    part_i()