
class Packet:
    def __init__(self, line):
        self.packet_string = line
        self.packet = eval(line)
        self.values = []
        self.no_values = True
        for p in line:
            if p not in ['[', ']', ' ', ',']:
                self.no_values = False
                self.values.append(p)

        self.length = 0 if not self.no_values else len(line)

    def __str__(self):
        return str(self.packet)


def compare(self, other):
    return -1 if is_in_right_order(self.packet, other.packet) else 1
    # if self.no_values and not other.no_values:
    #     return -1
    # if not self.no_values and other.no_values:
    #     return 1
    # if self.no_values and other.no_values:
    #     return -1 if self.length < other.length else 1
    # self_len = len(self.values)
    # other_len = len(other.values)
    # max_length = max(self_len, other_len)
    # for i in range(max_length):
    #     if i >= self_len:
    #         return -1
    #     if i >= other_len:
    #         return 1
    #     if self.values[i] > other.values[i]:
    #         return 1
    #     if self.values[i] < other.values[i]:
    #         return -1
    # return 0

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
