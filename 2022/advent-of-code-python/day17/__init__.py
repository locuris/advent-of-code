from common.grid_objects import Point
from rocks import Rock, Chamber
from common.util import get_input

h_line_rock = Rock([Point(0, 0), Point(1, 0), Point(2, 0), Point(3, 0)])
plus_rock = Rock([Point(0, 1), Point(1, 1), Point(2, 1), Point(1, 2), Point(1, 0)])
reversed_l_rock = Rock([Point(0, 0), Point(1, 0), Point(2, 0), Point(2, 1), Point(2, 2)])
v_line_rock = Rock([Point(0, 0), Point(0, 1), Point(0, 2), Point(0, 3)])
square_rock = Rock([Point(0, 0), Point(1, 0), Point(0, 1), Point(1, 1)])

rocks = [h_line_rock, plus_rock, reversed_l_rock, v_line_rock, square_rock]

gas = get_input('day_17)

chamber = Chamber(7, 2, gas, rocks)

chamber.start_fall()
