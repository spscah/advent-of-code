import re


test = False

filename = "test.txt" if test else "today.txt"

partone = 0
parttwo = 0

with open(filename, "r") as f:
	lines = f.read().replace("\n\n","\n").split("\n")
	

def convert(patterns, value):
	for pattern in patterns:
		if pattern[1] <= value <= (pattern[1] + pattern[2]):
			return value - pattern[1] + pattern[0]
	return value  

test_patterns = [(50, 98, 2),(52, 50, 48)]

assert(convert(test_patterns, 79) == 81)
assert(convert(test_patterns, 14) == 14)
assert(convert(test_patterns, 55) == 57)
assert(convert(test_patterns, 13) == 13)
assert(convert(test_patterns, 99) == 51)

maps = ["seed-to-soil",
"soil-to-fertilizer",
"fertilizer-to-water",
"water-to-light",
"light-to-temperature",
"temperature-to-humidity",
"humidity-to-location"]

mps = {k:[] for k in maps}

for line in lines:
	if line.startswith("seeds"):
		colon = line.index(":")+1
		seeds = [int(x) for x in line[colon:].strip().split(" ")]
	elif line.endswith(" map:"):
		currentmap = mps[line[:-5]]
	else:
		patterns = [int(p) for p in line.split(" ")]
		assert(len(patterns) == 3)
		a,b,c = patterns 
		currentmap.append((a,b,c))

results = []

def location(value, maps, mps):
	for m in maps:
		value = convert(mps[m], value)
	return value

for seed in seeds:
	results.append(location(seed, maps, mps))

partone = min(results)

results2 = {}

for seed in range(seeds[1]):
	if seed not in results2.keys():
		results2[seed] = location(seeds[0]+seed, maps, mps)

parttwo = min(results2.values())

if test: 
	assert(partone == 35)
	assert(parttwo == 46)
	print("all good")
else:
	print(f"{partone=}")
	print(f"{parttwo=}")