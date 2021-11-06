using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    /// <summary>
    /// This class should contain all the "work" for catering
    /// </summary>
    public class Catering
    {
        // This object a list that holds all events that need to be logged
        EventLogger logger = new EventLogger();

        // This object is used for writing transactions to log
        FileAccess file = new FileAccess();

        /// <summary>
        /// This list displays the current state of all catering items
        /// </summary>
        private List<CateringItem> items = new List<CateringItem>();

        /// <summary>
        /// This list displays the current state of all catering items invoiced
        /// </summary>
        private List<CateringItem> invoice = new List<CateringItem>();

        /// <summary>
        /// // This method is used in file access to add items and create the catering list
        /// </summary>
        /// <param name="newItem"></param>
        public void AddItem(CateringItem newItem)
        {
            items.Add(newItem);
        }

        /// <summary>
        /// Total balance of money in order session, starts at 0
        /// </summary>
        public decimal Balance { get; private set; } = 0;

        /// <summary>
        /// This adds money to the Catering Balance Property
        /// </summary>
        /// <param name="moneyToAdd">The amount you would like to add</param>
        /// <returns></returns>
        public string AddMoney(int moneyToAdd)
        {
            if (moneyToAdd > 0)
            {
                // Verify additions to balance over account maximum cannot be added
                if (Balance + moneyToAdd <= 4200M)
                {
                    Balance += moneyToAdd;

                    // Creates and adds an event to the log list
                    Event logItem = new Event("ADD MONEY:", Convert.ToDecimal(moneyToAdd), this.Balance);
                    logger.AddEvent(logItem);
                    file.LogOutput(logger);

                    return $"You have added {moneyToAdd.ToString("C")} to your Balance.";
                }

                // Displays error instead of adding to balance if exceeding 4200 
                else
                {
                    return "Balance cannot exceed $4200.00";
                }
            }

            // Displays error if the user attempted to add less than a whole dollar into account
            return "Please add at least $1.00 to your account.";
        }

        /// <summary>
        /// Subtracts money from the total balance based on purchase
        /// </summary>
        /// <param name="moneyToRemove"></param>
        /// <returns></returns>
        public void RemoveMoney(decimal moneyToRemove)
        {
            // Only removes from balance if user has enough money to remove
            if (Balance > moneyToRemove)
            {
                Balance -= moneyToRemove;
            }
        }

        /// <summary>
        /// Finds that the code entered is present on the list, if none remain, prints sold out, otherwise allows for use to try and purchase if enough are in stock.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="quantityInput"></param>
        /// <returns></returns>
        public string Purchase(string itemCode, string quantityInput)
        {
            // Set to false by default to run loop
            bool isValid = false;

            while (!isValid)
            {
                try
                {
                    // Converts itemcode input by user to uppercase
                    string itemCodeUpper = itemCode.ToUpper();
                    
                    foreach (CateringItem item in items)
                    {
                        // Variables needed to calculate purchase cost and display
                        int quantityToBuy = int.Parse(quantityInput);
                        int quantityOnHand = item.Quantity;
                        decimal purchasePrice = item.PurchasePrice;
                        decimal totalPurchaseCost = purchasePrice * quantityToBuy;

                        // If user input matches a valid item code do the following
                        if (item.CodeIdentifier == itemCodeUpper)
                        {
                            // If quantity is 0, no purchase is completed and screen displays error to user then returns to menu
                            if (quantityOnHand == 0)
                            {
                                return "Sold Out!";
                            }

                            // If more quantity wanting to be bought than in stock, no purchase is completed and screen displays error to user then returns to menu
                            else if (quantityToBuy > quantityOnHand)
                            {
                                return "Not enough stock!";
                            }

                            // If cost is greater than present balance balance, no purchase is completed and screen displays error to user then returns to menu
                            else if (totalPurchaseCost > Balance)
                            {
                                return "You cannot afford that.";
                            }

                            // Otherwise complete purchase and deduct cost from total
                            else
                            {
                                RemoveMoney(totalPurchaseCost);
                                quantityOnHand -= quantityToBuy;
                                item.Quantity = quantityOnHand;
                                item.InvoiceQuantity = quantityToBuy;
                                invoice.Add(item);

                                // Creates and adds an event to the log list
                                Event logItem = new Event(item, this.Balance);
                                logger.AddEvent(logItem);
                                file.LogOutput(logger);
                                return "Purchase Completed!";
                            }
                        }
                    }

                    // Displays error if not a valid item code
                    return @"Error Code 42: Invalid item code";
                }

                // Displays other errors from invalid inputs
                catch (NullReferenceException)
                {
                    return "Invalid input.";
                }
                catch (FormatException)
                {
                    return "Invalid input.";
                }
                catch (ArgumentNullException)
                {
                    return "Invalid input.";
                }
                catch (IOException error)
                {
                    return "Invalid input." + error.Message;
                }
            }

            // Return empty string to make compiler happy :)
            return "";
        }

        /// <summary>
        /// Calculates change of remaining balance and breaks it up into smallest amount of bills possible.
        /// </summary>
        /// <returns></returns>
        public string ChangeReturned()
        {
            // Creates and adds an event to the log list
            Event logItem = new Event("GIVE CHANGE: ", this.Balance, 0.00M);
            logger.AddEvent(logItem);
            file.LogOutput(logger);
            file.CreateCsv(logger);

            // Setting to 0 by default in case change type does not require output
            int twentiesReturned = 0;
            int tensReturned = 0;
            int fivesReturned = 0;
            int onesReturned = 0;
            int quartersReturned = 0;
            int dimesReturned = 0;
            int nicklesReturned = 0;

            // Calculating the most efficient change to return and subtracting the total from each change-type from Balance before advancing
            if (Balance >= 20)
            {
                twentiesReturned = (int)Balance / 20;
                Balance -= (twentiesReturned * 20);
            }

            if (Balance >= 10)
            {
                tensReturned = (int)Balance / 10;
                Balance -= (tensReturned * 10);
            }

            if (Balance >= 5)
            {
                fivesReturned = (int)Balance / 5;
                Balance -= (fivesReturned * 5);
            }

            if (Balance >= 1)
            {
                onesReturned = (int)Balance / 1;
                Balance -= (onesReturned);
            }

            if (Balance >= 0.25M)
            {
                quartersReturned = (int)(Balance * 100M) / 25;
                Balance -= (quartersReturned * .25M);
            }

            if (Balance >= 0.10M)
            {
                dimesReturned = (int)(Balance * 100M) / 10;
                Balance -= (dimesReturned * .10M);
            }

            if (Balance >= 0.05M)
            {
                nicklesReturned = (int)(Balance * 100) / 5;
                Balance -= (nicklesReturned * .05M);
            }

            // Clears out invoice list so it will be empty when the user returns to main menu (if a new cycle of purchases is to be completed)
            invoice.Clear();

            // Returns change that will be sent to user
            return $"You will recieve {twentiesReturned} twenties, {tensReturned} tens, {fivesReturned} fives, {onesReturned} ones," +
                $" {quartersReturned} quarters, {dimesReturned} dimes, and {nicklesReturned} nickles via mail.";
        }

        /// <summary>
        /// Array created from AllItems list for security
        /// </summary>
        public Catering[] AllItems
        {
            get
            {
                return items.ToArray();
            }
        }

        /// <summary>
        /// Array created from InvoiceItems list for security
        /// </summary>
        public Catering[] InvoiceItems
        {
            get
            {
                return invoice.ToArray();
            }
        }
    }
}
