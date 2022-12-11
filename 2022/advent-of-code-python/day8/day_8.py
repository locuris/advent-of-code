def part_1():
    file = open('../data/day_8.txt')
    grid = []
    for line in file.read().splitlines():
        new_row = []
        for c in line:
            new_row.append(int(c))
        grid.append(new_row)

    x_length = len(grid)
    y_length = len(grid[0])
    outside = (x_length * 2) + ((y_length - 2) * 2)
    visible_trees = 0

    for x, row in enumerate(grid):
        for y, tree in enumerate(row):
            if (x == 0 or x == x_length - 1) or (y == 0 or y == y_length - 1):
                visible_trees += 1
                continue
            if max(row[:y]) < tree:
                visible_trees += 1
                continue
            if max(row[y + 1:]) < tree:
                visible_trees += 1
                continue
            visible = False
            for r in grid[:x]:
                if r[y] < tree:
                    visible = True
                else:
                    visible = False
                    break
            if visible:
                visible_trees += 1
                continue
            for r in grid[x + 1:]:
                if r[y] < tree:
                    visible = True
                else:
                    visible = False
                    break
            if visible:
                visible_trees += 1
            # for t in grid[:x]:
            #     if t[y] >= tree:
            #         invisible = True
            # for t in grid[x + 1:]:
            #     if t[y] >= tree:
            #         invisible = True
            # for t in grid[x][:y]:
            #     if t >= tree:
            #         invisible = True
            # for t in grid[x][y + 1:]:
            #     if t >= tree:
            #         invisible = True
            # if invisible:
            #    continue
            # visible_trees += 1

    # visible_trees += outside
    print(visible_trees)


def part_2():
    file = open('../data/day_8.txt')
    grid = []
    for line in file.read().splitlines():
        new_row = []
        for c in line:
            new_row.append(int(c))
        grid.append(new_row)

    x_length = len(grid)
    y_length = len(grid[0])
    outside = (x_length * 2) + ((y_length - 2) * 2)
    visible_trees = 0

    scores = []
    for x, row in enumerate(grid):
        for y, tree in enumerate(row):
            if (x == 0 or x == x_length - 1) or (y == 0 or y == y_length - 1):
                continue
            up = 0
            down = 0
            left = 0
            right = 0
            for t in reversed(row[:y]):
                left += 1
                if t >= tree:
                    break
            for t in row[y + 1:]:
                right += 1
                if t >= tree:
                    break
            for r in reversed(grid[:x]):
                up += 1
                if r[y] >= tree:
                    break
            for r in grid[x + 1:]:
                down += 1
                if r[y] >= tree:
                    break
            score = up * down * left * right
            scores.append(score)

    print(max(scores))
