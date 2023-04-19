import copy
import matplotlib.pyplot as plt
from enum import Enum
from networkx import draw, all_simple_paths, all_simple_edge_paths, MultiGraph, Graph
from sys import maxsize

valve_id_prefix = 6
valve_id_length = 2
flow_rate_prefix = 23
other_valves_prefix = 24
int_id = 1

use_multigraph = False
graph_to_use = Graph
highest_path_value = 0


class Valve:
    def __init__(self, input_string: str):
        valve_input_components = input_string.split(';')
        self.id: str = valve_input_components[0][valve_id_prefix:valve_id_length + valve_id_prefix]
        global int_id
        self.int_id: int = int_id
        int_id += 1
        self.flow_rate: int = int(valve_input_components[0][flow_rate_prefix:])
        jvs = valve_input_components[1][other_valves_prefix:].replace(' ', '')
        self.joining_valves_strings: list[str] = jvs.split(',')
        self.joining_valves: set[Valve] = set()
        self.open: bool = False
        self.degree: int = 0
        self.optimal_value: int = 0

    def __str__(self):
        return self.id

    def __repr__(self):
        return self.id

    def __hash__(self):
        return hash(self.id)

    def __eq__(self, other):
        return self.id == other.id

    def __contains__(self, item):
        return self.id == item.id

    def set_joining_valves(self, valves: set):
        for valve in valves:
            if valve.id in self.joining_valves_strings:
                self.joining_valves.add(valve)

    def potential_release(self, time_left: int) -> int:
        return self.flow_rate * time_left


class ActionType(Enum):
    Move = 'move'
    Open = 'open'


class Action:
    def __init__(self, action_type: ActionType, valve_id: str, pressure_value: int = 0):
        self.action_type = action_type
        self.valve_id = valve_id
        self.pressure_value = pressure_value

    def __str__(self):
        return f'({self.action_type.value} {self.valve_id} {self.pressure_value})'

    def __eq__(self, other):
        return self.valve_id == other.valve_id and self.action_type == self.action_type


class Path:
    def __init__(self, total_time: int, starting_valve: Valve):
        self.path_value: int = 0
        self.starting_valve: Valve = starting_valve
        self.current_valve: Valve = starting_valve
        self.total_time: int = total_time
        self.route: [Action] = []
        self.visited_valves: dict[str, int] = {}
        self.opened_valves: dict[str, bool] = {}

    def __str__(self):
        actions = []
        for action in self.route:
            actions.append(f'{action}')
        actions.append(f'T: {self.path_value}')
        return ', '.join(actions)

    def __repr__(self):
        return f'time: {self.total_time} value: {self.path_value}'

    def copy(self):
        new_path = Path(self.total_time, self.starting_valve)
        new_path.current_valve = self.current_valve
        new_path.route = copy.copy(self.route)
        new_path.visited_valves = copy.copy(self.visited_valves)
        new_path.opened_valves = copy.copy(self.opened_valves)
        new_path.update_path_value()
        return new_path

    def move(self, valve: Valve) -> bool:
        if valve.id in self.visited_valves.keys():
            if self.visited_valves[valve.id] >= valve.degree:
                return False
            else:
                self.visited_valves[valve.id] += 1
        else:
            self.visited_valves[valve.id] = 1
        self.route.append(Action(ActionType.Move, valve.id))
        self.current_valve = valve
        self.total_time -= 1
        return self.total_time > 0

    def open(self) -> bool:
        if self.current_valve.id in self.opened_valves.keys():
            if self.opened_valves[self.current_valve.id]:
                return False
        self.total_time -= 1
        self.opened_valves[self.current_valve.id] = True
        step_value = self.total_time * self.current_valve.flow_rate
        self.path_value += step_value
        self.route.append(Action(ActionType.Open, self.current_valve.id, step_value))
        return self.total_time > 0

    def current_valve_is_open(self):
        if self.current_valve.flow_rate <= 0:
            self.opened_valves[self.current_valve.id] = True
        try:
            return self.opened_valves[self.current_valve.id]
        except KeyError:
            return False

    def update_path_value(self):
        self.path_value = 0
        for action in self.route:
            self.path_value += action.pressure_value

    def all_valves_are_open(self, valuable_valves: set[str]) -> bool:
        for valve in valuable_valves:
            try:
                if not self.opened_valves[valve]:
                    return False
            except KeyError:
                return False
        return True

    def not_worth_continuing(self, valves: dict[str, Valve]) -> bool:
        sorted_valves = []
        for valve_id, valve in valves.items():
            if (valve_id not in self.opened_valves.keys() or not self.opened_valves[valve_id]) and valve.flow_rate > 0:
                sorted_valves.append(valve)

        minutes_left = self.total_time
        sorted_valves = sorted(sorted_valves, key=lambda v: v.flow_rate)
        potential_pressure = self.path_value
        for valve in sorted_valves:
            minutes_left -= 1
            potential_pressure += valve.flow_rate * minutes_left

        return potential_pressure <= highest_path_value


class SimplePath:
    def __init__(self, input_string: str):
        self.path_string = input_string
        path_components = input_string.split(',')
        self.steps: [tuple[str, float]] = []
        self.total: int = 0
        for component in path_components:
            sub_components = component.split('|')
            key = sub_components[0]
            value = sub_components[1]
            if not key == 'total':
                self.steps.append((key, float(value)))
            else:
                self.total = float(value)

    def __repr__(self):
        return self.path_string

    def __str__(self):
        lines = []
        for step in self.steps:
            lines.append(f'[{step[0]}:{step[1]}]')
        lines.append(f'total: {self.total}')
        return ', '.join(lines)

    def __hash__(self):
        return hash(str(self))

    def __eq__(self, other):
        return str(self) == str(other)

    def contains_valve(self, valve: Valve):
        for step in self.steps:
            if step[0] == valve.id:
                return True
        return False

    def valve_value(self, valve: Valve):
        for step in self.steps:
            if step[0] == valve.id:
                return step[1]

    # def generate_alternative_paths(self):
    #     new_paths = []
    #     for step in self.steps:
    #         `


class You:
    def __init__(self, valves: dict[str, Valve], starting_valve: str, minutes_left: int):
        self.valves: dict[str, Valve] = valves
        self.current_valve: Valve = self.valves[starting_valve]
        self.starting_valve_id: str = starting_valve
        self.minutes_left: int = minutes_left
        self.pressure_released: int = 0
        self.tunnel_layout: graph_to_use | None = None
        self.paths: set[Path] = set()
        self.simple_paths: set[SimplePath] = set()
        self.valuable_valves: set[str] = set()
        self.sub_paths: set[set[str]] = set()
        for k, v in self.valves.items():
            if v.flow_rate > 0:
                self.valuable_valves.add(k)

    def move(self, target_valve: str):
        self.current_valve = self.valves[target_valve]
        self.minutes_left -= 1

    def open(self):
        self.current_valve.open = True
        self.minutes_left -= 1
        self.pressure_released += self.minutes_left * self.current_valve.flow_rate

    def wait(self):
        self.minutes_left -= 1

    def generate_tunnel_layout(self):
        graph = graph_to_use()
        graph.add_node(self.current_valve, distance_from_start=0, weighted_value=0)
        self.__add_node(self.current_valve, graph)
        self.tunnel_layout = graph
        #self.__update_valve_distance_and_weight()
        self.__update_valve_weight()
        self.__generate_simple_paths()
        self.__print_simple_paths()
        draw(graph, with_labels=True)
        plt.show()
        for _, v in self.valves.items():
            v.degree = graph.degree[v]

    def __add_node(self, current_valve: Valve, graph: graph_to_use):
        for valve in current_valve.joining_valves:
            if graph.has_node(valve):
                if not graph.has_edge(current_valve, valve):
                    graph.add_edge(current_valve, valve)
                continue
            graph.add_node(valve)
            graph.add_edge(current_valve, valve)
            self.__add_node(valve, graph)

    def print_simple_paths(self, use_edge: bool = False):
        end_valves = set()
        for valve_id, valve in self.valves.items():
            if self.tunnel_layout.degree[valve] == 1:
                end_valves.add(valve)
        path_function = all_simple_edge_paths if use_edge else all_simple_paths
        for path in path_function(self.tunnel_layout, self.current_valve, end_valves):
            print(path)

    def print_all_paths(self):
        all_valves = set()
        for valve_id, valve in self.valves.items():
            if not valve_id == self.starting_valve_id:
                all_valves.add(valve)
        for path in all_simple_paths(self.tunnel_layout, self.current_valve, all_valves):
            path_string = ''
            path_value = 0
            last_path = path[len(path) - 1]
            for valve in path:
                current_node = self.tunnel_layout.nodes[valve]
                try:
                    weight = current_node[f'weight-{last_path.id}']
                    path_value += weight
                    path_string += f'[{valve.id}|{weight}], '
                except KeyError:
                    path_string += '[AA], '
            print(f'{path_string}[total:{path_value}]')

    def __generate_simple_paths(self):
        all_valves = set()
        for valve_id, valve in self.valves.items():
            if not valve_id == self.starting_valve_id:
                all_valves.add(valve)
        for path in all_simple_paths(self.tunnel_layout, self.current_valve, all_valves):
            path_string = ''
            path_value = 0
            last_path = path[len(path) - 1]
            for valve in path:
                current_node = self.tunnel_layout.nodes[valve]
                try:
                    weight = current_node[f'weight-{last_path.id}']
                    path_value += weight
                    path_string += f'{valve.id}|{weight},'
                except KeyError:
                    pass
            path_string += f'total|{path_value}'
            self.simple_paths.add(SimplePath(path_string))
        for _, valve in self.valves.items():
            for path in self.simple_paths:
                if path.contains_valve(valve):
                    valve_value = path.valve_value(valve)
                    if valve.optimal_value < valve_value:
                        valve.optimal_value = valve_value

    def __print_simple_paths(self):
        for simple_path in self.simple_paths:
            print(simple_path)

    def __update_valve_distance_and_weight(self):
        starting_valve = self.valves[self.starting_valve_id]
        for _, current_valve in self.valves.items():
            if current_valve == starting_valve:
                continue
            for path in all_simple_paths(self.tunnel_layout, starting_valve, current_valve):
                for idx, valve in enumerate(path):
                    if valve == starting_valve:
                        continue
                    weight = (29 - idx) * valve.flow_rate if not valve.flow_rate == 0 else 0
                    self.tunnel_layout.nodes[valve][f'weight-{current_valve.id}'] = weight

    def __update_valve_weight(self):
        paths_from_start = []
        for _, non_start_valve in self.valves.items():
            if non_start_valve == self.current_valve:
                continue
            simple_paths_from_start = all_simple_paths(self.tunnel_layout, self.current_valve, non_start_valve)
            for simple_path_from_start in simple_paths_from_start:
                print(simple_path_from_start)
                paths_from_start.append(simple_path_from_start)
        for _, start_valve in self.valves.items():
            for _, end_valve in self.valves.items():
                if start_valve == end_valve:
                    continue
                for path in all_simple_paths(self.tunnel_layout, start_valve, end_valve):
                    for idx, valve in enumerate(path):
                        if valve.flow_rate == 0:
                            self.tunnel_layout.nodes[valve][weight_node_key(start_valve, end_valve, False)] = 0
                            self.tunnel_layout.nodes[valve][weight_node_key(start_valve, end_valve, True)] = 0
                            continue
                        paths_with_valve = [p for p in paths_from_start if valve in p and valve == p[-1]]
                        longest_length = len(max(paths_with_valve, key=len))
                        shortest_length = len(min(paths_with_valve, key=len))
                        self.tunnel_layout.nodes[valve][weight_node_key(start_valve, end_valve, False)] =\
                            (longest_length - 1) * valve.flow_rate
                        self.tunnel_layout.nodes[valve][weight_node_key(start_valve, end_valve, True)] =\
                            (shortest_length - 1) * valve.flow_rate

    def generate_paths(self):
        paths: set[Path] = set()
        starting_valve = self.valves[self.starting_valve_id]
        paths.add(Path(self.minutes_left, starting_valve))
        self.__generate_path(paths)
        print('Paths generated')

    def __generate_path(self, paths: set[Path]):
        if len(self.paths) >= 10000:
            pass
        if len(paths) == 0:
            return
        new_paths: set[Path] = set()
        for path in paths:
            if path.all_valves_are_open(self.valuable_valves):
                self.__add_path(path)
                continue
            elif path.not_worth_continuing(self.valves):
                continue
            for next_valve in path.current_valve.joining_valves:
                new_path = path.copy()
                if not new_path.move(next_valve):
                    self.__add_path(new_path)
                    continue
                new_paths.add(new_path)
            if not path.current_valve_is_open():
                new_path = path.copy()
                if not new_path.open():
                    self.__add_path(new_path)
                    continue
                for next_valve in path.current_valve.joining_valves:
                    new_path = new_path.copy()
                    if not new_path.move(next_valve):
                        self.__add_path(new_path)
                        continue
                    new_paths.add(new_path)
        self.__generate_path(new_paths)

    def __add_path(self, path: Path):
        global highest_path_value
        if path.path_value > highest_path_value:
            highest_path_value = path.path_value
            self.paths.add(path)
            print(f'Path added {len(self.paths)}')

    # def best_path(self):
    #     next_valve = self.current_valve
    #     for valve in self.current_valve.joining_valves:
    #         if valve.flow_rate != 0 and not valve.open:
    #             best_valve = valve
    #             for v in valve.joining_valves:
    #                 if v.potential_release(self.minutes_left - 2) > best_valve.p


def weight_node_key(start_valve: Valve, end_valve: Valve, shortest: bool):
    weight_type = 'short' if shortest else 'long'
    return f'weight-{weight_type}-{start_valve.id}-{end_valve.id}'
