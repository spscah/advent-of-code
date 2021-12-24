using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace ArithmeticLogicUnit.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 0;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IDictionary<char, long> registers = new Dictionary<char,long> {
                    { 'w', 0 }, 
                    { 'x', 0 }, 
                    { 'y', 0 }, 
                    { 'z', 0 }, 
                };

            string input = "11931881141161";
                

            int inputCounter = 0;

            foreach(string d in data) {
                IList<string> operands = d.Split(' ');
                switch(operands[0]) {
                    case "inp":
                        registers[operands[1][0]] = input[inputCounter] - '0';
                        ++inputCounter;
                        break;
                    case "add": 
                        registers[operands[1][0]] += Value(registers, operands[2]);
                        break;
                    case "mul": 
                        registers[operands[1][0]] *= Value(registers, operands[2]);
                        break;
                    case "div": 
                        registers[operands[1][0]] /= Value(registers, operands[2]);
                        break;
                    case "mod": 
                        registers[operands[1][0]] %= Value(registers, operands[2]);
                        break;
                    case "eql":
                        registers[operands[1][0]] = (registers[operands[1][0]] == Value(registers, operands[2]) ? 1 : 0);
                        break;

                }
                Output(registers);
                if(registers['z'] == 0)
                    Console.WriteLine(input);
//                            Console.WriteLine(registers['z']);

            }
        }        

        static void Output(IDictionary<char, long> registers) {
            foreach(char key in registers.Keys) 
                Console.WriteLine($"{key}: {registers[key]}");
            Console.WriteLine("---");
        }


        static long Value(IDictionary<char, long> registers, string operand) {
            if(registers.ContainsKey(operand[0])) {
                return registers[operand[0]];
            }
            return long.Parse(operand);

        }
    }
}