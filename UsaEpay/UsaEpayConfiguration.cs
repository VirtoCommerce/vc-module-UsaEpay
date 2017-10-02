using System.Collections.Generic;

namespace UsaEpay
{
    public class UsaEpayConfiguration
    {
        public string Mode { get; set; }

        public string SourceKey { get; set; }

        public string Pin { get; set; }

        public string Endpoint { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                { "mode", string.IsNullOrWhiteSpace(Mode) ? "sandbox" : Mode.ToLowerInvariant() },
                { "sourceKey", SourceKey },
                { "pin", Pin },
                { "endpoint", Endpoint }
            };
        }
    }
}