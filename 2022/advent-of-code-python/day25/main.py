from snafu import Snafu
from common.util import get_lines


def part_i():
    lines = get_lines('input, from_main=True)
    snafus = []
    for line in lines:
        snafus.append(Snafu(snafu_value=line))
    final_number = 0
    for snafu in snafus:
        final_number += snafu.decimal_int
    print(f'Sum is: {final_number}')
    answer = Snafu(dec_value=final_number)
    print(f'Snafu is: {answer.snafu.lstrip("0")}')


def test():
    tests = [1747, 906, 198, 11, 201, 31, 1257, 32, 353, 107, 3, 37]
    #tests = [201]
    for t in tests:
        snafu = Snafu(dec_value=t)
        print(f'Snafu {snafu.snafu} | Decimal {snafu.decimal_int}')

