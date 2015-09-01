using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.Utilities;
using System.Collections.Generic;

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



    }
}
