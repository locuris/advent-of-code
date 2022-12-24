from enum import Enum

root = 'root'
me = 'humn'
part_ii = False

class Operation(Enum):
    Add = '+'
    Minus = '-'
    Multiply = '*'
    Divide = '/'
    NoOp = '@'


def reset_monkeys(monkeys):
    for monkey in monkeys.keys():
        monkeys[monkey].reset_monkey()


class Monkey:
    def __init__(self, line: str):
        line = ''.join(line.split())
        line_comps = line.split(':')
        self.name = line_comps[0]
        self.operation = Operation.NoOp
        self.is_me = self.name == me
        self.is_root = self.name == root
        self.operation_string = line_comps[1]

        for op in [op for op in Operation]:
            if op is not Operation.NoOp and op.value in self.operation_string:
                self.operation = Operation(op)
                break
        self.value_monkey = self.operation == Operation.NoOp
        self.left_op = ''
        self.right_op = ''
        self.value = ''
        if self.value_monkey:
            self.value = self.operation_string
        else:
            op_cmps = self.operation_string.split(self.operation.value)
            self.left_op = op_cmps[0]
            self.right_op = op_cmps[1]
        self.op_string = '' if not self.value_monkey else self.value if not self.is_me else 'x'
        self.me_monkey: Monkey | None = None

    def __str__(self):
        return f'{self.name}: {self.value} {self.left_op} {self.operation.value} {self.right_op}'

    def perform_operation(self, left_monkey, right_monkey):
        self.op_string = f'({left_monkey.op_string}) {self.operation.value} ({right_monkey.op_string})'
        if left_monkey.is_me or right_monkey.is_me:
            global my_operation
            my_operation.operation = f'{left_monkey.op_string} {self.operation.value} {eval(right_monkey.op_string)}'
        if self.is_root:
            if part_ii:
                operation = left_monkey.op_string.replace('x', self.me_monkey.value)
                left_op = int(float(eval(operation)))
                right_op = int(float(right_monkey.value))
                while not left_op == right_op:
                    value = int(self.me_monkey.value)
                    value += 1
                    print(f'new me value {value} | {left_op} == {right_op} | {my_operation.operation}')
                    operation = left_monkey.op_string.replace('x', str(value))
                    left_op = int(float(eval(operation)))
                    self.me_monkey.value = str(value)
                return True
            self.left_op = left_monkey.op_string
            self.right_op = right_monkey.op_string

        self.value = str(eval(left_monkey.value + ' ' + self.operation.value + ' ' + right_monkey.value))
        self.value_monkey = True

    def reset_monkey(self):
        if self.is_me:
            self.value = str(int(self.value) + 1)
        elif not self.operation == Operation.NoOp:
            op_cmps = self.operation_string.split(self.operation.value)
            self.left_op = op_cmps[0]
            self.right_op = op_cmps[1]
            self.value_monkey = False


class MyOperation:
    def __init__(self, operation=''):
        self.operation = operation


my_operation = MyOperation()
