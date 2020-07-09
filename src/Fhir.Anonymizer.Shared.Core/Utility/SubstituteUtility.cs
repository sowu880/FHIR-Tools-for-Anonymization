using System.Text;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Support.Model;
using Hl7.Fhir.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fhir.Anonymizer.Core.Utility
{
    public class SubstituteUtility
    {
        private static readonly PocoStructureDefinitionSummaryProvider s_provider = new PocoStructureDefinitionSummaryProvider();
        public static void Substitute(ElementNode node, ElementNode targetNode)
        {

            //node.Value = targetNode.Value;
            //var children = node.Children().Cast<ElementNode>().ToList();
            foreach (var child in node.Children().Cast<ElementNode>().ToList())
            {
                node.Remove(child);
            }

            var newChildren =  targetNode.Children().Select(element =>ElementNode.FromElement(element)).ToList();
            foreach (var child in newChildren)
            {
                //targetNode.Remove(child);
                node.Add(s_provider, child);
            }
            node.Value = targetNode.Value;
        }
    }
}
