test = False

filename = "test.txt" if test else "today.txt"

partone = 0
parttwo = 0



def manhattan(a, b, rs, cs, d):
	ab = abs(a[0]-b[0])+abs(a[1]-b[1])
	r = len([i for i in rs if a[0] >= i >= b[0] or a[0] <= i <= b[0]])
	c = len([i for i in cs if a[1] >= i >= b[1] or a[1] <= i <= b[1]])
	return ab + (d-1) * (r + c)

with open(filename, "r") as f:
	lines = f.read().split("\n")

i = 0
rows = []
while i < len(lines):
	if all([ch == '.' for ch in lines[i]]):
		rows.append(i)
	i += 1

c = 0
cols = []
while c < len(lines[0]):
	if all([l[c] == '.' for l in lines]):
		cols.append(c)
	c+=1

galaxies = []
for r in range(len(lines)):
	for c in range(len(lines[0])):
		if lines[r][c] == '#':
			galaxies.append((r,c))

distance = 0

for g1 in galaxies:
	for g2 in galaxies:
		distance += manhattan(g1, g2, rows, cols, 1)

partone = distance // 2 

distance = 0

for g1 in galaxies:
	for g2 in galaxies:
		distance += manhattan(g1, g2, rows, cols, 1000000)

parttwo = distance // 2 

print(f"{partone = }")
print(f"{parttwo = }")