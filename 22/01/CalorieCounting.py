for test in [True, False]:
    with open("data.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()        
    m = []
    current = int(data[0]) 
    for line in data[1:]:
        if line.strip():
            current += int(line)
        else:
            m.append(current)
            current = 0
    m.append(current)
    m.sort()
    three = sum(m[-3:])
    if test:
        assert(max(m) == 24000)
        assert(three == 45000)
    else:
        print(f"part one: {max(m)}")
        print(f"part two: {three}")
        