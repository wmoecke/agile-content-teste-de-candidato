using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.WernerMoecke.Convert
{
	public class Log
	{
		public string Provider { get => "\"MINHA CDN\""; }
		public string HttpMethod { get; set; }
		public string StatusCode { get; set; }
		public string UriPath { get; set; }
		public string TimeTaken { get; set; }
		public string ResponseSize { get; set; }
		public string CacheStatus { get; set; }
        
        public static async Task<string> ConvertFile(Uri fileUri)
        {
            string _fileContent = string.Empty;

            using (HttpClient _client = new HttpClient())
            using (Stream _fs = await _client.GetStreamAsync(fileUri))
            using (StreamReader _reader = new StreamReader(_fs, Encoding.UTF8))
            {
                while (!_reader.EndOfStream)
                {
                    _fileContent += ConvertLine(_reader.ReadLine());
				}
            }
            
            return _fileContent;
        }

        public static string ConvertLine(string line)
		{
            string[] _splitLine = line.Split('|');

            Log _log = new Log
            {
                HttpMethod = _splitLine[3].Split(' ')[0].Substring(1),
                StatusCode = _splitLine[1].ToString(),
                UriPath = _splitLine[3].Split(' ')[1],
                TimeTaken = Math.Round(decimal.Parse(_splitLine[4], CultureInfo.InvariantCulture)).ToString(),
                ResponseSize = _splitLine[0].ToString(),
                CacheStatus = _splitLine[2]
			};

            return 
                $"{_log.Provider} " +
                $"{_log.HttpMethod} " +
                $"{_log.StatusCode} " +
                $"{_log.UriPath} " +
                $"{_log.TimeTaken} " +
                $"{_log.ResponseSize} " +
                $"{_log.CacheStatus}|";
		}
	}
}
