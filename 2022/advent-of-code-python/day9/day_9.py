from common.grid_objects import Grid, Point, create_movements
from common.util import get_lines

grid = Grid(Point(0, 0), Point(0, 0))


def part_1():
    head_point = Point(0, 0)
    tail_point = Point(0, 0)
    tail_positions = [tail_point]
    knots = 0
    movements = create_movements(get_lines('day_9_example.txt'))
    for movement in movements:
        for i in range(movement.distance):
            head_point.move_with_grid(movement.direction, grid)  # = move_head_point(movement.direction, head_point)
            if not knot_adjacent(tail_point, head_point):
                tail_point = move_knot_point(tail_point, head_point)
                knots += 1
                if not tail_positions.__contains__(tail_point):
                    tail_positions.append(tail_point)

    print_grid(tail_positions, grid)
    print(len(tail_positions))

def knot_adjacent(tail_point, head_point):
    return not (abs(head_point.x - tail_point.x) > 1 or abs(head_point.y - tail_point.y) > 1)


def move_knot_point(tail_point, head_point):
    x = tail_point.x
    y = tail_point.y
    if head_point.x > x:
        x += 1
    elif head_point.x < x:
        x -= 1
    if head_point.y > y:
        y += 1
    elif head_point.y < y:
        y -= 1
    return Point(x, y)


def print_grid(tail_positions, local_grid):
    output = ''
    for y in range(local_grid.start.y, local_grid.end.y + 1):
        line = ''
        for x in range(local_grid.start.x, local_grid.end.x + 1):
            point = Point(x, y)
            if tail_positions.__contains__(point):
                if tail_positions[0] == point:
                    line += 's'
                else:
                    line += '#'
            else:
                line += '.'
        line = line[::-1]
        output += line + '\n'
    print(output[::-1])


def print_grid_motions(knot_points, local_grid):
    output = ''
    points_with_knots = []
    for y in range(local_grid.start.y, local_grid.end.y + 1):
        line = ''
        for x in range(local_grid.start.x, local_grid.end.x + 1):
            point = Point(x, y)
            if not points_with_knots.__contains__(point):
                if knot_points.__contains__(point):
                    points_with_knots.append(point)
                    line += str(knot_points.index(point)) if not knot_points[0] == point else 'H'
                elif point == Point(0, 0):
                    line += 's'
                else:
                    line += '.'
        output += line[::-1] + '\n'
    print(output[::-1])

# def set_grid_size(movements):
#     for movement in movements:
#         update_grid_size(movement)
#
#
# def update_grid_size(movement):
#     match movement.direction:
#         case Direction.Up:
#             if movement.distance.
