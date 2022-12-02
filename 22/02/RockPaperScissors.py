for test in [True, False]:
    with open("data.txt" if not test else "test.txt", "r") as f:
        data = f.readlines()
    
    score, score2 = 0, 0
    for line in data:
        items = line.split(" ")
        them, me = items[0], items[1]
        
        them = 1 + ord(them[0]) - ord('A')
        me = 1 + ord(me[0]) - ord('X')
        me2 = me 
        
        if me == them:
            score += 3
        elif me > them%3: 
            score += 6        
        score += me
            
        if me2 == 2:
            score2 += 3 + them
        elif me2 == 1:            
            score2 += [3,1,2][them-1]
        elif me2 == 3:
            score2 += 6+[2,3,1][them-1]        
        
    if test:
        assert(score == 15)
        assert(score2 == 12)
    else:
        print(f"part one: {score}")
        print(f"part two: {score2}")
        