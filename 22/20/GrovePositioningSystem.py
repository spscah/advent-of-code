from collections import deque


def mix(deck, original):
    for pair in original:
        old = deck.index(pair)  # find the old position
        deck.remove(pair)  # remove the old position
        deck.rotate(-pair[1])  # rotate the deck left by the value
        deck.insert(old, pair)  # insert the new position by the offset

    return [x[1] for x in deck]


def grove_coordinates(data):
    rv = 0
    zero = data.index(0)
    for offset in [1, 2, 3]:
        rv += data[(zero + offset * 1000) % len(data)]
    return rv


for test in [True, False]:
    with open("today.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()

    original = [(position, value)
                for position, value in enumerate([int(x.strip()) for x in data])]

    deck = deque(original)
    data = mix(deck, original)
    partone = grove_coordinates(data)

    original = [(i, 811589153 * x) for (i, x) in original]
    deck = deque(original)

    for _ in range(10):
        data = mix(deck, original)

    parttwo = grove_coordinates(data)

    if test:
        assert(partone == 3)
        assert(parttwo == 1623178306)
    else:
        print(f"part one: {partone}")
        print(f"part two: {parttwo}")
