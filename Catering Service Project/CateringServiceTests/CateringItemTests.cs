using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Capstone.Classes;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class CateringItemTests
    {
        //WIP
        [TestMethod]
        public void CheckingDisplayInfoOutputsCorrectly()
        {
            //Arrange
            CateringItem item = new CateringItem();
            Catering sut = new Catering();
            FileAccess fileAccessTest = new FileAccess();

            //Act
            // Loading menu list so items exist.
            fileAccessTest.LoadCateringMenu(sut);
            // Must add money for attempt to buy
            sut.AddMoney(100);
            // Must purchase all items from inv for 1 product
            sut.Purchase("B1", "25");
            // Attempt to purchase when stock is 0
            sut.Purchase("B1", "1");
            string result = item.DisplayInfo;

            //Assert
            // As a result of the input list changing, cannot specify expectation with this test for varying text docs.
            Assert.Inconclusive();
        }
    }
}
