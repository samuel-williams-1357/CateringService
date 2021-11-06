using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Classes;

namespace CapstoneTests
{
    [TestClass]
    public class EventLoggerTests
    {
        // Checks that Event logger is correctly adding events to the log file.
        [TestMethod]
        public void EventLoggerCorrectlyAddsEventsToFile()
        { 
            //Arrange
            EventLogger logTest = new EventLogger();
            // Catering obj is required for constructor of event obj
            CateringItem testItem = new CateringItem();
            Event testEvent = new Event(testItem, 100);

            // To know where to test logged file from
            FileAccess testLogOutput = new FileAccess();
            // This is a method called from File Access which takes in an EventLogger param
            testLogOutput.LogOutput(logTest); 

            //Act
            logTest.AddEvent(testEvent);

            //Assert
            // Checking that the logTest Even logger obj which has the array AllEvents has an array Length of 1 since...
            // ...one event was passed in.
            Assert.IsTrue(logTest.AllEvents.Length == 1);
        }
    }
}
