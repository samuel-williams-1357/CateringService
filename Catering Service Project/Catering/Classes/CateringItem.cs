using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// This represents a single catering item in your system
    /// </summary>
    public class CateringItem : Catering
    {
        /// <summary>
        /// Type of Item (Beverage, Entree, Appetizer, Dessert)
        /// </summary>
        public string MenuType { get; set; }

        /// <summary>
        /// Code for identifying specific menu item
        /// </summary>
        public string CodeIdentifier { get; set; }

        /// <summary>
        /// Name of specific item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cost to buy specific item
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Quantity of remaining items
        /// </summary>
        public int Quantity { get; set; } = 25;

        /// <summary>
        /// Quantity of items purchased for invoice
        /// </summary>
        public int InvoiceQuantity { get; set; } = 0;

        /// <summary>
        /// Displays the information relating to the list of items
        /// </summary>
        public string DisplayInfo
        {
            get
            {
                // If item is sold out display sold out with item information
                if (this.Quantity == 0) 
                {
                    return $"  SOLD OUT  | {this.CodeIdentifier} {this.MenuType} {this.Name} {this.PurchasePrice} ";
                }

                // Otherwise display number of items in stock and item information
                else
                {
                    return $" {this.Quantity} IN STOCK | {this.CodeIdentifier} {this.MenuType} {this.Name} {this.PurchasePrice} ";
                }
            }
        }
    }
}
