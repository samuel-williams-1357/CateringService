using System;
using Capstone.Classes;

namespace Capstone
{
    /// <summary>
    /// The main entry point for this application.
    /// </summary>
    /// <remarks>
    /// You should not need to modify this file. If you believe you do, ask your instructor.
    /// </remarks>
    public class Program
    {
        public static void Main(string[] args)
        {
            // This is the only code that goes here
            // DO NOT CHANGE THIS CODE
            UserInterface userInterface = new UserInterface();
            userInterface.RunMainMenu();
        }
    }
}
