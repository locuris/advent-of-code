from common.util import get_string_by_empty_line, get_lines

file = 'day_13_example.txt'


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
    packets = {}
    for packet_string in packets_string:
        packet = eval(packet_string)
        if not packet:
            packets[packet_string] = 0
            continue
        packet_value = 0
        while packet:
            for inner_packet in packet:
                if isinstance(inner_packet, int):
                    packets[packet_string] = inner_packet
                    packet = []
                else:
                    if inner_packet:
                        packet = inner_packet
                        continue
                    else:
                        break
    pass


def is_in_right_order(left, right):
    left = left if isinstance(left, list) else [left]
    right = right if isinstance(right, list) else [right]
    length = max(len(left), len(right))
    for i in range(length):
        if i >= len(left):
            return True
        if i >= len(right):
            return False
        left_item = left[i]
        right_item = right[i]
        if isinstance(left_item, list) or isinstance(right_item, list):
            result = is_in_right_order(left_item, right_item)
            if isinstance(result, bool):
                return result
            else:
                continue
        if left_item < right_item:
            return True
        if left_item > right_item:
            return False
    return 0
