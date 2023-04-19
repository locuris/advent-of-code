from typing import TypeVar, List

T = TypeVar("T")
V = TypeVar("V")


def __filename(test: bool) -> str:
    return 'test.txt' if test else 'input.txt'


def as_lines(test: bool = False) -> List[str]:
    lines = open(__filename(test)).readlines()
    return [line.replace('\n', '') for line in lines]


def as_tuples(item_1_type: T, item_2_type: V, separator: str, test: bool = False) -> [(T, V)]:
    lines = as_lines(test)
    container = []
    for line in lines:
        tuple_components = line.split(separator)
        item_1 = item_1_type(tuple_components[0])
        item_2 = item_2_type(tuple_components[1])
        container.append((item_1, item_2))
    return container
