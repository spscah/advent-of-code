def output(drawing):
    offset = 0
    while offset < len(drawing):
        print(drawing[offset:offset+40])
        offset += 40

for test in [True, False]:
    with open("today.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    partone = 0
    cycle = 1
    registerX = 1
    target = 20
    drawing = ""

    for line in data:
        offset = 0
        x = 0
        if line.startswith("noop"):
            offset = 1
        elif line.startswith("addx"):
            x = int(line.split(" ")[1])
            offset = 2

        if (cycle+offset) > target:
            partone += target * registerX
            target += 40

        while len(drawing) < (cycle-1+offset):
            drawing += '#' if abs(registerX - len(drawing) % 40) <= 1 else '.'

        cycle += offset
        registerX += x
    if test:
        assert(partone == 13140)
    else:
        print(f"part one: {partone}")
    output(drawing)
