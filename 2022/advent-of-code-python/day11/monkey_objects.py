from enum import Enum

monkey_prefix = 7
items_prefix = 18
operations_prefix = 23
test_prefix = 21
true_prefix = 29
false_prefix = 30

worried_factor = 100
default_old_value = 0


class Monkey:
    def __init__(self, string_input, worried, starting_item_id):
        lines = string_input.splitlines()
        self.id = int(lines[0][monkey_prefix:-1])
        self.items = []
        starting_items = map(int, lines[1][items_prefix:].split(', '))
        for item in starting_items:
            self.items.append(Item(item, worried, starting_item_id))
            starting_item_id += 1
        self.last_item_id = starting_item_id
        operations = lines[2][operations_prefix:].split(' ')
        self.operation_type = OperationType(operations[0])
        operation = operations[1]
        self.operation_value = default_old_value if operation == 'old' else int(operation)
        self.test = int(lines[3][test_prefix:])
        self.true_id = int(lines[4][true_prefix:])
        self.false_id = int(lines[5][false_prefix:])
        self.true_monkey = None
        self.false_monkey = None
        self.items_inspected = 0
        self.throw_actions = []
        print(
            f'Monkey {self.id} with test {self.test} and operation {self.operation_type} and value {self.operation_value}')

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
            item.inspect_item(self.operation_type, self.operation_value, self.id)
            self.items_inspected += 1
            self.throw_actions.append(ThrowAction(item, self.true_monkey if item.is_worried(self.test) else self.false_monkey))
        for throw_action in self.throw_actions:
            self.throw_item(throw_action)
        self.throw_actions = []

    def throw_item(self, throw_action):
        self.items.remove(throw_action.item)
        throw_action.monkey.catch_item(throw_action.item)


class Monkeys:
    def __init__(self, monkeys, worried):
        self.worried = worried
        self.monkeys = []
        item_id = 1
        for monkey_string in monkeys:
            monkey = Monkey(monkey_string, worried, item_id)
            self.monkeys.append(monkey)
            item_id = monkey.last_item_id
        for monkey in self.monkeys:
            monkey.update_target_monkeys(self.monkeys)

    def run_round(self):
        for monkey in self.monkeys:
            monkey.throw_items()

    def print_monkey_items(self):
        for monkey in self.monkeys:
            print(monkey)

    def print_monkey_inspections(self):
        for monkey in self.monkeys:
            print(f'Monkey {monkey.id} inspected items {monkey.items_inspected} times.')

    def print_monkey_business(self):
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
    def __init__(self, worry_level, really_worried, item_id):
        self.id = item_id
        self.really_worried = really_worried
        self.worry_level = worry_level
        self.very_worried_level = worry_level
        self.cent_worry_level = worry_level // worried_factor
        self.dec_worry_level = int(worry_level - (self.cent_worry_level * worried_factor))

    def __str__(self):
        return f'[id:{self.id}: ({self.worry_level}, {self.very_worried_level})], '

    def inspect_item(self, operation, value, monkey_id):
        self.inspect_item_very_worried(operation, value, monkey_id) if self.really_worried else self.inspect_item_not_worried(operation, value)
        self.worry_level //= 3
        test_very_worried_level = self.very_worried_level // 3
        if self.id == 7:
            if self.really_worried:
                return
            else:
                return

    def inspect_item_not_worried(self, operation, value):
        value = self.worry_level if value == default_old_value else value
        match operation:
            case OperationType.Add:
                self.worry_level += value
            case OperationType.Multiply:
                self.worry_level *= value

    def inspect_item_very_worried(self, operation, value, monkey_id):
        cent = (self.cent_worry_level if value == default_old_value else value) // worried_factor
        dec = (self.dec_worry_level if value == default_old_value else value) - (cent * worried_factor)
        match operation:
            case OperationType.Add:
                self.cent_worry_level += cent
                self.dec_worry_level += dec
            case OperationType.Multiply:
                self.cent_worry_level *= cent
                self.dec_worry_level *= dec
        if self.dec_worry_level >= worried_factor:
            cent = self.dec_worry_level // worried_factor
            self.cent_worry_level += cent
            self.dec_worry_level -= cent * worried_factor
        self.very_worried_level = (self.cent_worry_level * worried_factor) + self.dec_worry_level

    def is_worried(self, test):
        return self.is_really_worried(test) if self.really_worried else self.is_slightly_worried(test)

    def is_slightly_worried(self, test):
        return self.worry_level % test == 0

    def is_really_worried(self, test):
        cent_mod = test // worried_factor
        dec_mod = test - cent_mod
        cent = 0 if cent_mod == 0 else int(self.cent_worry_level % cent_mod)
        dec = 0 if dec_mod == 0 else int(self.dec_worry_level % dec_mod) if self.dec_worry_level != 0 else -1
        return cent + dec == 0


class OperationType(Enum):
    Multiply = '*'
    Add = '+'


class ThrowAction:
    def __init__(self, item, monkey):
        self.item = item
        self.monkey = monkey
