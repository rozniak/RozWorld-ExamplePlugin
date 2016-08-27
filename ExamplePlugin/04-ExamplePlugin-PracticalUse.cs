/**
 * ExamplePlugin.ExamplePlugin -- Example RozWorld Plugin Demo
 *
 * This source-code is part of the example plugin tutorial for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-ExamplePlugin>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 * 
 * ---
 * 
 * ~04-ExamplePlugin-PracticalUse.cs~ (Putting lessons to practical use)
 * 
 * In this code file, we will put the lessons so far into practical use by making a useful plugin feature. Here, we will be
 * making a 'broadcaster' used to send chat messages every so often for useful messages or possibly advertisements.
 * 
 * There is also a command that will be used to change the interval of the time or enable/disable it.
 */

using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.API.Server;
using System;
using System.Collections.Generic;
using System.Timers;

namespace ExamplePlugin
{
    /// <summary>
    /// Represents the IPlugin class that will be loaded by the server.
    /// </summary>
    public partial class ExamplePlugin : IPlugin
    {
        // This is the Timer that will be used for sending chat messages at intervals over time
        // in this plugin. It has its Elapsed event attached in the Starting and Stopping events
        // shown in lesson 1.
        private Timer BroadcastTimer;

        // This collection will be populated with the strings to broadcast.
        private IList<string> BroadcastMessages;

        private Random Random;


        private bool CommandBroadcaster(ICommandCaller sender, IList<string> args)
        {
            // Here we will check the arguments in order to determine what operation to carry
            // out on this broadcasting system. Either toggling it on/off, changing the interval
            // or viewing help for the command:
            if (args.Count == 0)
            {
                // Display help since no arguments were passed.
                sender.SendMessage("Usage for /broadcaster:");
                sender.SendMessage("/broadcaster interval <milliseconds>");
                sender.SendMessage("/broadcaster toggle");
            }
            else if (args.Count == 1 && args[0].ToLower() == "toggle")
                BroadcastTimer.Enabled = !BroadcastTimer.Enabled;
            else if (args.Count == 2 && args[0].ToLower() == "interval")
            {
                int newInterval = (int)BroadcastTimer.Interval;

                if (int.TryParse(args[1], out newInterval))
                    BroadcastTimer.Interval = newInterval;
                else
                    return false;
            }
            else
            {
                // If none of the above clauses were matched, then invalid arguments were
                // passed and so the sender should be alerted.
                sender.SendMessage("Invalid arguments passed to /broadcaster.");
                return false;
            }

            return true;
        }


        private void BroadcastTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // If there are messages loaded, randomly pick one to broadcast
            if (BroadcastMessages.Count > 0)
            {
                int messageIndex = Random.Next(0, BroadcastMessages.Count);

                RwCore.Server.BroadcastMessage(BroadcastMessages[messageIndex]);
            }
        }
    }
}
