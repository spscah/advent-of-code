# advent-of-code

From the command line: 

```bash
mkdir yy/dd
dotnet new sln -o yy/dd/TaskName
dotnet new console -o yy/dd/TaskName/TaskName.App
dotnet sln yy/dd/TaskName/TaskName.sln add yy/dd/TaskName/TaskName.App/TaskName.App.csproj 
dotnet sln yy/dd/TaskName/TaskName.sln add AdventOfCode/AdventOfCode.Lib/AdventOfCode.Lib.csproj 
dotnet add yy/dd/TaskName/TaskName.App/TaskName.App.csproj reference AdventOfCode/AdventOfCode.Lib/AdventOfCode.Lib.csproj 
cp AdventOfCode/AdventOfCode.Lib/session-cookie.json yy/dd/TaskName/TaskName.App/
code yy/dd/TaskName/. 
code --add AdventOfCode/AdventOfCode.Lib/.
```

