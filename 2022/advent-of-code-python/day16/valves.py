import copy
import matplotlib.pyplot as plt
from enum import Enum
from networkx import Graph, draw, all_simple_paths

valve_id_prefix = 6
valve_id_length = 2
flow_rate_prefix = 23
other_valves_prefix = 24
int_id = 1


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

    def __str__(self):
        return self.id

    def __repr__(self):
        return self.id

    def set_joining_valves(self, valves: set):
        for valve in valves:
            if valve.id in self.joining_valves_strings:
                self.joining_valves.add(valve)


class ActionType(Enum):
    Move = 'move'
    Open = 'open'


class Action:
    def __init__(self, action_type: ActionType, valve_id: str):
        self.action_type = action_type
        self.valve_id = valve_id

    def __str__(self):
        return f'{self.action_type.value} {self.valve_id}'

    def __eq__(self, other):
        return self.valve_id == other.valve_id and self.action_type == self.action_type


class Path:
    def __init__(self, total_time: int, valuable_valves: set[str]):
        self.path_value: int = 0
        self.total_time: int = total_time
        self.route: [Action] = []
        self.open_valves: dict[str, bool] = {}
        for valve_id in valuable_valves:
            self.open_valves[valve_id] = False

    def move(self, valve_id: str):
        self.route.append(Action(ActionType.Move, valve_id))
        self.total_time -= 1

    def open(self, valve: Valve):
        self.total_time -= 1
        self.open_valves[valve.id] = True
        self.path_value += self.total_time * valve.flow_rate
        self.route.append(Action(ActionType.Open, valve.id))

    def is_all_valuable_valves_open(self) -> bool:
        for _, v in self.open_valves:
            if not v:
                return False
        return True

    def __eq__(self, other):
        length = len(self.route)
        if length != len(other.route):
            return False
        for idx in range(0, length):
            if self.route[idx] != other.route[idx]:
                return False
        return True


class You:
    def __init__(self, valves: dict[str, Valve], starting_valve: str, minutes_left: int):
        self.valves: dict[str, Valve] = valves
        self.current_valve: Valve = self.valves[starting_valve]
        self.minutes_left: int = minutes_left
        self.pressure_released: int = 0
        self.tunnel_layout: Graph | None = None
        self.paths: set[Path] = set()
        self.valuable_valves: set[str] = set()
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
        graph = Graph()
        graph.add_node(self.current_valve.id, distance_from_start=0, weighted_value=0)
        self.__add_node(self.current_valve, graph)
        self.tunnel_layout = graph
        draw(graph, with_labels=True)
        plt.show()

    def print_simple_paths(self):
        end_valves = set()
        for valve_id in self.valves.keys():
            if self.tunnel_layout.degree[valve_id] == 1:
                end_valves.add(valve_id)
        paths = [{}]
        for path in all_simple_paths(self.tunnel_layout, self.current_valve.id, end_valves):
            new_path = {}
            print(path)
            for n in path:
                new_path[n] = self.valves[n].flow_rate
            paths.append(new_path)

    def generate_paths(self):



    def __add_node(self, current_valve: Valve, graph: Graph, distance_from_start: int = 0, weighted_value: int = 0):
        for valve in current_valve.joining_valves:
            distance = distance_from_start + 1
            weight = weighted_value
            if graph.has_node(valve.id):
                if graph.nodes[valve.id]['distance_from_start'] > distance:
                    graph.nodes[valve.id]['distance_from_start'] = distance
                if not graph.has_edge(current_valve.id, valve.id):
                    graph.add_edge(current_valve.id, valve.id)
                continue

            if valve.flow_rate > 0:
                weight += (valve.flow_rate / distance)
            graph.add_node(valve.id, distance_from_start=distance, weighted_value=weight)
            graph.add_edge(current_valve.id, valve.id)
            self.__add_node(valve, graph, distance, weight)
