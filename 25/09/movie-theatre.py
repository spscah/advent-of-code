import itertools

def data(test):
    with open(f"{'test' if test else 'today'}.txt","r") as f:
        
        pairs = []
        for line in f.readlines():
            pairs.append(tuple(map(int, line.split(','))))
        return pairs
    
def area(a, b):
    x1,y1 = a
    x2,y2 = b
    return (1+abs(x2-x1))*(1+abs(y2-y1))

def part1(test):
    pairs = data(test)
    #print(pairs)
    result = [area(p[0], p[1]) for p in itertools.combinations(pairs, 2)]
    #print(result)
    return max(result)

assert(part1(True) == 50)
print(part1(False))