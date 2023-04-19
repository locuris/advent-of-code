from common.util import get_string_by_empty_line
from day11.monkey_objects import Monkeys

file_name = 'day_11
monkey_string = get_string_by_empty_line(file_name)


def print_results(rounds):
    monkeys = Monkeys(monkey_string)
    for r in range(rounds):
        print(f'round {r}')
        monkeys.run_round()
    monkeys.print_monkey_business()





def part_ii():
    print_results(Monkeys(monkey_string, True, False), 10000, True)
