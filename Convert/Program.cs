using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace CandidateTesting.WernerMoecke.Convert
{
	public static class Program
	{
		private static string _sourceURI = null;
		private static string _outputPath = null;
        private static ILog _logger;

		public static async Task Main(string[] args)
		{
			if (args.Length < 2 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
            {
				GetValuesFromConfigFile();
			} 
            else 
            {
                _sourceURI = args[0];
                _outputPath = args[1];
			}
            
            // Programmatically set the Appender's output path
            GlobalContext.Properties["OutputPath"] = _outputPath;
            // Get a log4net instance for our session
            _logger = LogManager.GetLogger("_logger");
            
            Console.WriteLine($"Getting source from: \"{_sourceURI}\"...");

			try
			{
                string _fileContent = await Log.ConvertFile(new Uri(_sourceURI));
                string[] _output = _fileContent.Substring(0, _fileContent.Length - 1).Split('|');
                foreach (string line in _output)
                {
                    _logger.Warn(line); // Write output to console
                    _logger.Info(line); // Write output to log file
			    }
            
                Console.WriteLine($"Log file saved to:\n\"{Path.GetFullPath(_outputPath)}\".");
			}
			catch (Exception ex)
			{
                _logger.Error($"ERROR: {ex.Message}\n\tDetails: {ex.InnerException}");
			}
            
            Console.Write("Press any key to end.");
            Console.ReadLine();
		}
		
		public static void GetValuesFromConfigFile()
        {  
            _sourceURI = ConfigurationManager.AppSettings["SourceURI"];
            _outputPath = ConfigurationManager.AppSettings["OutputPath"];
        }
	}
}
