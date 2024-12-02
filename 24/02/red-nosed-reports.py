def unpack(fn):
    with open(fn) as f:
        lines = f.readlines()
    data = []
    for line in lines:        
        data.append(list(map(int, line.split())))
    return data 

def is_safe(line):
        direction = line[0]-line[1]
        if(direction == 0): return False
        direction = direction // abs(direction)

        for i in range(len(line)-1):
            if (line[i] - line[i+1]) * direction > 3 or ((line[i] - line[i+1])) * direction < 1:
                return False
        return True

def partone(data):
    value = 0
    for line in data:
        safe = is_safe(line)

        if safe:
            value += 1

    return value 

def parttwo(data):
    value = 0
    for line in data:
        safe = is_safe(line)
        if not safe:
            for i in range(len(line)):
                dupe = line[:]
                dupe.pop(i)
                if is_safe(dupe):
                    safe = True
                    break


        if safe:
            value += 1

    return value 



p1 = unpack('test.txt')
assert(partone(p1) == 2)

p2 = unpack('today.txt')
print(partone(p2))

assert(parttwo(p1) == 4)
print(parttwo(p2))