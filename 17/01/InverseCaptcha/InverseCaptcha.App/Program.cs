// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

string data = File.ReadAllText("data.txt");

Debug.Assert(SumNeighbours("1122") == 3);
Debug.Assert(SumNeighbours("1111") == 4);
Debug.Assert(SumNeighbours("1234") == 0);
Debug.Assert(SumNeighbours("91212129") == 9);

Console.WriteLine($"part one {SumNeighbours(data)}");

Debug.Assert(SumHalfway("1212") == 6);
Debug.Assert(SumHalfway("1221") == 0);
Debug.Assert(SumHalfway("123425") == 4);
Debug.Assert(SumHalfway("123123") == 12);
Debug.Assert(SumHalfway("12131415") == 4);

Console.WriteLine($"part one {SumHalfway(data)}");

Console.WriteLine($"done.{Environment.NewLine}<press any key>");


static int SumNeighbours(string input)
{
    int result = 0;
    for(int i = 0; i < input.Length; i++)
    {
        if (input[i] == input[(i + 1) % input.Length])
            result += input[i] - '0';
    }
    return result;
}

static int SumHalfway(string input)
{
    int result = 0;
    for (int i = 0; i < input.Length/2; i++)
    {
        if (input[i] == input[i + input.Length/2])
            result += input[i] - '0';
    }
    return result*2;
}