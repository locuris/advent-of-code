from common.data import as_tuples
from common.grid_objects import Point


DEBUG = False


def part_i():
    course_instructions = as_tuples(str, int, ' ', DEBUG)
    start_point = Point(0, 0)
    for instruction in course_instructions:
        match instruction[0]:
            case 'forward':
                start_point.move_x(instruction[1])
                continue
            case 'up':
                start_point.move_y(instruction[1] * -1)
                continue
            case 'down':
                start_point.move_y(instruction[1])

    answer = start_point.x * start_point.y
    print(answer)


def part_ii():
    course_instructions = as_tuples(str, int, ' ', DEBUG)
    start_point = Point(0, 0)
    aim = 0
    for instruction in course_instructions:
        match instruction[0]:
            case 'forward':
                start_point.move_x(instruction[1])
                start_point.move_y(instruction[1] * aim)
                continue
            case 'up':
                aim += (instruction[1] * -1)
                continue
            case 'down':
                aim += instruction[1]

    answer = start_point.x * start_point.y
    print(answer)


if __name__ == '__main__':
    part_i()
    part_ii()

