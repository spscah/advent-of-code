
for test in [True, False]:
    with open("data.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    part1 = ""
    part2 = ""

    initial = []
    stacks = []
    instructions = []
    for line in data:
        if '[' in line:
            initial.append(line)
        elif len(line) == 1:
            pass
        elif line[1] == '1':
            stacks = [] * max(map(int, line.split('   ')))
        else:
            instructions.append(line)

    print(len(stacks))

    if test:
        assert(part1 == "CMZ")
        #assert(part2 == 4)
    else:
        print(f"part one: {part1}")
        # print(f"part two: {part2}")
