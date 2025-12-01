
def unpack(fn):
    with open(fn) as f:
        line = map(int, f.readlines()[0].strip())
    return line

def alternate(items):
    zero = False
    for i in items:
        yield [i, zero]
        zero = not zero

def chop(items, size):
    collection = []
    while len(collection) < size:
        if items[-1][0] == size:
            items[-1][0] -= size
            collection.append
        if items[-1][0] == size:

        
    return collection 
    

def partone(items):
    checksum = 0
    location = 0
    zero = False    
    while items:
        if zero:            
            value = items.pop(0)
            zero = False
        else:
            items.pop(0)
            zero = True
     
