from cave import Cave
from common.util import get_lines

lines = get_lines('day_14.txt')
cave = Cave(lines)
cave.start_sand()
