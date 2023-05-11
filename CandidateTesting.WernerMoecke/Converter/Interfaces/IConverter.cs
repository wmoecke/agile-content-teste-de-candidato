using System;
using System.Threading.Tasks;

namespace CandidateTesting.WernerMoecke.Converter.Interfaces
{
    public interface IConverter
    {
        void Convert(string sourceUri, string outputPath);
        string ConvertFile(Uri fileUri);
        string ConvertLine(string line);
    }
}