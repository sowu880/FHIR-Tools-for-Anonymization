using System.Text;
using Fhir.Anonymizer.Core.Models;
using Fhir.Anonymizer.Core.Utility;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Linq;
using System;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using System.IO;
using Fhir.Anonymizer.Core.AnonymizationConfigurations;
using System.Collections.Generic;

namespace Fhir.Anonymizer.Core.Processors
{
    public class SubstituteProcessor : IAnonymizerProcessor
    {
        //private readonly string substitutetest;
        private readonly ILogger _logger = AnonymizerLogging.CreateLogger<SubstituteProcessor>();
        
        public SubstituteProcessor() { }

        public ProcessResult Process(ElementNode node, AnonymizerNodeProcessSetting processSetting)
        {
            
            var processResult = new ProcessResult();
            var instanceType = node.InstanceType.ToString();
            var assembly = Assembly.GetAssembly(typeof(Patient));
            var parser = new FhirJsonParser();
            ElementNode newnode = null;
            Type objectType = null;
            try
            {
                objectType = assembly.GetTypes().Where(type => type.IsClass && type.Name == instanceType).Single();

            }
            catch (Exception e)
            {
                Console.WriteLine("A primitive type node");
            }
            if (objectType == null)
            {
                newnode = ElementNode.FromElement(ElementNode.ForPrimitive(processSetting.replaceValue));
            }
            else
            {
                newnode = ElementNode.FromElement(parser.Parse(processSetting.replaceValue, objectType).ToTypedElement());
            }
                    
                
            
            
            SubstituteUtility.Substitute(node, newnode);
            _logger.LogDebug($"Fhir value at '{node.Location}' has been substituted.");

            processResult.AddProcessRecord(AnonymizationOperations.Substitute, node);
            return processResult;
        }
    }
}