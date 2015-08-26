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
    public class InsuranceInformationPersistenceService
    {
        private string _fileName;

        public InsuranceInformationPersistenceService(string fileName)
        {
            _fileName = fileName;
        }

        public void Save(InsuranceInformation insuranceInformation)
        {
            using(var fileStream = new FileStream(_fileName, FileMode.Append,FileAccess.Write))
            {
                using(var streamWriter = new StreamWriter(fileStream))
                {
                    var json = JsonConvert.SerializeObject(insuranceInformation);
                    streamWriter.WriteLine(json);
                }
            }
        }

        public List<InsuranceInformation> GetAll()
        {
            var allInsuranceInformations = new List<InsuranceInformation>();

            using (var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    String infoJson = String.Empty;
                    while (!String.IsNullOrEmpty(infoJson = streamReader.ReadLine()))
                    {
                        var infoObject = JsonConvert.DeserializeObject<InsuranceInformation>(infoJson);
                        allInsuranceInformations.Add(infoObject);
                    }
                }
            }

            return allInsuranceInformations;
        }
    }
}
