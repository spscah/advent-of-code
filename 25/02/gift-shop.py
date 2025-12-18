
# def invalid(line):

def halve(n, e2o=False):
    h = (len(n) + (1 if e2o else 0))//2
    if h == 0:
        f = 0
        s = int(n)
    else:
        f = int(n[:h])
        s = int(n[h:])
    return f,s

assert(halve("655") == (6,55))
assert(halve("11") == (1,1))
assert(halve("1188511880") == (11885,11880))

def recognise(line):
    first, second = line.split("-")
    f1,f2 = halve(first)
    even2odd = (len(first)%2 == 0) and (len(second)%2==1)
    l1,l2 = halve(second, even2odd)
    first = int(first)
    second = int(second)
    total = 0 
    for i in range(f1,l1+1):
        candidate = int(str(i)*2)
        if first <= candidate <= second:
            total += candidate 
    
    
    return total

assert(recognise("11-22") == 33)
assert(recognise("99-115") == 99)
assert(recognise("1188511880-1188511890") == 1188511885)
assert(recognise("1698522-1698528") == 0)

def part1(fn):
    total = 0 
    with open(fn, "r") as data:
        for item in data.read().strip().split(','):
            value = recognise(item)
            #print(f"{item} : {value}")
            total += value 
    return total
        
    
assert(part1("test.txt") == 1227775554)
print(f"part 1: {part1('today.txt')}")

def getData(filename):
    lines = []
    with open(filename, "r") as datafile:
        for line in datafile:
            lines.append(line.strip())
    return lines

import re

def part1b(test):
    data = getData("test.txt" if test else "today.txt")    
    ranges = data[0].split(',')
    total = 0
    for pair in ranges:
        a, b = pair.split('-')        
        n = len(b)//2 + len(b)%2
        for candidate in range(int(a), int(b)+1):            
            candidates = []
            dupes = 1
            result = re.search(r'(\d+)\1{'+str(dupes)+'}',str(candidate))
            if result != None and result.group() == str(candidate):                    
                candidates.append(candidate)
                    
                    
            #print (result.group())
            # print(set(candidates))
            total += sum(set(candidates))
    return total

assert(part1b(True) == 1227775554)
print(f"part 1b: {part1b(False)}")

def repeats(x):
    if x < 2:
        return []
    return [i-1 for i in range(2,x+1) if x%i == 0]    

def part2(test):
    data = getData("test.txt" if test else "today.txt")    
    ranges = data[0].split(',')
    total = 0
    for pair in ranges:
        a, b = pair.split('-')        
        for candidate in range(int(a), int(b)+1):            
            candidates = []
            for dupes in repeats(len(str(candidate))):
                result = re.search(r'(\d{'+str(len(str(candidate))//(dupes+1))+'})\1{'+str(dupes)+'}',str(candidate))
                if result != None and result.group() == str(candidate):                    
                    candidates.append(candidate)                    
            total += sum(set(candidates))
    return total

assert(part2(True) == 4174379265)
print(f"part 2: {part2(False)}")
# 28905253283 is too low
# 28915331059 is too low
# 28915664389 ✅
