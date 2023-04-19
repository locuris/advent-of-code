from common.grid_objects import Point, Grid, create_movements
from day9.day_9 import knot_adjacent, print_grid
from common.util import get_lines

grid = Grid(Point(0, 0), Point(0, 0))


def part_ii():
    lines = get_lines('day_9)
    knot_points = [Point(0, 0), Point(0, 0), Point(0, 0), Point(0, 0), Point(0, 0), Point(0, 0), Point(0, 0),
                   Point(0, 0), Point(0, 0), Point(0, 0)]
    tail_points = [Point(0, 0)]
    movements = create_movements(lines)
    grid.calculate_size(movements)

    for movement in movements:
        for i in range(movement.distance):
            knot_points[0].move(movement.direction)
            previous_knot = knot_points[0]
            for knot in knot_points[1:]:
                if not knot_adjacent(knot, previous_knot):
                    knot.move_to_adjacent(previous_knot)
                previous_knot = knot
                tail = knot_points[-1]
                if not tail_points.__contains__(tail):
                    tail_points.append(Point(tail.x, tail.y))
        #print_grid_motions(knot_points, grid)

    #for tail in tail_points:
#        print(tail)
    print_grid(tail_points, grid)
    print(len(tail_points))
