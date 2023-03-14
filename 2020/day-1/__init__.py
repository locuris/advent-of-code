from common.data import as_lines

TARGET_NUMBER = 2020

def part_i():
    lines = as_lines(True)
    lines = [int(l) for l in lines]
    first_answer: int
    second_answer: int
    for first_line in lines:
        for second_line in lines:
            if first_line + second_line == TARGET_NUMBER:
                first_answer = first_line
                second_answer = second_line
                print(f'Numbers found\n{first_answer} {second_answer}\n Answer is {first_answer * second_answer}')
                return



if __name__ == '__main__':
    part_i()
