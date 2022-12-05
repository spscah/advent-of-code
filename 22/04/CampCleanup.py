
def parse_range(s):
    numbers = s.split('-')
    return int(numbers[0]), int(numbers[1])


for test in [True, False]:
    with open("data.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    score1 = 0
    score2 = 0

    for line in data:
        pairs = line.split(',')
        assert(len(pairs) == 2)

        first = parse_range(pairs[0])
        second = parse_range(pairs[1])

        if (first[0] <= second[0] and first[1] >= second[1]) or (second[0] <= first[0] and second[1] >= first[1]):
            score1 += 1

        if first[0] <= second[0] <= first[1] or first[0] <= second[1] <= first[1] or second[0] <= first[0] <= second[1] or second[0] <= first[1] <= second[1]:
            score2 += 1

    if test:
        assert(score1 == 2)
        assert(score2 == 4)
    else:
        print(f"part one: {score1}")
        print(f"part two: {score2}")
