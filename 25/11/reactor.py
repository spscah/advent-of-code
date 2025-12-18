def getData(filename):
    lines = []
    with open(filename, "r") as datafile:
        for line in datafile:
            lines.append(line.strip())
    return lines


def split(line):
    f, ts = line.split(": ")
    return [(f, t) for t in ts.split(" ")]

def fromto(pairs, a, b):
    routes = 0
    q = [a]
    while q != []:
        location = q.pop(0)
        for p in pairs:
            if p[0] != location:
                continue
            if p[1] == b:
                routes += 1
            else:
                q.append(p[1])
        if routes % 1000000 == 0:
            print(".", end = "")
                    
    return routes 


def part1(test):
    data = getData("test.txt" if test else "today.txt")
    pairs = []    
    for line in data:
        pairs += split(line)
    return fromto(pairs, "you", "out")
    
def part2(test):
    data = getData("test2.txt" if test else "today.txt")
    pairs = []    
    for line in data:
        pairs += split(line)

    sf = fromto(pairs, "svr", "fft")
    print(sf)
    fd = fromto(pairs, "fft", "dac")
    print(fd)
    do = fromto(pairs, "dac", "out")
    print(do)
    return sf * fd * do 


def old():
    print(f"pairs: {len(pairs)}")
    sd = 0 #routes(pairs, "svr", "dac", ["fft"])
    print(f"sd {sd}")
    df = 0 # routes(pairs, "dac", "fft", [])
    print(f"df {df}")
    sf = routes(pairs, "svr", "fft", ["dac"])
    print(f"sf {sf}")
    fo = 0 # routes(pairs, "fft", "out", ["dac"])
    print(f"fo {fo}")
    fd = routes(pairs, "fft", "dac", [])
    print(f"fd {fd}")
    do = routes(pairs, "dac", "out", ["fft"])
    print(f"do {do}")    
    
    return sd * df * fo + sf * fd * do 
    

def routes(pairs, a, b, x):
    rv = 0
    q = [[a]]
    while q != []:
        route = q.pop(0)
        for p in [hop for hop in pairs if hop[0] == route[-1]]:
            if p[1] == b:
                rv += 1
            if p[1] not in x and p[1] not in route:
                q.append(route[:] + [p[1]])
    return rv 


assert(part1(True) == 5)
print(part1(False))

assert(part2(True) == 2)
print(part2(False))
