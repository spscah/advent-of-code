test = False

filename = "test.txt" if test else "today.txt"

partone = 0
parttwo = 0

def deltas(sequence):
	return [sequence[i+1]-sequence[i] for i in range(len(sequence)-1)]
def diffs(sequence):
	ds = deltas(sequence)
	if len(set(ds)) > 1:
		delta = diffs(ds) 
		sequence.append(sequence[-1] + delta)
		return sequence[-1]
	return sequence[-1] + ds[0]

def ndiffs(sequence):
	ds = deltas(sequence)
	if len(set(ds)) > 1:
		delta = ndiffs(ds) 
		sequence.insert(0,sequence[0] - delta)
		return sequence[0]
	return sequence[0] - ds[0]

with open(filename, "r") as f:
	lines = f.read().split("\n")
	rows = [[int(v) for v in l.split(" ")] for l in lines]

ns = [diffs(r) for r in rows]
print(ns)
partone = sum(ns)

ns = [ndiffs(r) for r in rows]
print(ns)
parttwo = sum(ns)

print(f"{partone = }")
print(f"{parttwo = }")