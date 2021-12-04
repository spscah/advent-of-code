#!/usr/bin/env bash

if [ ! -d ./$1/$2 ]; then
	mkdir -p ./$1/$2;
fi;

dotnet new sln -o $1/$2/$3
dotnet new console -o $1/$2/$3/$3.App
dotnet sln $1/$2/$3/$3.sln add $1/$2/$3/$3.App/$3.App.csproj 
dotnet sln $1/$2/$3/$3.sln add AdventOfCode/AdventOfCode.Lib/AdventOfCode.Lib.csproj 
dotnet add $1/$2/$3/$3.App/$3.App.csproj reference AdventOfCode/AdventOfCode.Lib/AdventOfCode.Lib.csproj 
cp AdventOfCode/AdventOfCode.Lib/session-cookie.json $1/$2/$3/$3.App/
cp AdventOfCode/AdventOfCode.Lib/Program.cs $1/$2/$3/$3.App/
touch $1/$2/$3/$3.App/test.txt
code $1/$2/$3/. 
code --add AdventOfCode/AdventOfCode.Lib/.
touch $1/$2/$3/$3.App/test.txt
