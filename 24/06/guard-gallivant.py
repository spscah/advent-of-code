def unpack(fn):
    with open(fn) as f:
        grid = f.readlines()
    return grid
     

    

def locate(grid):
    for y, row in enumerate(grid):
        if "^"  in row:
            return (row.index("^"), y)        

    return None

def partone(grid):
    x,y = locate(grid)
    direction = "up"
    locations = [(x,y,direction)]
    while True:
        location = locations[-1]
        rotation = False 
        if direction == "up":
            if location[1] == 0: 
                return locations
            nextlocation = (location[0], location[1] - 1, direction)
            if grid[nextlocation[1]][nextlocation[0]] == "#":
                direction = "right"
                nextlocation = (location[0], location[1] , direction)
                rotation = True
            
        if direction == "right" and not rotation:
            if location[0] == len(grid[0]) - 1: 
                return locations
            nextlocation = (location[0]+1, location[1], direction)
            if grid[nextlocation[1]][nextlocation[0]] == "#":
                direction = "down"
                nextlocation = (location[0], location[1] , direction)
                rotation = True
            
        if direction == "down" and not rotation:
            if location[1] == len(grid) - 1: 
                return locations
            nextlocation = (location[0], location[1]+1, direction)
            if grid[nextlocation[1]][nextlocation[0]] == "#":
                direction = "left"
                nextlocation = (location[0], location[1] , direction)
                rotation = True
            
        if direction == "left":
            if location[0] == 0: 
                return locations
            nextlocation = (location[0]-1, location[1], direction)
            if grid[nextlocation[1]][nextlocation[0]] == "#":
                direction = "up"
                nextlocation = (location[0], location[1] , direction)
            
        locations.append(nextlocation)

def uniques(locations):
    return len(set([(x,y) for x,y,d in locations]))

def meetscycle(triple, path, grid):
    x,y,d = triple
    if triple in path:
        return True
    while True:
        if d == "up":
            if y == 0:
                return False
            nextlocation = (x,y-1,"up")
        if d == "right":
            if x == len(grid[0]) - 1:
                return False
            nextlocation = (x+1,y,"right")
        if d == "down":
            if y == len(grid) - 1:
                return False
            nextlocation = (x,y+1,"down")
        if d == "left":
            if x == 0:
                return False
            nextlocation = (x-1,y,"left")

        if nextlocation in path:
            return True            
        if grid[nextlocation[1]][nextlocation[0]] == "#":
            return False
        x,y,d = nextlocation

def parttwo(track, grid):
    counter = 0
    path = []
    for i in range(len(track)):
        x,y,d = track[i]
        path.append((x,y,d))
        if d == "up":
            nextlocation = (x+1,y,"right")
        if d == "right":
            nextlocation = (x,y+1,"down")
        if d == "down":
            nextlocation = (x-1,y,"left")
        if d == "left":
            nextlocation = (x,y-1,"up")
        if meetscycle(nextlocation, path, grid):
            counter += 1            
            realnext = track[i+1] if i < len(track) else None
#            print(f"{nextlocation} {realnext}")
    return counter

# 389 is too low 

p1 = unpack('test.txt')
testtrack = partone(p1)
testanswer = uniques(testtrack)
assert(testanswer == 41)

p2 = unpack('today.txt')
track = partone(p2)
answer = uniques(track)
print(answer)

assert(parttwo(testtrack,p1) == 6)
print(parttwo(track,p2))
