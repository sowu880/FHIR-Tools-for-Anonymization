using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fhir.Anonymizer.Core.AnonymizationConfigurations
{
    
    public class AnonymizerNodeProcessSetting
    {
        public string replaceValue { get; set; }
        public string Method { get; set; }
        public AnonymizerNodeProcessSetting(string method)
        {
            Method = method;
        }
    }
}
