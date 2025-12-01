import graphlib

def unpack(fn):
    with open(fn) as f:
        lines = f.readlines()
    rules = {}
    sequences = []
    header = True

    for line in lines:        
        if line == '\n':
            header = False
            continue
        if header:
            key, value = list(map(int, line.split('|')))

            if key not in rules:
                rules[key] = []
            rules[key].append(value)            
        else:
            sequences.append(list(map(int, line.split(','))))
    return rules, sequences


def partone(data):
    rules, sequences = data
    value = 0
    for seq in sequences:
        ok = True
        for i in range(len(seq)-1):             
            if seq[i] not in rules or  seq[i+1] not in rules[seq[i]]:
                ok = False
        if ok:
            value += seq[len(seq)//2]
    return value

def parttwo(data):
    rules, sequences = data
    value = 0
    for seq in sequences:
        ok = True
        for i in range(len(seq)-1):             
            if seq[i] not in rules or  seq[i+1] not in rules[seq[i]]:
                ok = False
        if ok: continue
        shifted = True
        while shifted:
            shifted = False
            i = 0 
            while i < len(seq)-1:
                if seq[i] not in rules or  seq[i+1] not in rules[seq[i]]:
                    seq[i], seq[i+1] = seq[i+1], seq[i]
                    shifted = True
                i += 1
        value += seq[len(seq)//2]
    return value


p1 = unpack('test.txt')
assert(partone(p1) == 143)

p2 = unpack('today.txt')
print(partone(p2))

assert(parttwo(p1) == 123)
print(parttwo(p2))

