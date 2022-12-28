from common.util import get_lines
from day10.grid_objects import CPU


def print_result(part_ii):
    lines = get_lines('day_10)
    cpu = CPU(part_ii)
    for line in lines:
        cpu.execute(line)
    print(cpu.print_result())
