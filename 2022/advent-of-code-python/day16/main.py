from valves import Valve, You
from common.util import get_lines

filename = 'test'
starting_valve = 'AA'
minutes_left = 30


def create_valves() -> dict[str, Valve]:
    lines = get_lines(filename, from_main=True)
    valves = set()
    for line in lines:
        valves.add(Valve(line))

    valve_dict = {}
    for valve in valves:
        valve.set_joining_valves(valves)
        valve_dict[valve.id] = valve

    return valve_dict


def part_i():
    valves = create_valves()
    you = You(valves, starting_valve, minutes_left)

    you.generate_tunnel_layout()
    you.print_simple_paths()
    you.print_all_paths()

    you.generate_paths()

    for path in you.paths:
        path.update_path_value()

    best_path = max(you.paths, key=lambda p: p.path_value)

    print(f'Pressure released: {best_path.path_value}')
