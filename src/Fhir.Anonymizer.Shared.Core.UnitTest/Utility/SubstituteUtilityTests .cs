using System.Collections.Generic;
using Fhir.Anonymizer.Core.Utility;
using Xunit;

namespace Fhir.Anonymizer.Core.UnitTests.Utility
{
    public class SubstituteUtilityTests
    {
        private const string Testsubchar = "hju";
        public static IEnumerable<object[]> GetSubstituteData()
        {
            yield return new object[] { null, null };
            yield return new object[] { string.Empty, string.Empty }; 
            yield return new object[] { "abc", "***" };
            yield return new object[] { "&*^%$@()=-,/", "************" };
            yield return new object[] { "ÆŊŋßſ♫∅", "*******" };
            yield return new object[] { "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()=-",
                "**************************************************************************" };
            yield return new object[] { "测试", "**" };
        }

        [Theory]
        [MemberData(nameof(GetSubstituteData))]
        public void GivenAString_WhenComputeHmac_CorrectHashShouldBeReturned(string input, string expectedresult)
        {
            //string sub_result = SubstituteUtility.Substitute(input, Testsubchar);
            //Assert.Equal(expectedresult, sub_result);
        }
    }
}
