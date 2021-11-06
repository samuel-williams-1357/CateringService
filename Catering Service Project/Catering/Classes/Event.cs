using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// Class to hold event information
    /// </summary>
    public class Event
    {
        /// <summary>
        /// This constructor creates an event for an item purchase
        /// </summary>
        /// <param name="item">All relevant properties of the item will be stored</param>
        /// <param name="balance">accepts the Balance value in catering</param>
        public Event(CateringItem item, decimal balance)
        {
            this.Item = item.Name;
            this.ItemCode = item.CodeIdentifier;
            this.Quantity = item.InvoiceQuantity;
            this.ItemCost = item.PurchasePrice * item.InvoiceQuantity;
            this.RunningTotal = balance;
            this.IsPurchase = true;
            string time = DateTime.Now.ToString();
            this.EventTime = time;
        }

        /// <summary>
        /// This constructor creates an event for a transaction where money is added or change is given
        /// </summary>
        /// <param name="eventType">Accepts a string describing transaction type</param>
        /// <param name="money">accepts decimal of money being transferred</param>
        /// <param name="balance"> accepts decimal of Balance property in Catering</param>
        public Event(string eventType, decimal money, decimal balance)
        {
            this.RunningTotal = balance;
            this.EventType = eventType;
            this.AddedMoneyOrChange = money;
            string time = DateTime.Now.ToString();
            this.EventTime = time;
        }
        
        /// <summary>
        /// Item quantity
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// When the purchase or transaction occured
        /// </summary>
        public string EventTime { get; private set; }

        /// <summary>
        /// This bool decides which string type will be displayed. Default is false
        /// </summary>
        public bool IsPurchase { get; private set; } = false;

        /// <summary>
        /// This property holds the amount a user added to their balance or the change they recieved
        /// </summary>
        public decimal AddedMoneyOrChange { get; private set; }

        /// <summary>
        /// Accepts a string describing the type of event; This string is only accepted through the constructor
        /// </summary>
        public string EventType { get; private set; }

        /// <summary>
        /// Holds the name of a catering item
        /// </summary>
        public string Item { get; private set; }

        /// <summary>
        /// Holds the item's identification code
        /// </summary>
        public string ItemCode { get; private set; }

        /// <summary>
        /// The total cost of all of a prticular item that has been purchased
        /// </summary>
        public decimal ItemCost { get; private set; }

        /// <summary>
        /// The total cost of a particular purchase session
        /// </summary>
        public decimal RunningTotal { get; private set; }

        /// <summary>
        /// Displays information concerning a purchase
        /// </summary>
        public string DisplayPurchase
        {
            get
            {
                return $"{this.EventTime} {this.Quantity} {this.Item} {this.ItemCode} {this.ItemCost.ToString("C")} {this.RunningTotal.ToString("c")}";
            }
        }

        /// <summary>
        /// Display information concerning a transaction (AddMoney or MakeChange)
        /// </summary>
        public string DisplayTransaction
        {
            get
            {
                return $"{this.EventTime} {this.EventType} {this.AddedMoneyOrChange.ToString("C")} {this.RunningTotal.ToString("c")}";
            }
        }

        /// <summary>
        /// Converts information to a csv friendly format
        /// </summary>
        public string CsvFriendlyFormat
        {
            get
            {
                return $"{this.Item}|{this.Quantity}|{this.ItemCost}";
            }
        }
    }
}
