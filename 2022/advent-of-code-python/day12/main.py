from common.util import get_string_by_empty_line


def main():
    pairs = get_string_by_empty_line('day_12.txt')

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
