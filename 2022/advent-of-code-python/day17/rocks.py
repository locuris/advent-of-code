from enum import Enum
from common.grid_objects import Point

debug = False


class Direction(Enum):
    Left = '<'
    Right = '>'


class Rock:
    def __init__(self, points: [Point]):
        self.points: [Point] = points
        self.starting_pos: Point = Point(0, 0)
        self.left_edge: Point = None
        self.bottom_edge: Point = None
        self.width: int = 0
        self.height: int = 0
        self.set_boundaries()

    def __str__(self):
        rock_string = ''
        for h in range(self.height):
            line = ''
            for w in range(self.width):
                if Point(w, h) in self.points:
                    line += '#'
                else:
                    line += '.'
            rock_string += line[::-1] + '\n'
        return rock_string[::-1]

    def __add__(self, other):
        points = self.points.copy()
        other_points = other.points.copy()
        points.extend(other_points)
        return Rock(points)

    def set_boundaries(self):
        left_edge = self.points[0]
        bottom_edge = self.points[0]
        width = 0
        height = 0
        for point in self.points:
            if point.x < left_edge.x:
                left_edge = point
            if point.y < bottom_edge.y:
                bottom_edge = point
            if point.x > width:
                width = point.x
            if point.y > height:
                height = point.y
        self.left_edge: Point = left_edge
        self.bottom_edge: Point = bottom_edge
        self.width: int = (width - left_edge.x) + 1
        self.height: int = (height - bottom_edge.y) + 1

    def print_rock(self):
        print(self)

    def copy(self):
        return Rock(self.points)

    def is_touching(self, rock):
        for other_point in rock.points:
            for point in self.points:
                if point.is_above(other_point):
                    return True
        return False

    def offset(self, width_offset: int, height_offset: int, chamber, with_overlap_check: bool = True):
        new_points = []
        for idx, point in enumerate(self.points):
            new_point = Point(point.x + width_offset, point. y + height_offset)
            if new_point.x >= chamber.width or new_point.x < 0 or (with_overlap_check and new_point in chamber.rock.points):
                return
            new_points.append(new_point)
        self.points = new_points
        self.set_boundaries()

    def move(self, direction: Direction, chamber):
        if (self.left_edge.x == 0 and direction == Direction.Left) or \
                (direction == Direction.Right and self.left_edge.x + self.width > chamber.width + 1):
            return
        self.offset(-1 if direction == Direction.Left else 1, 0, chamber)

    def move_down(self, chamber) -> bool:
        new_points = self.copy()
        new_points.offset(0, -1, chamber, False)
        for chamber_point in chamber.rock.points:
            if chamber_point in new_points.points:
                return False
        self.offset(0, -1, chamber)
        return True


class Chamber:
    def __init__(self, width: int, border: int, gas: str, rocks: [Rock], target_rock_falls: int = 2022):
        self.width: int = width
        self.border: int = border
        self.rocks: [Rock] = rocks
        self.target_rock_falls = target_rock_falls
        self.gas: [Direction] = []
        for flow in gas:
            self.gas.append(Direction(flow))
        floor = []
        for w in range(self.width):
            floor.append(Point(w, 0))
        self.floor = Rock(floor)
        self.rock = self.floor.copy()

    def start_fall(self):
        gas_flow = 0
        rock_type = 0
        for rock_fall in range(self.target_rock_falls):
            print(rock_fall)
            rock = self.rocks[rock_type].copy()
            rock.offset(2, self.rock.height + 3, self)
            self.print_chamber(rock)
            while True:
                gas = self.gas[gas_flow]
                if debug:
                    print(gas)
                rock.move(self.gas[gas_flow], self)
                gas_flow = gas_flow + 1 if gas_flow < len(self.gas) - 1 else 0
                if not rock.move_down(self):
                    break
                else:
                    self.print_chamber(rock)

            self.rock += rock
            self.print_chamber(rock)
            rock_type = rock_type + 1 if rock_type < len(self.rocks) - 1 else 0

        print(self.rock.height - 1)

    def print_chamber(self, new_rock):
        if not debug:
            return
        rock = self.rock.copy()
        if new_rock is not None:
            rock += new_rock
        chamber_str = '+-------+\n'
        for y in range(1, rock.height):
            line_str = '|'
            for x in range(0, 7):
                point = Point(x, y)
                if point in rock.points:
                    line_str += '@' if new_rock is not None and point in new_rock.points else '#'
                else:
                    line_str += '.'
            line_str += '|'
            chamber_str += line_str[::-1] + '\n'
        print(chamber_str[::-1])
