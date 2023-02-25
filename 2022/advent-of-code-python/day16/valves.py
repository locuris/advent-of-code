import copy
from enum import Enum

from anytree import AnyNode, RenderTree
import networkx as nx

valve_id_prefix = 6
valve_id_length = 2
flow_rate_prefix = 23
other_valves_prefix = 24


class Valve:
    def __init__(self, input_string: str):
        valve_input_components = input_string.split(';')
        self.id: str = valve_input_components[0][valve_id_prefix:valve_id_length + valve_id_prefix]
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
        self.tunnel_layout: AnyNode | None = None
        self.paths: set[Path] = set()
        self.valuable_valves: set[str] = set()
        for k, v in self.valves.items():
            if v.flow_rate > 0:
                self.valuable_valves.add(k)
        self.tunnel_graph: nx.Graph | None = None

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
        graph = nx.Graph()
        for id, valve in self.valves.items():
            node = graph.add_node(id)

        for id, valve in self.valves.items()
            for j_valve in valve.joining_valves:
                nx.add_path(graph, )
        # paths = AnyNode(id=self.current_valve.id)
        # # noinspection PyTypeChecker
        # self.__add_node(self.current_valve, paths, self.minutes_left, 0)
        # print(RenderTree(paths))
        # self.tunnel_layout = paths

    def __add_node(self, current_valve: Valve, current_node: AnyNode, minutes_left: int, previous_node_value: int,
                   weight: int = 1, previous_summed_weighted_value: int = 0):

        for valve in current_valve.joining_valves:
            if valve.id == current_node.id or valve.id in [n.id for n in current_node.ancestors]:
                continue
            current_minutes = minutes_left - 1
            node_value = previous_node_value
            current_weight = weight
            weighted_value = 0
            if valve.flow_rate > 0:
                current_minutes -= 1
                node_value += current_minutes * valve.flow_rate
                weighted_value = node_value / current_weight
            summed_weighted_value = previous_summed_weighted_value + weighted_value
            node = AnyNode(id=valve.id, flow_rate=valve.flow_rate, optimal_value=node_value, wieght=current_weight,
                           weighted_value=weighted_value, summed_weighted_value=summed_weighted_value, parent=current_node)
            current_weight += 1
            # noinspection PyTypeChecker
            self.__add_node(valve, node, current_minutes, node_value, current_weight, summed_weighted_value)

    def generate_paths(self):
        start = self.tunnel_layout.root
        path = Path(self.minutes_left, self.valuable_valves)
        self.__generate_path(start, path)

    def __generate_path(self, current_node: AnyNode, path: Path):
        next_valve = None
        highest_value = 0
        for next in current_node.children:
            if path.open_valves[next.id]:
                continue
            if next.summed_weighted_value > highest_value:
                next_valve = next
                highest_value = next.summed_weighted_value

        if next_valve is None:
            path.move()


    # def generate_path(self, current_valve: Valve, previous_path: None | Path = None):
    #     for valve in current_valve.joining_valves:
    #         path = Path(self.minutes_left, self.valuable_valves) if previous_path is None else copy.copy(previous_path)
    #         #if valve

