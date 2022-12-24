from monkeys import Monkey, root, my_operation, me, reset_monkeys, Operation
from common.util import get_lines

file = 'day_21_alt.txt'


def generate_monkeys() -> {}:
    lines = get_lines(file)

    monkeys = {}

    for line in lines:
        monkey = Monkey(line)
        monkeys[monkey.name] = monkey

    return monkeys


def part_i():
    monkeys = generate_monkeys()

    root_monkey = monkeys[root]
    root_monkey.me_monkey = monkeys[me]

    all_monkeys_calculated = False
    while not all_monkeys_calculated:
        all_monkeys_calculated = True
        for _, (_, monkey) in enumerate(monkeys.items()):
            if monkey.value_monkey:
                continue
            left_monkey = monkeys[monkey.left_op]
            right_monkey = monkeys[monkey.right_op]
            if left_monkey.value_monkey and right_monkey.value_monkey:
                monkey.perform_operation(left_monkey, right_monkey)
                continue
            else:
                all_monkeys_calculated = False

    print(f'my operation {my_operation.operation}')
    print(f'operation {root_monkey.op_string}')
    print(f'answer {root_monkey.value}')
    left_op = root_monkey.left_op.replace('x', root_monkey.me_monkey.value)
    print(f'left side {eval(left_op)}')
    print(f'left side {eval(root_monkey.right_op)}')

    my_index = root_monkey.left_op.index('x') - 1
    left_par_count = root_monkey.left_op[:my_index].count('(')
    right_par_count = root_monkey.left_op[my_index:].count(')')

    operation = Operation.NoOp

    while True:
        my_index -= 1
        left_par_count = root_monkey.left_op[:my_index].count('(')
        right_par_count = root_monkey.left_op[my_index:].count(')')
        if left_par_count == right_par_count:
            if root_monkey.left_op[my_index] in [op.value for op in Operation]:
                break


    #operation = Operation(root_monkey.left_op[my_index])

    left = root_monkey.left_op[:my_index]
    right = root_monkey.left_op[my_index+1:]

    l_par = '('
    r_par = ')'

    print(f'left: ( = {left.count(l_par)} | ) = {left.count(r_par)}')
    print(f'right: ( = {right.count(l_par)} | ) = {right.count(r_par)}')

    # right = root_monkey.left_op[my_index+1:] + ')'
    # print(f'{eval(root_monkey.left_op[:my_index])} {operation.value} {eval(right)}')

def part_ii():
    monkeys = generate_monkeys()

    root_monkey = monkeys[root]
    me_monkey = monkeys[me]
    root_monkey.me_monkey = me_monkey

    all_monkeys_calculated = False
    while not all_monkeys_calculated:
        all_monkeys_calculated = True
        for _, (_, monkey) in enumerate(monkeys.items()):
            if monkey.value_monkey:
                continue
            left_monkey = monkeys[monkey.left_op]
            right_monkey = monkeys[monkey.right_op]
            if left_monkey.value_monkey and right_monkey.value_monkey:
                if monkey.is_root:
                    if monkey.perform_operation(left_monkey, right_monkey):
                        all_monkeys_calculated = True
                        break
                    else:
                        reset_monkeys(monkeys)
                monkey.perform_operation(left_monkey, right_monkey)
                continue
            else:
                all_monkeys_calculated = False

    print(f'my operation {my_operation.operation}')
    print(f'operation {root_monkey.op_string}')
    print(f'answer {me_monkey.value}')
