from enum import Enum


class CPU:
    def __init__(self, render):
        self.register = 1
        self.cycle = 0
        self.signal_strength = 0
        self.screen = Screen()
        self.render = render
        self.base = 0 if render else 20
        if render:
            self.screen.display_pixel(self.cycle, self.register)

    def update_signal_strength(self):
        if (self.cycle + self.base) % 40 == 0:
            self.signal_strength += self.cycle * self.register

    def increase_cycle(self):
        if not self.render:
            self.cycle += 1
            self.update_signal_strength()
        else:
            self.screen.display_pixel(self.cycle, self.register)
            self.cycle += 1

    def execute(self, command_string):
        command = Command(command_string[:4])
        match command:
            case Command.Noop:
                self.increase_cycle()
            case Command.AddX:
                x = int(command_string[5:])
                for c in range(2):
                    self.increase_cycle()
                self.register += x

    def print_result(self):
        print(self.screen.print_screen() if self.render else self.signal_strength)


class Command(Enum):
    Noop = 'noop'
    AddX = 'addx'


class Row:
    def __init__(self, row_number):
        self.row = '........................................'
        self.row_number = row_number

    def __str__(self):
        return self.row

    def display_pixel(self, cycle):
        position = row_cycle(cycle, self.row_number)
        self.row = self.row[:position] + '#' + self.row[position + 1:]


class Screen:
    def __init__(self):
        self.rows = []

    def add_row(self, row_number):
        self.rows.append(Row(row_number))

    def display_pixel(self, cycle, register):
        row_index = int(cycle / 40)
        if len(self.rows) < row_index + 1:
            self.add_row(row_index)
        if register - 1 <= row_cycle(cycle, row_index) <= register + 1:
            self.rows[row_index].display_pixel(cycle)

    def print_screen(self):
        for row in self.rows:
            print(row)


def row_cycle(cycle, row_number):
    return cycle - (row_number * 40)
