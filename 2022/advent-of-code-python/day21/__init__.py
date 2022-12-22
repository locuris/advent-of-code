from monkeys import Monkey
from common.util import get_lines

file = 'day_21.txt'
root = 'root'

lines = get_lines(file)

monkeys = {}

for line in lines:
    monkey = Monkey(line)
    monkeys[monkey.name] = monkey

all_monkeys_calculated = False
while not all_monkeys_calculated:
    all_monkeys_calculated = True
    for _, (_, monkey) in enumerate(monkeys.items()):
        if monkey.value_monkey:
            continue
        left_monkey = monkeys[monkey.left_op]
        right_monkey = monkeys[monkey.right_op]
        if left_monkey.value_monkey and right_monkey.value_monkey:
            monkey.perform_operation(left_monkey.value, right_monkey.value)
            continue
        else:
            all_monkeys_calculated = False

print(f'answer {monkeys[root].value}')
