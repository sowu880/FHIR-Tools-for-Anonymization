using System.Text;
using Fhir.Anonymizer.Core.Models;
using Fhir.Anonymizer.Core.Utility;
using Hl7.Fhir.ElementModel;
using Microsoft.Extensions.Logging;

namespace Fhir.Anonymizer.Core.Processors
{
    public class SubstitueProcessor : IAnonymizerProcessor
    {
        private readonly char _ch;
        private readonly ILogger _logger = AnonymizerLogging.CreateLogger<SubstitueProcessor>();

        public SubstitueProcessor(char sub_char)
        {
            _ch = sub_char;
        }

        public ProcessResult Process(ElementNode node)
        {
            var processResult = new ProcessResult();
            if (string.IsNullOrEmpty(node?.Value?.ToString()))
            {
                return processResult;
            }

            var input = node.Value.ToString();
            // Entire string is masked
            node.Value = SubstituteUtility.Substitute(input, _ch);
            _logger.LogDebug($"Fhir value '{input}' at '{node.Location}' is substituted to '{node.Value}'.");

            processResult.AddProcessRecord(AnonymizationOperations.Substitute, node);
            return processResult;
        }
    }
}