
def check(left, right): 
    if len(left) == 0:
        return True
    if len(right) == 0:
        return False

    l, *left = left 
    r, *right = right

    if l == r:
        pass
    elif type(l) is int and type(r) is int:
        return l < r

    elif type(l) is list and type(r) is list:        
        return check(l, r)

    elif type(l) is int and type(r) is list:
        if [l] == r:
            return check(left, right)
        return check([l], r)

    elif type(l) is list and type(r) is int:
        if l == [r]:
            return check(left, right)
        return check(l, [r])

    return check(left, right)

for test in [True, False]:
    with open("today.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    partone = 0 
    parttwo = 0
    pair = 0
    signals = []
    while pair * 3 < len(data):
        left = eval(data[3*pair])
        right = eval(data[3*pair+1])
        pair += 1
        result = check(left, right)
        if result: 
            partone += pair
        signals += [left,right]

    two = [[2]]
    six = [[6]]
    
    signals.append(two)
    signals.append(six)
    
    for i in range(len(signals)-1):
        swapped = False
        for j in range(len(signals)-1-i):
            if not check(signals[j], signals[j+1]):
                signals[j], signals[j+1] = signals[j+1], signals[j]
                swapped = True            
        if not swapped:
            break
    
    parttwo = (signals.index(two) + 1) * (signals.index(six) + 1)  
        
    if test:
        assert(partone == 13)
        assert(parttwo == 140)
    else:
        print(f"part one: {partone}") 
        print(f"part two: {parttwo}")    
