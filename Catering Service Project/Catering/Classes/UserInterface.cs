using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// This class provides all user communications, but not much else.
    /// All the "work" of the application should be done elsewhere
    /// </summary>
    public class UserInterface
    {
        // Creating catering object
        private Catering catering = new Catering();

        /// <summary>
        /// Displays to user the main menu options to select from.
        /// </summary>
        public void RunMainMenu()
        {
            // Creates list from csv contents for display
            FileAccess file = new FileAccess();
            file.LoadCateringMenu(catering);

            // Set to false while program needs to be open
            bool done = false;

            while (!done)
            {
                // Displays menu options and allows user to enter which option they would like to choose
                Console.WriteLine("(1) Display Catering Items \n(2) Order \n(3) Quit \n");
                Console.Write("Enter Menu Option: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                // Takes in user input to select from and execute above options
                switch (input)
                {
                    case "1":
                        DisplayCateringItems();
                        break;

                    case "2":
                        DisplayOrderMenu();
                        break;

                    case "3":
                        ExitMessagesFromOurSponsor();
                        // Set done to true to exit program
                        done = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Displays to user the list of of items from csv file for option 1 of initial menu
        /// </summary>
        public void DisplayCateringItems()
        {
            // Loops through the catering items pulled from csv and displays the format set up in the catering item class
            foreach (CateringItem item in this.catering.AllItems)
            {
                Console.WriteLine(item.DisplayInfo);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Displays to user the purchase sub-menu of for option 2 of initial menu
        /// </summary>
        public void DisplayOrderMenu()
        {
            // Set to false while sub-menu needs to be open
            bool done = false;

            while (!done)
            {
                // Displays menu options and allows user to enter which option they would like to choose
                Console.WriteLine($"(1) Add Money \n(2) Select Products \n(3) Complete Transaction \n \nCurrent Account Balance:  { catering.Balance.ToString("C") } \n");
                Console.Write("Enter Menu Option: ");
                string input2 = Console.ReadLine();
                Console.WriteLine();

                // Takes in user input to select from and execute above options
                switch (input2)
                {
                    case "1":
                        AddFunds();
                        break;

                    case "2":
                        PurchaseItems();
                        break;

                    case "3":
                        DisplayInvoice();
                        // Set done to true to exit sub-menu
                        done = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Displays to user the options for adding funds to balance
        /// </summary>
        public void AddFunds()
        {
            // Prompts to add money to account and converts input to int
            Console.Write("How much money would you like to add to your account?: ");
            int moneyToAdd = Convert.ToInt32(Console.ReadLine());

            // Adds money to balance if possible then displays amount added(done in method), afterwards displays total balance to user
            Console.WriteLine(catering.AddMoney(moneyToAdd));
            Console.WriteLine($"You have { catering.Balance.ToString("C") } in your account. \n");
        }

        /// <summary>
        /// Displays to user item purchase screen
        /// </summary>
        public void PurchaseItems()
        {
            // Prompts user to enter item code and item quantity and stores in string variables
            Console.Write("Enter item code: ");
            string whichToBuy = Console.ReadLine();

            Console.Write("How much of this item would you like to purchase? ");
            string quanitityToBuy = Console.ReadLine();
            Console.WriteLine();

            // Uses input strings to identify if itema can be purchased based on stock quantity and balance (done in method).
            Console.WriteLine(catering.Purchase(whichToBuy, quanitityToBuy));
            Console.WriteLine($"You have { catering.Balance.ToString("C") } in your account. \n");
        }

        /// <summary>
        /// Displays to user information regarding purchase of items including totals
        /// </summary>
        public void DisplayInvoice()
        {
            // Starting audit of total spent at 0 for output if no money is spent
            decimal total = 0M;

            // Loops through each COMPLETED purchase that has been added to invoice list in purchase method
            foreach (CateringItem item in this.catering.InvoiceItems)
            {
                // Setting item total to the cost of all units bought and adds itemTotal to audited total
                decimal itemTotal = item.PurchasePrice * item.InvoiceQuantity;
                total += itemTotal;

                // Displays details of each completed purchase to user
                Console.WriteLine("{0, -10} {1, -10} {2, -15} {3, -20} {4, 15:C} {5, 10:C}", "   ", item.InvoiceQuantity, item.MenuType, item.Name, item.PurchasePrice, itemTotal);
            }

            // Displays the total spent via audit and change needing "mailed" to user to complete invoice
            Console.WriteLine($"\nTotal: {total.ToString("C")}\n");
            Console.WriteLine($"{catering.ChangeReturned()} \n");
        }

        /// <summary>
        /// Displays exit messages *Sponsored by The Company*
        /// </summary>
        public void ExitMessagesFromOurSponsor()
        {
            Console.WriteLine("Thank you for shopping with Weyland Yutani Corporation!");
            Console.WriteLine("'Building Better Worlds' since 2099!");
            Console.WriteLine();
            Console.WriteLine("Please come again!");
        }
    }
}
