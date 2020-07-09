using System.Collections.Generic;
using System.Linq;
using Fhir.Anonymizer.Core.Processors;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;

namespace Fhir.Anonymizer.Core.Models
{
    public class ProcessResult
    {
        public bool IsRedacted
        {
            get
            {
                return ProcessRecords.ContainsKey(AnonymizationOperations.Redact) && ProcessRecords[AnonymizationOperations.Redact].Count != 0;
            }
        }

        public bool IsAbstracted
        {
            get
            {
                return ProcessRecords.ContainsKey(AnonymizationOperations.Abstract) && ProcessRecords[AnonymizationOperations.Abstract].Count != 0;
            }
        }

        public bool IsCryptoHashed
        {
            get
            {
                return ProcessRecords.ContainsKey(AnonymizationOperations.CryptoHash) && ProcessRecords[AnonymizationOperations.CryptoHash].Count != 0;
            }
        }

        public bool IsEncrypted
        {
            get
            {
                return ProcessRecords.ContainsKey(AnonymizationOperations.Encrypt) && ProcessRecords[AnonymizationOperations.Encrypt].Count != 0;
            }
        }

        public bool IsPerturbed 
        { 
            get 
            {
                return ProcessRecords.ContainsKey(AnonymizationOperations.Perturb) && ProcessRecords[AnonymizationOperations.Perturb].Count != 0;
            }
        }

        public bool IsSubstituted
        {
            get
            {
                return ProcessRecords.ContainsKey(AnonymizationOperations.Substitute) && ProcessRecords[AnonymizationOperations.Substitute].Count != 0;
            }
        }
        public Dictionary<string, HashSet<ITypedElement>> ProcessRecords { get; } = new Dictionary<string, HashSet<ITypedElement>>();

        public void AddProcessRecord(string operationName, ITypedElement node)
        {
            if (ProcessRecords.ContainsKey(operationName))
            {
                ProcessRecords[operationName].Add(node);
            }
            else
            {
                ProcessRecords[operationName] = new HashSet<ITypedElement>() { node };
            }
        }

        public void Update(ProcessResult result)
        {
            if (result == null)
            {
                return;
            }

            foreach (var pair in result.ProcessRecords)
            {
                
                if (!ProcessRecords.ContainsKey(pair.Key))
                {
                    ProcessRecords[pair.Key] = pair.Value;
                }
                else
                {
                    ProcessRecords[pair.Key].UnionWith(pair.Value);
                }
                if (string.Equals(pair.Key, AnonymizationOperations.Substitute))
                {
                    var tempRecords = new Dictionary<string, HashSet<ITypedElement>>();
                    foreach (var key in ProcessRecords.Keys)
                    {
                        if (!string.Equals(key, AnonymizationOperations.Substitute))
                        {
                            tempRecords[key]=ProcessRecords[key].Except(pair.Value).ToHashSet();
                        }
                        
                    }
                    tempRecords[AnonymizationOperations.Substitute] = ProcessRecords[AnonymizationOperations.Substitute];
                    foreach(var key in tempRecords.Keys)
                    {
                        ProcessRecords[key] = tempRecords[key];
                    }
                }
            }
        }
    }
}
