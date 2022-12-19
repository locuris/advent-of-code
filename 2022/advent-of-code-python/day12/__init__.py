from paths import HeightMap, Path
from common.util import get_lines

string_map = get_lines('day_12_example.txt')

height_map = HeightMap(string_map)
height_map.start_path_generation()
smallest_path = height_map.completed_paths[0]
for path in height_map.completed_paths:
    path._print_path()
    if path.length < smallest_path.length:
        smallest_path = path
print(smallest_path.length)
