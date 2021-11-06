using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// Creates array of events to output to Log.txt
    /// </summary>
    public class EventLogger
    {
        // Creating list object to hold events
        private List<Event> log = new List<Event>();

        /// <summary>
        /// Adds Events to List
        /// </summary>
        /// <param name="eventLog"></param>
        public void AddEvent(Event eventLog)
        {
            log.Add(eventLog);
        }

        /// <summary>
        /// Returns log list to an array
        /// </summary>
        public Event[] AllEvents
        {
            get
            {
                return log.ToArray();
            }
        }
    }
}
