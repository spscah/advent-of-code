def unpack(fn):
    with open(fn) as f:
        grid = [list(map(int, line.strip())) for line in f.readlines()]
    return grid

def neighbours(grid, x, y):
    neighbours = []
    me = grid[y][x]
    for dx, dy in [(1, 0), (-1, 0), (0, 1), (0, -1)]:
        nx, ny = x + dx, y + dy
        if 0 <= nx < len(grid) and 0 <= ny < len(grid[0]) and grid[ny][nx] == me+1:
            neighbours.append((nx, ny))
    return neighbours

def find(grid, digit):
    for y, row in enumerate(grid):
        for x, cell in enumerate(row):
            if cell == digit:
                yield (x, y)

def snake(grid, x, y):
    queue = [(x, y)]
    nines = []
    ninescount = 0
    while queue:
        x, y = queue.pop(0)
        if grid[y][x] == 9:
            nines.append((x, y))
            ninescount += 1
            continue
        for nx, ny in neighbours(grid, x, y):
            queue.append((nx, ny))
    return len(set(nines)), ninescount

def partone(grid):
    nines = 0
    nines2 = 0
    for zero in find(grid, 0):
        local, alllocal = snake(grid, *zero)
        print(f"{zero}: {local}")
        nines += local
        nines2 += alllocal

    return nines, nines2

p1, p1b = partone(unpack('test.txt'))
assert(p1 == 36)
assert(p1b == 81)

p2 = partone(unpack('today.txt'))
print(p2)

