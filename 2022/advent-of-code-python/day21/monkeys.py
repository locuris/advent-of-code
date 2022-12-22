from enum import Enum


class Operation(Enum):
    Add = '+'
    Minus = '-'
    Multiply = '*'
    Divide = '/'
    NoOp = ''


class Monkey:
    def __init__(self, line: str):
        line = ''.join(line.split())
        line_comps = line.split(':')
        self.name = line_comps[0]
        self.operation = Operation.NoOp
        operation = line_comps[1]
        for op in [op for op in Operation]:
            if op is not Operation.NoOp and op.value in operation:
                self.operation = Operation(op)
                break
        self.value_monkey = self.operation == Operation.NoOp
        self.left_op = ''
        self.right_op = ''
        self.value = ''
        if self.value_monkey:
            self.value = operation
        else:
            op_cmps = operation.split(self.operation.value)
            self.left_op = op_cmps[0]
            self.right_op = op_cmps[1]

    def __str__(self):
        return f'{self.name}: {self.value} {self.left_op} {self.operation.value} {self.right_op}'

    def perform_operation(self, left_value, right_value):
        self.value = str(eval(left_value + ' ' + self.operation.value + ' ' + right_value))
        self.value_monkey = True
