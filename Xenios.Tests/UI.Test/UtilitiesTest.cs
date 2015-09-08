using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.Utilities;
using System.Collections.Generic;
using System.Windows;
using System.Collections;

namespace Xenios.UI.Test
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void Should_create_collection_of_enum()
        {
            var collection = EnumHelper.GetAllAsCollection<List<Domain.Enums.TermUnits>,Domain.Enums.TermUnits>();
            var actualValues = Enum.GetValues(typeof(Domain.Enums.TermUnits));
            var actualCount = actualValues.Length;
            var collectionCount = collection.Count;

            Assert.AreEqual(actualCount,collectionCount);
        }

        [TestMethod]
        public void Should_convert_true_to_Visible_Visibility()
        {
            var visiblity = new BooleanToVisibilityConverter().Convert(true,null,null, null);

            Assert.AreEqual(Visibility.Visible, visiblity);
        }

        [TestMethod]
        public void Should_convert_false_to_Collapsed_Visibility()
        {
            var visibility = new BooleanToVisibilityConverter().Convert(false, null, null, null);

            Assert.AreEqual(Visibility.Collapsed, visibility);
        }

        [TestMethod]
        public void Should_convert_Collapsed_Visibility_back_to_false()
        {
            var boolean = new BooleanToVisibilityConverter().ConvertBack(Visibility.Collapsed, null, null, null);

            Assert.AreEqual(false, boolean);
        }

        [TestMethod]
        public void Should_convert_Visible_Visibility_back_to_true()
        {
            var boolean = new BooleanToVisibilityConverter().ConvertBack(Visibility.Visible, null, null, null);

            Assert.AreEqual(true, boolean);
        }

        [TestMethod]
        public void Should_split_InsuranceTypes_into_list_InsuranceType_enums_via_convert()
        {
            var insuranceType = Domain.Enums.InsuranceTypes.Collision |
                                    Domain.Enums.InsuranceTypes.Comprehensive |
                                    Domain.Enums.InsuranceTypes.Glass |
                                    Domain.Enums.InsuranceTypes.Liability |
                                    Domain.Enums.InsuranceTypes.Umbrella;

            var insuranceTypesList = (List<Domain.Enums.InsuranceTypes>)
                                            new InsuranceTypesListToFlagsConverter()
                                                .ConvertBack(insuranceType, null, null, null);

            AssertAllInsuranceTypesArePresent(insuranceType, insuranceTypesList);
        }

        private static void AssertAllInsuranceTypesArePresent(Domain.Enums.InsuranceTypes expected, 
                ICollection<Domain.Enums.InsuranceTypes> selectedInsuranceTypes)
        {
            foreach (var val in selectedInsuranceTypes)
            {
                Assert.IsTrue((expected & val) != 0);
            }
        }

        [TestMethod]
        public void Should_combine_SelectedInsuranceTypes_into_single_enum_via_convert()
        {
            var insuranceTypesList = new List<Domain.Enums.InsuranceTypes>
            {
                Domain.Enums.InsuranceTypes.Collision, 
                Domain.Enums.InsuranceTypes.Comprehensive,
                Domain.Enums.InsuranceTypes.Glass,
                Domain.Enums.InsuranceTypes.Liability,
                Domain.Enums.InsuranceTypes.Umbrella
            };

            var insuranceTypesFlags = (Domain.Enums.InsuranceTypes)
                                         new InsuranceTypesListToFlagsConverter()
                                             .Convert(insuranceTypesList, null, null, null);

            AssertAllInsuranceTypesArePresent(insuranceTypesFlags, insuranceTypesList);
        }

        [TestMethod]
        public void Should_combine_SelectedInsuranceTypes_into_single_enum_via_combine()
        {
            var insuranceTypesList = new List<Domain.Enums.InsuranceTypes>
            {
                Domain.Enums.InsuranceTypes.Collision, 
                Domain.Enums.InsuranceTypes.Comprehensive,
                Domain.Enums.InsuranceTypes.Glass,
                Domain.Enums.InsuranceTypes.Liability,
                Domain.Enums.InsuranceTypes.Umbrella
            };

            var insuranceTypesFlags = InsuranceTypesListToFlagsConverter.CombineIntoSingle(insuranceTypesList);
                                             

            AssertAllInsuranceTypesArePresent(insuranceTypesFlags, insuranceTypesList);
        }

        [TestMethod]
        public void Should_split_InsuranceTypes_into_list_InsuranceType_enums_via_split()
        {
            var insuranceType = Domain.Enums.InsuranceTypes.Collision |
                                    Domain.Enums.InsuranceTypes.Comprehensive |
                                    Domain.Enums.InsuranceTypes.Glass |
                                    Domain.Enums.InsuranceTypes.Liability |
                                    Domain.Enums.InsuranceTypes.Umbrella;

            var insuranceTypesList = InsuranceTypesListToFlagsConverter.SplitIntoList(insuranceType);

            AssertAllInsuranceTypesArePresent(insuranceType, insuranceTypesList);
        }
    }
}
