def columns(fn):   
    with open(fn) as f:
        lines = f.readlines()

    left = []
    right = []
    for line in lines:
        pairs = line.split()
        if len(pairs) == 2:
            left.append(int(pairs[0]))
            right.append(int(pairs[1]))
            
    left.sort()
    right.sort()
    return left, right

def partone(left, right):
   
    answer = 0
    for i in range(len(left)):
        answer += abs(left[i] - right[i])

    return answer

def parttwo(left, right):
    answer = 0    
    for i in left:
        answer += i * right.count(i)        
    return answer

l1, r1 = columns('text.txt')
assert(partone(l1,r1) ==11)
assert(parttwo(l1,r1) == 31)

l2, r2 = columns('today.txt')
print(partone(l2,r2))
print(parttwo(l2,r2))


