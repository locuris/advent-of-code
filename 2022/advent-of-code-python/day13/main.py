from common.util import get_string_by_empty_line, get_lines
from day13.packet import Packet, compare, is_in_right_order
from functools import cmp_to_key

file = 'day_13.txt'


def main():
    pairs = get_string_by_empty_line(file, True)

    index = 1
    indices = []
    for pair in pairs:
        p = pair.splitlines()
        left = eval(p[0])
        right = eval(p[1])
        if is_in_right_order(left, right):
            indices.append(index)
        index += 1
    answer = sum(indices)
    print(answer)


def main_ii():
    packets_string = get_lines(file, True, True)
    packets = []
    for p in packets_string:
        if p == '':
            continue
        packets.append(Packet(p))

    first_div = Packet('[[2]]')
    second_div = Packet('[[6]]')
    packets.append(first_div)
    packets.append(second_div)

    sorted_packets = sorted(packets, key=cmp_to_key(compare))

    first_div_i = sorted_packets.index(first_div) + 1
    second_div_i = sorted_packets.index(second_div) + 1

    answer = first_div_i * second_div_i

    print(answer)
