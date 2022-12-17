from enum import Enum


class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def __eq__(self, other):
        return self.x == other.x and self.y == other.y

    def __str__(self):
        return '(' + str(self.x) + ', ' + str(self.y) + ')'

    def move(self, direction):
        match direction:
            case Direction.Up:
                self.y += 1
            case Direction.Down:
                self.y -= 1
            case Direction.Right:
                self.x += 1
            case Direction.Left:
                self.x -= 1

    def jump(self, movement, grid):
        for i in range(movement.distance):
            self.move_with_grid(movement.direction, grid)

    def move_with_grid(self, direction, grid):
        self.move(direction)
        if grid.end.y < self.y:
            grid.end.y = self.y
        if grid.start.y > self.y:
            grid.start.y = self.y
        if grid.end.x < self.x:
            grid.end.x = self.x
        if grid.start.x > self.x:
            grid.start.x = self.x

    def move_to_adjacent(self, point):
        if point.x > self.x:
            self.x += 1
        elif point.x < self.x:
            self.x -= 1
        if point.y > self.y:
            self.y += 1
        elif point.y < self.y:
            self.y -= 1

    def is_adjacent(self, point) -> bool:
        if self.y == point.y:
            return self.x == point.x - 1 or self.x == point.x + 1
        elif self.x == point.x:
            return self.y == point.y - 1 or self.y == point.y + 1
        return False

    def is_above(self, point) -> bool:
        return self.y == point.y + 1


class Direction(Enum):
    Up = 'U'
    Down = 'D'
    Left = 'L'
    Right = 'R'


class Grid:
    def __init__(self, end, start):
        self.end = end
        self.start = start

    def calculate_size(self, movements):
        point = Point(0, 0)
        for movement in movements:
            point.jump(movement, self)



class Movement:
    def __init__(self, text):
        comps = text.split(' ')
        self.direction = Direction(comps[0])
        self.distance = int(comps[1])


def create_movements(lines):
    movements = []
    for line in lines:
        movements.append(Movement(line))
    return movements
