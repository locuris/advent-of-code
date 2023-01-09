from common.util import get_lines
from day18.cubes import Cube

filename = 'test'


def generate_cubes() -> [Cube]:
    lines = get_lines(filename, from_main=True)
    cubes = []
    for line in lines:
        cubes.append(Cube(line))
    return cubes


def part_i():
    cubes = generate_cubes()
    total_sides = 0
    for _, cube in enumerate(cubes):
        cube.update_sides_exposed(cubes)
        total_sides += cube.sides_exposed

    print(f'answer {total_sides}')


def part_ii():
    cubes = generate_cubes()
    total_sides = 0
    for _, cube in enumerate(cubes):
        cube.update_sides_exposed(cubes)
        total_sides += cube.sides_exposed

    special_cubes = []
    for cube in cubes:
        if cube.sides_exposed == 6:
            special_cubes.append(cube)
