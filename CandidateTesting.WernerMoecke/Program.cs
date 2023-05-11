using CandidateTesting.WernerMoecke.Converter.Interfaces;
using System;
using System.Threading.Tasks;

namespace CandidateTesting.WernerMoecke
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Validate proper usage
            if (args.Length < 2 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
            {
                Console.WriteLine("Usage: convert <sourceUrl> <targetPath>");
                return;
            }

            // Perform conversion based on input parameters
            var converter = new Converter.Converter();
            converter.Convert(args[0], args[1]);
        }
    }
}
