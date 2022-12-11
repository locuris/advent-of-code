from enum import Enum

monkey_prefix = 7
items_prefix = 18
operations_prefix = 23
test_prefix = 21
true_prefix = 29
false_prefix = 30


class Monkey:
    def __init__(self, string_input):
        lines = string_input.splitlines()
        self.id = int(lines[0][monkey_prefix:-1])
        self.items = []
        starting_items = map(int, lines[1][items_prefix:].split(', '))
        for item in starting_items:
            self.items.append(Item(item))
        operations = lines[2][operations_prefix:].split(' ')
        self.operation_type = OperationType(operations[0])
        operation = operations[1]
        self.operation_value = 0 if operation == 'old' else int(operation)
        self.test = int(lines[3][test_prefix:])
        self.true_id = int(lines[4][true_prefix:])
        self.false_id = int(lines[5][false_prefix:])
        self.true_monkey = None
        self.false_monkey = None
        self.items_inspected = 0
        self.throw_actions = []
        print(f'Monkey {self.id} with test {self.test} and operation {self.operation_type} and value {self.operation_value}')

    def __str__(self):
        _str = f'Monkey {self.id}: '
        for item in self.items:
            _str += str(item) + ', '
        return _str

    def catch_item(self, item):
        self.items.append(item)

    def update_target_monkeys(self, monkeys):
        self.true_monkey = monkeys[self.true_id]
        self.false_monkey = monkeys[self.false_id]

    def throw_items(self):
        for item in self.items:
            item.inspect_item(self.operation_type, self.operation_value)
            self.items_inspected += 1
            self.throw_actions.append(ThrowAction(item, self.true_monkey if item.worry_level % self.test == 0 else self.false_monkey))
        for throw_action in self.throw_actions:
            self.throw_item(throw_action)
        self.throw_actions = []

    def throw_item(self, throw_action):
        self.items.remove(throw_action.item)
        throw_action.monkey.catch_item(throw_action.item)


class Monkeys:
    def __init__(self, monkeys):
        self.monkeys = []
        for monkey in monkeys:
            self.monkeys.append(Monkey(monkey))
        for monkey in self.monkeys:
            monkey.update_target_monkeys(self.monkeys)

    def run_round(self):
        for monkey in self.monkeys:
            monkey.throw_items()

    def print_monkey_items(self):
        for monkey in self.monkeys:
            print(monkey)

    def print_monkey_buisiness(self):
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


class Item:
    def __init__(self, worry_level):
        self.worry_level = worry_level

    def __str__(self):
        return str(self.worry_level)

    def inspect_item(self, operation, value):
        value = self.worry_level if value == 0 else value
        match operation:
            case OperationType.Add:
                self.worry_level += value
            case OperationType.Multiply:
                self.worry_level *= value
        self.worry_level = int(self.worry_level / 3)


class OperationType(Enum):
    Multiply = '*'
    Add = '+'


class ThrowAction:
    def __init__(self, item, monkey):
        self.item = item
        self.monkey = monkey
