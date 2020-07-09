using System;
using Fhir.Anonymizer.Core.AnonymizationConfigurations;
using Fhir.Anonymizer.Core.AnonymizerConfigurations;
using Fhir.Anonymizer.Core.Models;
using Fhir.Anonymizer.Core.Processors;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Xunit;

namespace Fhir.Anonymizer.Core.UnitTests.Processors
{
    public class SubstituteProcessorTests
    {
        
        [Fact]
        public void GivenARuleWithValidValue_WhenSubstitute_SubstituteNodeShouldBeReturned()
        {
            SubstituteProcessor processor = new SubstituteProcessor();
            Address testaddress = new Address() { State="DC"};
            var node = ElementNode.FromElement(testaddress.ToTypedElement());
            var processResult = processor.Process(node, CreateTestRule("{\r\n  \"use\": \"home\",\r\n  \"type\": \"both\",\r\n  \"text\": \"\",\r\n  \"city\": \"Beijing\",\r\n  \"district\": \"Haidian\",\r\n  \"state\": \"Beijing\",\r\n  \"postalCode\": \"100871\",\r\n  \"period\": {\r\n    \"start\": \"1974-12-25\"\r\n  }\r\n}"));
            Assert.Equal("{\r\n  \"use\": \"home\",\r\n  \"type\": \"both\",\r\n  \"text\": \"\",\r\n  \"city\": \"Beijing\",\r\n  \"district\": \"Haidian\",\r\n  \"state\": \"Beijing\",\r\n  \"postalCode\": \"100871\",\r\n  \"period\": {\r\n    \"start\": \"1974-12-25\"\r\n  }\r\n}", Standardize(node));
            Assert.True(processResult.IsSubstituted);
        }
        /*
        [Fact]
        public void GivenARuleWithValidValue_WhenSubstitute_SubstituteNodeShouldBeReturned()
        {
            SubstituteProcessor processor = new SubstituteProcessor();
            var teststring = new FhirString("test");
            var node = ElementNode.FromElement(teststring.ToTypedElement());
            var processResult = processor.Process(node, CreateTestRule("new"));
            Assert.Equal("new", Standardize(node));
            Assert.True(processResult.IsSubstituted);
        }
        */
        private static string Standardize(ElementNode node)
        {

            FhirJsonSerializationSettings serializationSettings = new FhirJsonSerializationSettings
            {
                Pretty = true
            };
            return node.ToJson(serializationSettings);
        }
       
        private static AnonymizationFhirPathRule CreateTestRule(string target)
        {
            return new AnonymizationFhirPathRule("Resource", "Resource", "Resource", "substitute", AnonymizerRuleType.FhirPathRule, target);
        }
    }
}
