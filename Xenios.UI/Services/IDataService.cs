﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.UI.Services
{
    public interface IDataService
    {
        String SourceFile { get; set; }

        List<InsuranceInformation> GetAllInsuranceInformations();

        List<InsuranceInformation> FindInsuranceInformationsByCustomerName(string searchValue);
    }
}
