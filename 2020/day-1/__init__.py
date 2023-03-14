#imported utility function that reads the test input from a file
from common.data import as_lines

#declared target number
TARGET_NUMBER = 2020


def part_i():
    #get lines from text file (test.txt)
    lines = as_lines(True)
    #converts strings to integers
    lines = [int(l) for l in lines]
    first_answer: int
    second_answer: int
    #start first loop through the different numbers
    for first_line in lines:
        #start second (nested) loop through the different numbers
        for second_line in lines:
            #checks if two of the numbers added together equals target number
            if first_line + second_line == TARGET_NUMBER:
                # determine and print answer
                first_answer = first_line
                second_answer = second_line
                print(f'Numbers found\n{first_answer} {second_answer}\n Answer is {first_answer * second_answer}')
                return


#PROGRAM ENTRY POINT
if __name__ == '__main__':
    part_i()
