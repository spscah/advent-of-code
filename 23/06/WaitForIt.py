from math import floor, ceil
for test in [True, False]:

	filename = "test.txt" if test else "today.txt" 

	with open(filename, "r") as f: 
		lines = f.read().split("\n")

	times = [int(x) for x in lines[0].split(" ")[1:] if x != ""]
	distances = [int(x) for x in lines[1].split(" ")[1:] if x != ""]

	assert(len(times) == len(distances))

	def natural(n):
		return n*(n+1)//2
		
	def root(a,b,c):
		r = (b*b - 4 * a * c)**0.5
		offset = 1 if r**2 == int(r)**2 else 0
		return floor((-b + r)/2 * a)-offset, ceil((-b -r)/2 * a)+offset

	partone = 1
	for i in range(len(times)):
		f, t = root(1,-times[i], distances[i])
		partone *= f-t+1
		print(f"{root(1,-times[i], distances[i])}, {partone = }")

	ti = int("".join(lines[0].split(" ")[1:]).replace(" ", ""))
	di = int("".join(lines[1].split(" ")[1:]).replace(" ", ""))

	f, t = root(1,-ti, di)
	parttwo = f-t+1

	if test:
		assert(partone == 288)
		assert(parttwo == 71503)
	else: 
		print(f"{partone = }")
		print(f"{parttwo = }")
