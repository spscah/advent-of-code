def priority(overlap):
    assert(len(overlap) == 1)
    overlap = list(overlap)[0]
    base = (1 - ord('a')) if 'a' <= overlap <= 'z' else (27-ord('A'))

    return ord(overlap) + base


for test in [True, False]:
    with open("data.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    score = 0
    score2 = 0
    current = []
    for rucksack in data:
        if rucksack == "":
            continue
        n = len(rucksack)//2
        one = set(rucksack[:n])
        two = set(rucksack[n:])
        overlap = one.intersection(two)

        assert(len(overlap) == 1)
        overlap = list(overlap)[0]
        base = (1 - ord('a')) if 'a' <= overlap <= 'z' else (27-ord('A'))

        score += priority(overlap)

        current.append(set(rucksack.strip()))
        if len(current) == 3:
            overlap = current[0].intersection(
                current[1]).intersection(current[2])
            assert(len(overlap) == 1)
            score2 += priority(overlap)
            current = []

    if test:
        assert(score == 157)
        assert(score2 == 70)
    else:
        print(f"part one: {score}")
        print(f"part two: {score2}")
