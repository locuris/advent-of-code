from common.util import get_string_by_empty_line
from day11.monkey_objects import Monkeys

file_name = 'day_11_example.txt'
monkey_string = get_string_by_empty_line(file_name)

def print_results(monkeys, rounds):
    starting_check = 20
    for r in range(rounds + 1):
        print(f'round {r}')
        if r == 1 or (r % starting_check == 0 and r != 0):
            print(f'== After round {r - 1} ==')
            monkeys.print_monkey_inspections()
            if r == 1:
                starting_check = 20
            elif starting_check == 20:
                starting_check = 1000
        monkeys.run_round()
    monkeys.print_monkey_business()


def both_parts():
    part_i_monkeys = Monkeys(monkey_string, False)
    part_ii_monkeys = Monkeys(monkey_string, True)
    part_i_monkeys.run_round()
    part_ii_monkeys.run_round()
    part_i_monkeys.print_monkey_business()
    part_ii_monkeys.print_monkey_business()

def part_i():
    print_results(Monkeys(monkey_string, False), 20)


def part_ii():
    print_results(Monkeys(monkey_string, True), 10000)
