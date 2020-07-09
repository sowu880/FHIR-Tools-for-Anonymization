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
        public void GivenAGeneratedDatatypeNodeAndValidReplaceValue_WhenSubstitute_SubstituteNodeShouldBeReturned()
        {
            SubstituteProcessor processor = new SubstituteProcessor();
            Address testaddress = new Address() { State="DC"};
            var node = ElementNode.FromElement(testaddress.ToTypedElement());
            var processSetting=new AnonymizerNodeProcessSetting("substitute");
            processSetting.replaceValue= "{\r\n  \"use\": \"home\",\r\n  \"type\": \"both\",\r\n  \"text\": \"room\",\r\n  \"city\": \"Beijing\",\r\n  \"district\": \"Haidian\",\r\n  \"state\": \"Beijing\",\r\n  \"postalCode\": \"100871\",\r\n  \"period\": {\r\n    \"start\": \"1974-12-25\"\r\n  }\r\n}";
            var processResult = processor.Process(node, processSetting);
            Assert.Equal("{\r\n  \"use\": \"home\",\r\n  \"type\": \"both\",\r\n  \"text\": \"room\",\r\n  \"city\": \"Beijing\",\r\n  \"district\": \"Haidian\",\r\n  \"state\": \"Beijing\",\r\n  \"postalCode\": \"100871\",\r\n  \"period\": {\r\n    \"start\": \"1974-12-25\"\r\n  }\r\n}", Standardize(node));
            Assert.True(processResult.IsSubstituted);
        }
        
        [Fact]
        public void GivenAPrimitiveDatatypeNodeAndValidReplaceValue_WhenSubstitute_SubstituteNodeShouldBeReturned()
        {
            SubstituteProcessor processor = new SubstituteProcessor();
            var teststring = new FhirString("test");
            var node = ElementNode.FromElement(teststring.ToTypedElement());
            var processSetting = new AnonymizerNodeProcessSetting("substitute");
            processSetting.replaceValue = "new";
            var processResult = processor.Process(node, processSetting);
            Assert.Equal("new", node.Value);
            Assert.True(processResult.IsSubstituted);
        }

        [Fact]
        public void GivenAnInvalidReplaceValue_WhenSubstitute_ExceptionWillBeThrown()
        {
            SubstituteProcessor processor = new SubstituteProcessor();
            Address testaddress = new Address() { State = "DC" };
            var node = ElementNode.FromElement(testaddress.ToTypedElement());
            var processSetting = new AnonymizerNodeProcessSetting("substitute");
            processSetting.replaceValue = "invalid";
            Assert.Throws<FormatException>(() => processor.Process(node, processSetting));
        }

        private static string Standardize(ElementNode node)
        {

            FhirJsonSerializationSettings serializationSettings = new FhirJsonSerializationSettings
            {
                Pretty = true,  
            };
            return node.ToJson(serializationSettings);
        }
       
        
    }
}
