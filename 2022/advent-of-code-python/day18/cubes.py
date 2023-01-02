

class Cube:
    def __init__(self, input_str: str):
        coordinates = input_str.split(',')
        self.x: int = int(coordinates[0])
        self.y: int = int(coordinates[1])
        self.z: int = int(coordinates[2])
        self.sides_exposed = 6

    def __eq__(self, other):
        return self.x == other.x and self.y == other.y and self.z == other.z

    def __str__(self):
        return f'x: {self.x}, y: {self.y}, z:{self.z}'

    def __is_adjacent(self, cube) -> bool:
        diff = abs(self.x - cube.x)
        diff += abs(self.y - cube.y)
        diff += abs(self.z - cube.z)
        return diff == 1

    def update_sides_exposed(self, cubes: []):
        for cube in cubes:
            if cube == self:
                continue
            if self.__is_adjacent(cube):
                self.sides_exposed -= 1

