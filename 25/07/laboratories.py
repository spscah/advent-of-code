
def part2(test):
    grid = []
    with open(f"{'today' if not test else 'test'}.txt", "r") as data:
        for line in data:
            grid.append(line.strip())
            
    counters = [0] * len(grid[0])
    counters[grid[0].find("S")] = 1
    
    for line in grid[1:]:
        oldcounters = counters
        counters = [0] * len(grid[0])
        
        for idx, value in enumerate(line):
            if value == '.':
                counters[idx] += oldcounters[idx]
            if value == '^':
                if idx > 0:
                    counters[idx-1] += oldcounters[idx]
                if idx < (len(line)-1):
                    counters[idx+1] += oldcounters[idx]
        #print(counters)
    return sum(counters)

assert(part2(True) == 40)
print(f"Part 2: {part2(False)}")

             
            
    
            