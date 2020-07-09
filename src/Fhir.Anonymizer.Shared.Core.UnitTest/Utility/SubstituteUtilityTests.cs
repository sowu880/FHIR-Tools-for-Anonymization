using System;
using System.Collections.Generic;
using Fhir.Anonymizer.Core.Utility;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Fhir.Anonymizer.Core.UnitTests.Utility
{
    public class SubstituteUtilityTests
    {

        public static IEnumerable<object[]> GetSameElementNode()
        {
            yield return new object[] { ElementNode.FromElement(new FhirUri("testuri").ToTypedElement()), ElementNode.FromElement(ElementNode.ForPrimitive("new")) };
            yield return new object[] { ElementNode.FromElement(new FhirString("test").ToTypedElement()), ElementNode.FromElement(ElementNode.ForPrimitive("new"))};
            yield return new object[] { ElementNode.FromElement(new Date("2015-02-07").ToTypedElement()), ElementNode.FromElement(new Date("2015-02-14").ToTypedElement()) };
            yield return new object[] { CreateTestNode(), CreateNewNode() };
            //yield return new object[] { CreateTestResource(), CreateNewResource() };

        }
        public static IEnumerable<object[]> GetDifferentElementNode()
        {
            yield return new object[] { ElementNode.FromElement(new FhirString("test").ToTypedElement()), ElementNode.FromElement(new FhirUri("patient/example").ToTypedElement()) };
            //yield return new object[] { ElementNode.FromElement(new Date("2015-02-07").ToTypedElement()), ElementNode.FromElement(new Date("2015-02-14").ToTypedElement()) };
            //yield return new object[] { CreateTestNode(), CreateNewNode() };
            //yield return new object[] { CreateTestResource(), CreateNewResource() };

        }

        [Theory]
        [MemberData(nameof(GetSameElementNode))]
        public void GivenAnTagetElementNodeInSameType_WhenSubstitute_OriginalNodeWillBeSubstitute(ElementNode node, ElementNode targetnode)
        {
            SubstituteUtility.Substitute(node, targetnode);
            Assert.Equal(Standardize(node), Standardize(targetnode));
        }
        /*
        [Theory]
        [MemberData(nameof(GetDifferentElementNode))]
        public void GivenAnTagetElementNodeInDifferentType_WhenSubstitute_ExceptionWillBeThrown(ElementNode node, ElementNode targetnode)
        {
            
            Assert.Throws<Exception>(() => SubstituteUtility.Substitute(node, targetnode));
            
        }
        */
        private static ElementNode CreateTestNode()
        {
            string content = "{\r\n  \"use\": \"home\",\r\n   \"city\": \"Santa Monica\",\r\n   \"postalCode\": \"39042\",\r\n  }";
            var parser = new FhirJsonParser();
            var node = parser.Parse<Address>(content).ToTypedElement();
            return ElementNode.FromElement(node);
        }

        private static ElementNode CreateNewNode()
        {
            string content = "{\r\n  \"use\": \"home\",\r\n  \"type\": \"both\",\r\n  \"text\": \"\",\r\n  \"city\": \"Beijing\",\r\n  \"district\": \"Haidian\",\r\n  \"state\": \"Beijing\",\r\n  \"postalCode\": \"100871\",\r\n  \"period\": {\r\n    \"start\": \"1974-12-25\"\r\n  }\r\n}";
            var parser = new FhirJsonParser();
            var node = parser.Parse<Address>(content).ToTypedElement();
            return ElementNode.FromElement(node);
        }
        /*
        private static ElementNode CreateTestResource()
        {

        }

        private static ElementNode CreateNewResource()
        {

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
    }
}
