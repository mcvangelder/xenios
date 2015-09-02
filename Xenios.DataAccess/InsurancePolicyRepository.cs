using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;
using Newtonsoft.Json;

namespace Xenios.DataAccess
{
    public class InsurancePolicyRepository
    {
        private string _fileName;
        public String FileName { get { return _fileName; } }

        public InsurancePolicyRepository(string fileName)
        {
            _fileName = fileName;
        }

        public void SaveAll(List<InsurancePolicy> insurancePolicies)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                using (var fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        foreach (var policy in insurancePolicies)
                        {
                            var json = JsonConvert.SerializeObject(policy);
                            streamWriter.WriteLine(json);
                        }
                    }
                }
            });
            try
            {
                t.Wait();
            } catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        public List<InsurancePolicy> GetAll()
        {
            Task<List<InsurancePolicy>> t = Task.Factory.StartNew(() =>
            {
                var allInsurancyPolicies = new List<InsurancePolicy>();

                using (var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        String infoJson = String.Empty;
                        while (!String.IsNullOrEmpty(infoJson = streamReader.ReadLine()))
                        {
                            var infoObject = JsonConvert.DeserializeObject<InsurancePolicy>(infoJson);
                            allInsurancyPolicies.Add(infoObject);
                        }
                    }
                }

                return allInsurancyPolicies;
            });

            return t.Result;
        }
    }
}
