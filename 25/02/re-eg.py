import re

result = re.search(r'(\d{2})\1{1}','2020' )
if result != None:
    print (result.group())