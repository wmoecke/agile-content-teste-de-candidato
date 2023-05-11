using CandidateTesting.WernerMoecke.Converter.Interfaces;
using log4net;
using log4net.Config;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace CandidateTesting.WernerMoecke.Converter
{
    /// <summary>
    /// Class responsible of performing all log conversion-related tasks.
    /// </summary>
    public class Converter : IConverter
    {
        private const char _pipe = '|';
        private const char _blank = ' ';
        private const string _configFile = "log4net.config";
        private const string _inputFile = "input.txt";

        public Converter()
        {
        }

        /// <summary>
        /// Main task to perform conversion of a given file to the new format.<br>
        /// Writes output to both the console and the specified log file.
        /// </summary>
        public void Convert(string sourceUri, string outputPath)
        {
            // Configure log4net Logger
            ConfigureLogger(outputPath);

            // Get log4net Logger instance
            ILog logger = LogManager.GetLogger(nameof(logger));

            Console.WriteLine($"Getting source from:\n\"{sourceUri}\"\n");

            try
            {
                // Convert the file from given URI
                var fileContent = ConvertFile(new Uri(sourceUri));
                string[] output = fileContent.Substring(0, fileContent.Length - 1).Split(_pipe);

                // Write out to console/log file
                foreach (string line in output)
                {
                    logger.Warn(line); // Write output to console
                    logger.Info(line); // Write output to log file
                }

                Console.WriteLine($"\nLog file saved to:\n\"{Path.GetFullPath(outputPath)}\".");
            }
            catch (Exception ex)
            {

                logger.Error($"ERROR: {ex.Message}");
            }
            finally
            {
                // Cleanup
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Get the file from the supplied URI and convert it to the new format.
        /// </summary>
        /// <param name="fileUri">URI to file</param>
        /// <returns>The file contents converted to the new format</returns>
        public virtual string ConvertFile(Uri fileUri)
        {
            var fileContent = string.Empty;
            var inputPath = Path.GetFullPath($"./{_inputFile}");

            // Download the file
            // Note: There's a reason why I'm using WebClient() instead of HttpClient()
            using WebClient client = new WebClient();
            client.DownloadFile(fileUri, _inputFile);

            // Perform line-by-line conversion
            using StreamReader reader = new StreamReader(inputPath, Encoding.UTF8);
            while (!reader.EndOfStream)
            {
                fileContent += ConvertLine(reader.ReadLine());
            }
            
            // Cleanup and finish
            reader.Dispose();
            File.Delete(inputPath);

            return fileContent;
        }

        /// <summary>
        /// Takes a line read from a given file and parses its contents.
        /// </summary>
        /// <param name="line">The line to parse</param>
        /// <returns>The line contents converted to the new format</returns>
        public virtual string ConvertLine(string line)
        {
            // Break down the line
            string[] splitLine = line.Split(_pipe);

            // Distribute line fields into the new format
            var log = new Log
            {
                HttpMethod = splitLine[3].Split(_blank)[0].Substring(1),
                StatusCode = splitLine[1].ToString(),
                UriPath = splitLine[3].Split(_blank)[1],
                TimeTaken = Math.Round(decimal.Parse(splitLine[4], CultureInfo.InvariantCulture)).ToString(),
                ResponseSize = splitLine[0].ToString(),
                CacheStatus = splitLine[2]
            };

            return
                $"{log.Provider} " +
                $"{log.HttpMethod} " +
                $"{log.StatusCode} " +
                $"{log.UriPath} " +
                $"{log.TimeTaken} " +
                $"{log.ResponseSize} " +
                $"{log.CacheStatus}{_pipe}";
        }

        /// <summary>
        /// Performs tasks required to configure a log4net instance.
        /// </summary>
        private void ConfigureLogger(string outputPath)
        {
            GlobalContext.Properties["OutputPath"] = outputPath;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var configurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configFile);
            XmlConfigurator.Configure(logRepository, new FileInfo(configurationPath));
        }
    }
}
