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
        self.open = False
        self.degree = 0

    def __str__(self):
        return self.id

    def __repr__(self):
        return self.id

    def __hash__(self):
        return hash(self.id)

    def set_joining_valves(self, valves: set):
        for valve in valves:
            if valve.id in self.joining_valves_strings:
                self.joining_valves.add(valve)


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
            if self.visited_valves[valve.id] > valve.degree:
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


class You:
    def __init__(self, valves: dict[str, Valve], starting_valve: str, minutes_left: int):
        self.valves: dict[str, Valve] = valves
        self.current_valve: Valve = self.valves[starting_valve]
        self.starting_valve_id: str = starting_valve
        self.minutes_left: int = minutes_left
        self.pressure_released: int = 0
        self.tunnel_layout: graph_to_use | None = None
        self.paths: set[Path] = set()
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
            print(path)

    def __update_valve_distance_and_weight(self, graph: graph_to_use, current_valve: Valve):
        distance = maxsize
        weight = 0
        for path in all_simple_paths(graph, self.starting_valve_id, current_valve.id):
            path_len = len(path)
            if path_len < distance:
                distance = path_len
        if distance == maxsize:
            distance = 1
        if current_valve.flow_rate > 0:
            weight = (current_valve.flow_rate / distance)
        graph.nodes[current_valve.id]['distance_from_start'] = distance - 1
        graph.nodes[current_valve.id]['weighted_value'] = weight

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
