using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Model;
using Hl7.FhirPath;
using Hl7.FhirPath.Expressions;

namespace Fhir.Anonymizer.Core.Extensions
{
    public static class FhirPathElementNodeExtension
    {
        public static HashSet<string> ResourceNames = ModelInfo.SupportedResources.ToHashSet<string>();
        public static HashSet<string> TypeNames = ModelInfo.FhirTypeToCsType.Keys.ToList().Except(ResourceNames).ToHashSet();

        public static IEnumerable<ITypedElement> NavigateAll(this ITypedElement element, string name)
        {
            if (string.Equals(name, "_"))
            {
                return element.DescendantsAndSelf();
            }
            else if (name.EndsWith("_"))
            {
                var fieldName = name.Substring(0, name.Length - 1);
                return element.DescendantsAndSelf()
                    .Where(node => string.Equals(node.InstanceType, fieldName, StringComparison.InvariantCulture) 
                        || string.Equals(node.Name, fieldName, StringComparison.InvariantCulture));
            }

            if (char.IsUpper(name[0]))
            {

                // If we are at a resource, we should match a path that is possibly not rooted in the resource
                // (e.g. doing "name.family" on a Patient is equivalent to "Patient.name.family")   
                // Also we do some poor polymorphism here: Resource.meta.lastUpdated is also allowed.
                var baseClasses = new[] { "Resource", "DomainResource" };
                if (element.InstanceType == name || baseClasses.Contains(name))
                {
                    return new List<ITypedElement>() { element };
                }

                return element.Children().Where(child => string.Equals(child.InstanceType, name));
            }
            else
            {
                if (TypeNames.Contains(name))
                {
                    return element.Children().Where(child => string.Equals(child.InstanceType, name));
                }

                return element.Children(name);
            }
        }

        public static IEnumerable<ITypedElement> NavigateAll(this IEnumerable<ITypedElement> elements, string name)
=> elements.SelectMany(e => e.NavigateAll(name));

        public static IEnumerable<ITypedElement> WildcardSelect(this ITypedElement node, string path)
        {
            var resolvedPath = path.Replace("*.", "_.");
            if (resolvedPath.Last() == '*')
            {
                resolvedPath = $"{resolvedPath.Substring(0, resolvedPath.Length - 1)}_";
            }
            return node.Select(resolvedPath);
        }

        public static void ExtendFhirCompilerSymbols()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Add("builtin.children", (IEnumerable<ITypedElement> f, string a) => f.NavigateAll(a), doNullProp: true);
            symbolTable.AddStandardFP();
            symbolTable.AddExtensionSymbols();
            FhirPathCompiler.SetDefaultSymbolTable(new Lazy<SymbolTable>(() => symbolTable));
        }
    }
}
