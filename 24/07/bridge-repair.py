def unpack(fn):
    with open(fn) as f:
        lines = f.readlines()
    data = []
    for line in lines:        
        pair = line.split(':')
        data.append((int(pair[0]), list(map(int, pair[1].strip().split()))))
    return data 

def compare(target, numbers):
    index = 2**(len(numbers)-1) 
    for i in range(index):        
        value = numbers[0]
        for j in range(len(numbers)-1):
            if i & 2**j == 2**j:
                value += numbers[j+1]
            else:
                value *= numbers[j+1]

        if value == target:
            return True
        
    return False    

def compare2(target, numbers):
    index = 3**(len(numbers)-1) 
    for i in range(index):        
        value = numbers[0]
        i3 = to_base_3(i,len(numbers)-1)
        for j in range(len(numbers)-1):
            if i3[j] == '0':
                value += numbers[j+1]
            elif i3[j] == '1':
                value *= numbers[j+1]
            else:
                value = int(str(value) + str(numbers[j+1]))
            if value > target: break

        if value == target:
            return True
        
    return False    

def to_base_3(num,pad):
    def to_digits(x):
        while x > 0:
            x, rem = divmod(x, 3)
            yield rem
    return ''.join('012'[d] for d in to_digits(num))[::-1].zfill(pad)


def partone(data):
    value = 0
    for line in data:
        if compare(line[0], line[1]):
            value += line[0]

    return value 

def parttwo(data):
    value = 0
    for line in data:
        if compare2(line[0], line[1]):
            value += line[0]

    return value 

p1 = unpack('test.txt')
assert(partone(p1) == 3749)

p2 = unpack('today.txt')
print(partone(p2))

assert(parttwo(p1) == 11387)
print(parttwo(p2))
