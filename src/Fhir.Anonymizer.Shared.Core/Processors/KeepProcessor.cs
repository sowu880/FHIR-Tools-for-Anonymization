using Fhir.Anonymizer.Core.AnonymizationConfigurations;
using Fhir.Anonymizer.Core.Models;
using Hl7.Fhir.ElementModel;

namespace Fhir.Anonymizer.Core.Processors
{
    public class KeepProcessor: IAnonymizerProcessor
    {
        public ProcessResult Process(ElementNode node, AnonymizerNodeProcessSetting processSetting)
        {
            return new ProcessResult();
        }
    }
}
