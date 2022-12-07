for test in [True, False]:
    with open("today.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    lookup = {}
    path = [""]
    lookup.update({"/".join(path): 0})
    for line in data[1:]:
        if line.startswith("$"):
            line = line[2:]
            if line.startswith("cd .."):
                path.pop()
            elif line.startswith("cd "):
                path.append(line[3:].strip())
                lookup.update({"/".join(path): 0})
        elif not line.startswith("dir "):
            sz = int(line.split()[0])
            for i in range(len(path)):
                p = "/".join(path[:i+1])
                lookup[p] += sz

    partone = sum([x for x in lookup.values() if x < 100000])

    target = 30000000 - (70000000 - max(lookup.values()))
    parttwo = min([m for m in lookup.values() if m > target])

    if test:
        assert(partone == 95437)
        assert(parttwo == 24933642)
    else:
        print(f"part one: {partone}")
        print(f"part two: {parttwo}")
