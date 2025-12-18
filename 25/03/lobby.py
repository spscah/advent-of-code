

def getData(filename):
    lines = []
    with open(filename, "r") as datafile:
        for line in datafile:
            lines.append(line.strip())
    return lines


def part1(test):
    data = getData("test.txt" if test else "today.txt")
    joltage = 0 
    for line in data:        
        digits = list(map(int, line))
        first = max(digits[:-1])
        location = digits.index(first)
        second = max(digits[location+1:])
        j = (first*10)+second
        #print(f"{line} : {j}")
        joltage += j
    return joltage 

assert(part1(True) == 357)
print(part1(False))

def part2(test):
    data = getData("test.txt" if test else "today.txt")
    joltage = 0 
    for line in data:
        j = 0
        digits = list(map(int, line))
        location = -1 
        for i in range(1,13):
            if i == 12:
                d = max(digits[location+1:])
            else:
                d = max(digits[location+1:-(12-i)])
            location = digits.index(d, location+1)
            j *= 10
            j += d
        # print(f"{line} : {j}")
        joltage += j
    return joltage 

assert(part2(True) == 3121910778619)
print(part2(False))

