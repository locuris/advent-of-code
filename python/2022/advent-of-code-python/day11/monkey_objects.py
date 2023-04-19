from enum import Enum
from typing import Final
from decimal import Decimal

MONKEY_PREFIX: Final[int] = 7
ITEMS_PREFIX: Final[int] = 18
OPERATIONS_PREFIX: Final[int] = 23
TEST_PREFIX: Final[int] = 21
TRUE_PREFIX: Final[int] = 29
FALSE_PREFIX: Final[int] = 30


class Item:
    def __init__(self, worry_level, item_id):
        self.id = item_id
        self.worry_level = worry_level

    def __str__(self):
        return f'[id:{self.id}: ({self.worry_level})]'

    def inspect_item(self, operation, value, test, magic_number):
        value = value if value is not None else self.worry_level
        old = 0
        match operation:
            case OperationType.Add:
                old = (self.worry_level + value)
            case OperationType.Multiply:
                old = (self.worry_level * value)
        self.worry_level = old % magic_number
        return old % test == 0


class Monkey:
    def __init__(self, string_input: str, starting_item_id: int):
        lines = string_input.splitlines()

        self.id: int = int(lines[0][MONKEY_PREFIX:-1])

        self.items: list[Item] = []
        starting_items = map(int, lines[1][ITEMS_PREFIX:].split(', '))
        for item in starting_items:
            self.items.append(Item(item, starting_item_id))
            starting_item_id += 1
        self.last_item_id: int = starting_item_id

        operations = lines[2][OPERATIONS_PREFIX:].split(' ')
        self.operation_type: OperationType = OperationType(operations[0])
        operation = operations[1]
        use_old_value_for_operations = operation == 'old'
        self.operation_value = None if use_old_value_for_operations else int(operation)
        self.test = int(lines[3][TEST_PREFIX:])

        self.true_id = int(lines[4][TRUE_PREFIX:])
        self.false_id = int(lines[5][FALSE_PREFIX:])
        self.true_monkey = None
        self.false_monkey = None
        self.items_inspected = 0
        self.throw_actions = []
        self.magic_number = 1

    def __str__(self):
        _str = f'Monkey {self.id}: '
        for item in self.items:
            _str += str(item) + ', '
        return _str

    def catch_item(self, item):
        self.items.append(item)

    def update_target_monkeys(self, monkeys, magic_number):
        self.true_monkey = monkeys[self.true_id]
        self.false_monkey = monkeys[self.false_id]
        self.magic_number = magic_number

    def throw_items(self):
        for item in self.items:
            target_monkey = self.true_monkey \
                if item.inspect_item(self.operation_type, self.operation_value, self.test, self.magic_number) \
                else self.false_monkey
            self.items_inspected += 1
            self.throw_actions.append(ThrowAction(item, target_monkey))
        for throw_action in self.throw_actions:
            self.throw_item(throw_action)
        self.throw_actions = []

    def throw_item(self, throw_action):
        self.items.remove(throw_action.item)
        throw_action.monkey.catch_item(throw_action.item)


class Monkeys:
    def __init__(self, monkeys: str):
        self.monkeys = []
        self.magic_number = 1
        item_id = 1
        for monkey_string in monkeys:
            monkey = Monkey(monkey_string, item_id)
            self.monkeys.append(monkey)
            item_id = monkey.last_item_id
            self.magic_number *= monkey.test
        for monkey in self.monkeys:
            monkey.update_target_monkeys(self.monkeys, self.magic_number)
        self.__round_number = 1

    def run_round(self):
        self.__round_number += 1
        for monkey in self.monkeys:
            monkey.throw_items()

    def print_monkey_items(self):
        for monkey in self.monkeys:
            print(monkey)
        print()

    def print_monkey_inspections(self):
        for monkey in self.monkeys:
            print(f'Monkey {monkey.id} inspected items {monkey.items_inspected} times.')

    def print_monkey_business(self):
        self.print_monkey_inspections()
        first = 0
        second = 0
        for monkey in self.monkeys:
            inspected = monkey.items_inspected
            if inspected > first:
                if first > second:
                    second = first
                first = inspected
            elif inspected > second:
                second = inspected
        print(str(first * second))


class OperationType(Enum):
    Multiply = '*'
    Add = '+'


class ThrowAction:
    def __init__(self, item, monkey):
        self.item = item
        self.monkey = monkey
