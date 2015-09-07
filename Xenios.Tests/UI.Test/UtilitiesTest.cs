using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.Utilities;
using System.Collections.Generic;
using System.Windows;

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

    }
}
