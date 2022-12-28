import copy
from common.util import get_lines

key = 811589153
mix_count = 10

lines = get_lines('day_20)

original_list = []

for line in lines:
    original_list.append(int(line) * key)
length = len(original_list)

indices = {}
for i, _ in enumerate(original_list):
    indices[i] = i

mutable_list = copy.copy(original_list)
mixes = 0
for _ in range(mix_count):
    print(f'mix {mixes}', end="\r", flush=True)
    print()
    loops = 0
    for original_idx, number in enumerate(original_list):
        #print(f'loop {loops}', end="\r", flush=True)
        if number == 0:
            continue
        current_index = indices[original_idx]
        loop_reduction = number
        new_index = (current_index + number) % (length - 1)
        if new_index <= 0:
            new_index = (length - 1) + new_index
        if new_index >= length:
            new_index = new_index - length + 1
        temp_list = copy.copy(indices)
        temp_list[original_idx] = new_index
        for _, (k, v) in enumerate(temp_list.items()):
            mutable_list[v] = original_list[k]
        reverse = not new_index > current_index
        shift = 1 if reverse else -1
        for _, (k, v) in enumerate(indices.items()):
            if (current_index < v <= new_index and not reverse) or (new_index <= v < current_index and reverse):
                new_v = v + shift
                if new_v == length:
                    new_v = 0
                elif new_v < 0:
                    new_v = length - 1
                temp_list[k] = new_v
        indices = temp_list
        for _, (k, v) in enumerate(indices.items()):
            mutable_list[v] = original_list[k]
        loops += 1
    mixes += 1

index = original_list.index(0)
index = indices[index]
indices = dict((v, k) for k, v in indices.items())
print()
print(f'length {length} starting_index {index}')
print(', '.join(str(item) for item in mutable_list))
first = indices[(index + 1000) % length]
first = original_list[first]
second = indices[(index + 2000) % length]
second = original_list[second]
third = indices[(index + 3000) % length]
third = original_list[third]

print(f'first {first}, second {second}, third {third}')
answer = first + second + third
print(f'answer {answer}')
