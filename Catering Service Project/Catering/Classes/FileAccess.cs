using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// This class should contain any and all details of access to files
    /// </summary>
    public class FileAccess
    {
        // All external data files for this application should live in this directory.
        // You will likely need to create this directory and copy / paste any needed files.


        // Main catering file location
        public const string cateringFile = @"C:\Catering\cateringsystem.csv";

        // Special menu location (must comment out main file above and uncomment below line to use)
        //string cateringFile = SpecialMenu();

        // Log file locations
        public const string logFile = @"C:\Catering\Log.txt";
        public const string logFileCsv = @"C:\Catering\Log.csv";

        // Total sales report log locations
        public const string TotalSystemLogFile = @"C:\Catering\TotalSales.csv";
        public const string TotalSystemLogReport = @"C:\Catering\TotalSales.rpt";

        /// <summary>
        /// Special Menu location (NOTE: THIS IS JUST FOR FUN)
        /// </summary>
        /// <returns></returns>
        static public string SpecialMenu()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string mainProjectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.Parent.Parent.FullName;
            mainProjectDirectory = mainProjectDirectory.Replace("\\", "/");
            string cateringFile = $"{mainProjectDirectory}/module-1_Mini-Capstone/CateringMenu.csv";

            return cateringFile;
        }

        /// <summary>
        /// This method takes each item from the csv file, parses them out from |-delimination and sets parameters in Catering Item
        /// </summary>
        /// <param name="catering"></param>
        public void LoadCateringMenu(Catering catering)
        {
            // Try reading desired catering menu from above file path
            try
            {
                using (StreamReader reader = new StreamReader(cateringFile))
                {
                    while (!reader.EndOfStream)
                    {
                        // Calling object to add Catering items pulled from menu to list
                        CateringItem item = new CateringItem();

                        // Reads each line of menu and splits parts of the menu by the |
                        string line = reader.ReadLine();
                        string[] menuSections = line.Split("|");

                        // Pulls item's type then converts item type character to full menu section name, and sets result to MenuType parameter
                        if (menuSections[0] == "B")
                        {
                            item.MenuType = "Beverage";
                        }
                        else if (menuSections[0] == "A")
                        {
                            item.MenuType = "Appetizer";
                        }
                        else if (menuSections[0] == "E")
                        {
                            item.MenuType = "Entree";
                        }
                        else if (menuSections[0] == "D")
                        {
                            item.MenuType = "Dessert";
                        }

                        // Pulls item-code and sets CodeIdentifier parameter
                        item.CodeIdentifier = menuSections[1];

                        // Pulls specific item name and sets Name parameter
                        item.Name = menuSections[2];

                        // Pulls item price and sets PurchasePrice parameter
                        item.PurchasePrice = Convert.ToDecimal(menuSections[3]);

                        // Adds items to the list of all catering items in menu
                        catering.AddItem(item);
                    }
                }
            }

            // Error list if issues with input file
            catch (FileNotFoundException)
            {
                Console.WriteLine("Could not fine the Catering Menu File");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Incorrect file path");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Nothing was passed into the LoadCateringMenu method");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The method LoadCateringMenu only accepts a pipe-deliminated csv file");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message + "¯\\_(ツ)_/¯");
            }
        }

        /// <summary>
        /// Outputs Log file
        /// </summary>
        /// <param name="catering"></param>
        public void LogOutput(EventLogger logger)
        {
            // Outputs each Event to log.txt in Catering folder on C:/
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile))
                {
                    foreach (Event logItem in logger.AllEvents)
                    {
                        if (logItem.IsPurchase)
                        {
                            writer.WriteLine($"{logItem.DisplayPurchase}"); 
                        }
                        else if (!logItem.IsPurchase)
                        {
                            writer.WriteLine($"{logItem.DisplayTransaction}");
                        }
                    }
                }
            }

            // Error list if unable to output logs
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to write to this file");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Nothing was passed into the LogOutput method");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The method LogOutput cannot write to this format");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Incorrect file path");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("Congratulations! The file path was too long. This is a rare achievment");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message + "¯\\_(ツ)_/¯");
            }

        }

        /// <summary>
        /// Creates a csv from all events for use in SQL
        /// </summary>
        /// <param name="logger"></param>
        public void CreateCsv (EventLogger logger)
        {
            using (StreamWriter csvWriter = new StreamWriter(logFileCsv, true))
            {

                foreach (Event logItem in logger.AllEvents)
                {
                    if (logItem.IsPurchase)
                    {
                        csvWriter.WriteLine($"{logItem.CsvFriendlyFormat}");
                    }
                }
            }
        }

        // Below is our attempts at the bonus by not using SQL

        /*public void TotalSystemSalesReport(EventLogger logger)
        {
            bool shouldAppend = true;

            string nameFromInvoice = "";
            int quantityFromInvoice = 0;
            decimal totalFromInvoice = 0M;
            decimal rptTotalSales = 0M;


            // Reading the total system sales report.
            using (StreamReader reader = new StreamReader(logFileCsv))
            {
                // Write to or replace items in the total sys sales rpt and append.
                using (StreamWriter writer = new StreamWriter(TotalSystemLogFile, shouldAppend))
                {
                    while (!reader.EndOfStream)
                    {
                        string rptLine = reader.ReadLine();
                        string[] splitRptLine = rptLine.Split("|"); //Array of strings from report current line.
                                                                    // For each element in the Invoice item array (contains multiple values per element) loop through and pull invidual values to store in a new variable.
                        foreach (Event logItem in logger.AllEvents)
                        {
                            if (logItem.IsPurchase)
                            {

                                nameFromInvoice = logItem.Item;
                                quantityFromInvoice = logItem.Quantity;
                                totalFromInvoice = logItem.ItemCost;
                                // Reading each line of the total system sales report, stored as string.
                                // Find out if item.Name.ToString() from invoice array is contained inside this looped line.
                                if (rptLine.Contains(nameFromInvoice)) // Does the name from the invoice array match?
                                {
                                    // If item name contains a space in the name, must remove b/c otherwise it was split both parts of the name as array items. Ex: Orange Juice => OrangeJuice.


                                    string nameInRpt = splitRptLine[0];
                                    int quantityInRpt = int.Parse(splitRptLine[1]);
                                    decimal totalInRpt = decimal.Parse(splitRptLine[2]);
                                    // For each loop will loop through each invoice item, and add the current element's quantity to the new quantity int from report, ect...
                                    int newQuant = quantityInRpt + quantityFromInvoice;
                                    decimal newTotal = totalFromInvoice + totalInRpt;
                                    rptTotalSales += newTotal;
                                    // Adding new quantity to the report in place of the old.
                                    string entry = rptLine.Replace(quantityInRpt.ToString(), newQuant.ToString());
                                    string entry2 = rptLine.Replace(totalInRpt.ToString(), newTotal.ToString());
                                    writer.WriteLine($"{logItem.Item}|{entry}|{entry2}");
                                }

                                // This is an existing item purchased and exists in the report file, must make this line into an array from the report file.
                                // Parse each item in the array of strings, to int/dec dependent. Store in variable.

                                // If the name does not exist currently in the total report file,
                                else
                                {
                                    // Need to pull DisplayReportInfo from event class to properly display the new line of informatino to add as a new writeLine.
                                    writer.WriteLine(logItem.CsvFriendlyFormat);
                                }
                            }
                        }
                    }
                    // Add total sales at the end.
                    shouldAppend = false;
                    writer.WriteLine();
                    writer.WriteLine("**TOTAL SALES** " + "$" + rptTotalSales);
                }
            }
        }*/
    }
}       /*using (StreamReader reader = new StreamReader(logFileCsv))
            {
                bool shouldAppend = false;
                using (StreamWriter writer = new StreamWriter(TotalSystemLogFile, shouldAppend))
                {
                    while (!reader.EndOfStream)
                    {
                        string logLine = reader.ReadLine();
                        string[] loglineArray = logLine.Split("|");

                        decimal finalTotal = 0;
                        foreach (Event logItem in logger.AllEvents)
                        {
                            if (logItem.Item == null)
                            {
                                
                            }
                            if (loglineArray[0] == logItem.Item)
                            {
                                decimal quantity = Convert.ToDecimal(loglineArray[1]);
                                decimal total = decimal.Parse(loglineArray[2]);
                                quantity += logItem.Quantity;
                                total += logItem.ItemCost;
                                finalTotal += total;
                                writer.WriteLine($"{logItem.Item}|{quantity}|{total}");
                            }
                            
                            else if (loglineArray[0] != logItem.Item)
                            {
                                shouldAppend = true;
                                finalTotal += logItem.RunningTotal;
                                writer.WriteLine($"{logItem.Item}|{logItem.Quantity}|{logItem.ItemCost}");
                            }
                        }
                        writer.WriteLine($"**TOTAL SALES** {finalTotal.ToString("C")}");
                    }
                }
            }*/