def getData(filename):
    lines = []
    with open(filename, "r") as datafile:
        for line in datafile:
            lines.append(line.strip())
    return lines


def part1(test):
    data = getData("test.txt" if test else "today.txt")
    count = 0 
    for y in range(len(data)):
        for x in range(len(data[y])):            
            if data[y][x] != '@':
                continue
            local = 0
            for dx in [1,0,-1]:
                for dy in [1,0,-1]:
                    if dx+x < 0 or dy+y <0 or dx+x >= len(data[y]) or y+dy>= len(data):
                        continue
                    if dx == 0 and dy == 0:
                        continue
                    if data[y+dy][x+dx] == '@':
                        local += 1
            #print(f"({x},{y}): {local}")
            if local < 4:                
                count += 1
    return count 
                    
                
    
assert(part1(True) == 13)
print(part1(False))

def part2(test):
    data = getData("test.txt" if test else "today.txt")
    total = 0
    removable = None 
    while removable != []:
        removable = []
        for y in range(len(data)):
            for x in range(len(data[y])):            
                if data[y][x] != '@':
                    continue
                local = 0
                for dx in [1,0,-1]:
                    for dy in [1,0,-1]:
                        if dx+x < 0 or dy+y <0 or dx+x >= len(data[y]) or y+dy>= len(data):
                            continue
                        if dx == 0 and dy == 0:
                            continue
                        if data[y+dy][x+dx] == '@':
                            local += 1
                #print(f"({x},{y}): {local}")
                if local < 4:                
                    removable.append((x,y))
        total += len(removable)
        for (x,y) in removable:
            l = list(data[y])
            l[x] = '.'
            data[y] = ''.join(l)
    return total


assert(part2(True) == 43)
print(part2(False))
