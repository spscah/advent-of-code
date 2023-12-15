import re
from itertools import chain
from collections import defaultdict


def solve(input_str):

    L = input_str.index("\n") + 1

    symbol = re.compile(r"[^\d.\n]")

    def vicinity(start, end):
        return [
            (i, input_str[i])
            for i in chain(
                range(start - 1 - L, end + 1 - L),
                # Tuple below, not a range!
                (start - 1, end),
                range(start - 1 + L, end + 1 + L),
            )
            if 0 <= i < len(input_str)
        ]

    gear_products = defaultdict(lambda: 1)
    part_1 = 0
    for m in re.finditer(r"\d+", input_str):
        part_number = False
        n = int(m.group())
        for i, c in vicinity(*m.span()):
            if symbol.match(c):
                part_number = True
            if c == "*":
                gear_products[i] *= -n
        part_1 += part_number * n

    return part_1, sum(i for i in gear_products.values() if i > 0)

with open("today.txt", "r") as f:
    print(solve(f.read()))