using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CapstoneTests
{
    [TestClass]
    public class CateringTest
    {
        // Additem method
        [TestMethod]
        public void AddItemsIsAddingToItemsList()
        {
            // Arrange 
            Catering test = new Catering();
            CateringItem testCatItem = new CateringItem();
            FileAccess newfileAccess = new FileAccess();

            // Act
            //Loading the catering menu for items to be output the array.
            newfileAccess.LoadCateringMenu(test);
            //Passing in new Catering item to be added to list.
            test.AddItem(testCatItem); //This will turn into a public array.

            // Assert
            Assert.IsTrue(test.AllItems.Length > 1); 
        }

        // Add money method
        [TestMethod]
        [DataRow(300, "You have added $300.00 to your Balance.")]
        [DataRow(4200, "You have added $4,200.00 to your Balance.")]
        [DataRow(0, "Please add at least $1.00 to your account.")] //Need to fix
        [DataRow(-50, "Please add at least $1.00 to your account.")] //Need to fix
        [DataRow(4201, "Balance cannot exceed $4200.00")]
        public void BalanceAddsMoneyCorrectly(int cashMoneyTest, string expected)
        {
            // Arrange 
            Catering test = new Catering();

            // Act
            //Adding money method which inputs parameters set in datarows
            string result = test.AddMoney(cashMoneyTest);

            // Assert
            Assert.AreEqual(result, expected);
        }

        // Remove money
        [TestMethod]
        public void CheckingMoneyIsRemovedFromBalanceCorrectly()
        {
            // Arrange 
            Catering test = new Catering();
            test.AddMoney(100); //adding balance to have positive balanace to remove from.

            // Act
            // A negative number will never be passed in since the only method that subtracts is purchase method.
            test.RemoveMoney(3.50M);
            // Resulting balance will output to result
            decimal result = test.Balance;

            // Assert
            Assert.IsTrue(result == 96.50M);
            //Assert.AreEqual("", result);
        }

        // Test of purchases result in correct strings
        [TestMethod]
        [DataRow("B2", "10", "You cannot afford that.")]
        public void BalanceCantGoBelow0ByPurchasingItems(string test1, string test2, string expected)
        {
            // Arrange 
            Catering test = new Catering();
            // Must create new file access class in order to call the load menu method.
            FileAccess testItems = new FileAccess();
            // Adding specific csv file info for testing
            testItems.LoadCateringMenu(test);
            // Adding a balance to AddMoney to attempt purchase.
            test.AddMoney(1); // Adding balance to have positive balanace to remove from.

            // Act
            string result = test.Purchase(test1, test2);
            //string result2 = test.RemoveMoney(10);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Test of purchases result in correct strings
        [TestMethod]
        [DataRow("B2", "26", "Not enough stock!")]
        public void PurchaseOfMoreThanItemStockResultsCorrectResponse(string test1, string test2, string expected)
        {
            // Arrange 
            Catering test = new Catering();
            // Must create new file access class in order to call the load menu method.
            FileAccess testItems = new FileAccess();
            // Adding specific csv file info for testing
            testItems.LoadCateringMenu(test);
            // Adding a balance to AddMoney to attempt purchase.
            test.AddMoney(100); //adding balance to have positive balanace to remove from.

            // Act
            string result = test.Purchase(test1, test2);
            //string result2 = test.RemoveMoney(10);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Test of purchases result in correct strings
        [TestMethod]
        [DataRow("B2", "1", "Sold Out!")]
        public void SoldOutItemResultsInSoldOutString(string test1, string test2, string expected)
        {
            // Arrange 
            Catering test = new Catering();
            // Must create new file access class in order to call the load menu method.
            FileAccess testItems = new FileAccess();
            // Adding specific csv file info for testing
            testItems.LoadCateringMenu(test);
            //Setting quantity of item to be 0
            test.AddMoney(150); //adding balance to have positive balanace to remove from.
            test.Purchase("B2", "25"); //Establishes that all items have been purchased.

            // Act
            string result = test.Purchase(test1, test2);
            //string result2 = test.RemoveMoney(10);

            // Assert
            Assert.AreEqual(expected, result);
        }

        //Null or empty string returns request to enter item value (PURCHASE METHOD)
        [TestMethod]
        [DataRow(null, null, "Invalid input.")]
        [DataRow("","", "Invalid input.")]
        public void Null(string test1, string test2, string expected)
        {
            // Arrange 
            Catering test = new Catering();
            // Must create new file access class in order to call the load menu method.
            FileAccess testItems = new FileAccess();
            // Adding specific csv file info for testing
            testItems.LoadCateringMenu(test);
            // Adding a balance to AddMoney to attempt purchase.
            test.AddMoney(100); //adding balance to have positive balanace to remove from.

            // Act
            string result = test.Purchase(test1, test2);
            //string result2 = test.RemoveMoney(10);

            // Assert
            Assert.AreEqual(expected, result);
        }

        //ADDITEM METHOD
        [TestMethod]
        public void CheckingArrayIsRecievingListItems()
        {
            // Arrange 
            Catering test = new Catering();
            CateringItem testCatItem = new CateringItem();
            List<CateringItem> items = new List<CateringItem>();
            

            // Act
            // Checking on new empty list of catering items created, catering item can be added.
            test.AddItem(testCatItem); //Add a new catering item to list

            // Assert
            Assert.IsTrue(test.AllItems.Length == 1);
        }

        [TestMethod]
        public void CheckChangeReturnedDisplaysCorrectAmounts()
        {
            //Arrange
            Catering catering = new Catering();
            FileAccess fileAccess = new FileAccess();
            fileAccess.LoadCateringMenu(catering);
            catering.AddMoney(117);

            //Act
            // There is no way to specify items quantities as purchased b/c they may vary per input file
            string result = catering.ChangeReturned();
            string expected = "You will recieve 5 twenties, 1 tens, 1 fives, 2 ones," +
            $" 0 quarters, 0 dimes, and 0 nickles via mail.";

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
