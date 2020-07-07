using System.Text;

namespace Fhir.Anonymizer.Core.Utility
{
    public class SubstituteUtility
    {
        public static string Substitute(string input,char ch)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            var strlen = input.Length;
            string result = new string(ch, strlen);
            return result;
        }
    }
}
