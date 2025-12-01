def unpack(fn):
    lines = []
    with open(fn) as f:
        for line in f:
            lines.append(line.strip())
    return lines

def whereis(grid, t):
    for i, row in enumerate(grid):
        if t in row:
            return (i, row.index(t))
    return None



def partone(grid):
    start = whereis(grid, 'S')
    end = whereis(grid, 'E')
    direction = 1
    offsets = [(-1, 0), (0, 1), (1, 0), (0, -1)] 
    steps = [[(start,direction,0)]]
    success = None
    bests = {start:(0, [start])}
    while steps:
        path = steps.pop(0)
        (r,c), d, s = path[-1]
        if success != None and s >= success[0]:
            continue
        sr,sc = (r+offsets[d][0],c+offsets[d][1])
        if grid[sr][sc] == 'E':
            path += [(sr,sc),d,s+1] 
            if success == None or s < success[0]:
                success = s+1, path
            elif s == success[0]:
                success = s, success[1].extend(path)
        elif grid[sr][sc] in '.':
            np = path + [((sr,sc),d,s+1)]
            if (sr,sc) not in bests or s+1 < bests[(sr,sc)][0]:
                steps.append(np)
                bests[(sr,sc)] = (s+1,np)
            elif s+1 == bests[(sr,sc)][0]:
                bests[(sr,sc)][1].extend(np)
                steps.append(np)
        for nd in [(d - 1) % 4, (d + 1) % 4]:
            sr,sc = (r+offsets[nd][0],c+offsets[nd][1])
            np = path + [((sr,sc),nd,s+1001)]
            if grid[sr][sc] == 'E':
                if s == None or s+1001 < success[0]:
                    success = s+1001, np
                elif s+1001 == success[0]:         
                    success = s, success[1].extend(np)   
            elif grid[sr][sc] in '.':
                if (sr,sc) not in bests or s+1001 < bests[(sr,sc)][0]:
                    steps.append(np)
                    bests[(sr,sc)] = (s+1001, np)
                elif s+1001 == bests[(sr,sc)][0]:
                    bests[(sr,sc)][1].extend(np)
                    steps.append(np)

        continue

    return success[0]         



t1 = unpack('test1.txt')
p1a = partone(t1)
assert p1a[0] == 7036

t2 = unpack('test2.txt')
p2a = partone(t2)
assert p2a[0] == 11048
today = unpack('today.txt')
print(partone(today))


