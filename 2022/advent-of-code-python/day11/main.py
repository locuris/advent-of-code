from common.util import get_string_by_empty_line
from day11.monkey_objects import Monkeys


def print_results(monkeys):
    monkeys.print_monkey_items()
    for r in range(20):
        monkeys.run_round()
        monkeys.print_monkey_items()
        print()
    monkeys.print_monkey_buisiness()


def part_i():
    monkey_string = get_string_by_empty_line('day_11_example.txt')
    print_results(Monkeys(monkey_string))
