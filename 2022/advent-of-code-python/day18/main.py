from common.util import get_lines
from day18.cubes import Cube

filename = 'input'


def part_i():
    lines = get_lines(filename, from_main=True)
    cubes = []
    for line in lines:
        cubes.append(Cube(line))

    total_sides = 0
    for _, cube in enumerate(cubes):
        cube.update_sides_exposed(cubes)
        total_sides += cube.sides_exposed

    print(f'answer {total_sides}')
