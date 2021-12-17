using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PacketDecoder.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 16;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);         
            
            Debug.Assert(ParsePacket(ToBinaryString("38006F45291200")).Item2 == 9);

            Debug.Assert(ParsePacket(ToBinaryString("EE00D40C823060")).Item2 == 14);

            Debug.Assert(ParsePacket(ToBinaryString("C200B40A82")).Item3 == 3);
            Debug.Assert(ParsePacket(ToBinaryString("04005AC33890")).Item3 == 54);
            Debug.Assert(ParsePacket(ToBinaryString("880086C3E88112")).Item3 == 7);
            Debug.Assert(ParsePacket(ToBinaryString("CE00C43D881120")).Item3 == 9);
            Debug.Assert(ParsePacket(ToBinaryString("D8005AC2A8F0")).Item3 == 1);
            Debug.Assert(ParsePacket(ToBinaryString("F600BC2D8F")).Item3 == 0);
            Debug.Assert(ParsePacket(ToBinaryString("9C005AC2F8F0")).Item3 == 0);
            Debug.Assert(ParsePacket(ToBinaryString("9C0141080250320F1802104A08")).Item3 == 1);
            Debug.Assert(ParsePacket(ToBinaryString("D2FE28")).Item3 == 2021);

            var result = ParsePacket(ToBinaryString(data[0])); // 1004600 is too low 

            Console.WriteLine($"{result.Item2}"); 
            Console.WriteLine($"{result.Item3}"); // 233391313 is too low, 124787442897 
        }

        static string ToBinaryString(string hex) {
            StringBuilder sb = new StringBuilder();
            foreach(char digit in hex) {
                string c = Convert.ToString(int.Parse(digit.ToString(), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(4,'0');
                sb.Append(c);
            }
            return sb.ToString();
        }

        static int GetSubPart(string d, int from, int length) {            
            string sub =d.Substring(from, length);
            return Convert.ToInt32(sub,2);     
        }

        static (string, int, long) ParsePacket(string packet) {
            if(packet.All(c => c == '0'))
                return (string.Empty, 0,0);
            int rv = GetSubPart(packet, 0, 3); 
            int tid = GetSubPart(packet, 3, 3);
            if(tid == 4) {
                // loop in block of 5 until a block is less than 16, then return the remainder
                int loc = 1;
                long value = 0;
                int inc;
                do {
                    loc += 5;
                    inc = GetSubPart(packet, loc, 5);
                    value <<= 4;
                    value += inc >= 16 ? inc-16 : inc;
                } while(inc >= 16);
                return (packet.Substring(loc+5), rv, value);
            }
                
            if(GetSubPart(packet,6,1) == 0) {
                // the sub parts are measured in length 
                int length = GetSubPart(packet, 7, 15);
                string sub = packet.Substring(22, length);
                IList<long> results = new List<long>();
                while(sub.Length > 0) {
                    (string, int, long) next = ParsePacket(sub);
                    rv += next.Item2;
                    sub = next.Item1;
                    results.Add(next.Item3);
                    if(sub.All(c => c == '0'))
                        break;
                } 
                return (packet.Substring(22+length), rv, Translate(tid, results));
            } else { 
                // the sub parts are counted 
                int counter = GetSubPart(packet, 7, 11);
                
                string sub = packet.Substring(18);
                IList<long> results = new List<long>();
                for(int i = 0; i < counter; ++i) {
                    (string, int, long) res = ParsePacket(sub);
                    rv += res.Item2;
                    sub = res.Item1;
                    results.Add(res.Item3);
                }
                return (sub, rv, Translate(tid, results));
            }
        }

        static long Translate(int tid, IList<long> results) {
            switch(tid) {
                case 0:
                    return results.Sum();                    
                case 1:
                    long value = 1; 
                    foreach(int i in results)
                        value *= i;
                    return value;
                case 2:
                    return results.Min();
                case 3:
                    return results.Max();
                case 5: 
                    return results[0] > results[1] ? 1 : 0;
                case 6: 
                    return results[0] < results[1] ? 1 : 0;
                case 7: 
                    return results[0] == results[1] ? 1 : 0;
                default:
                    return 0;                    
            }
        }
    }
}