namespace CandidateTesting.WernerMoecke.Converter
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
    }
}
