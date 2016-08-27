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
 * ~02-ExamplePlugin-HelloCommand.cs~ (Writing a very basic command)
 * 
 * In this code file, we will write a very simple plugin that determines whether the sender was the server itself, a human player, or
 * a (local) bot player. On top of that, this will demonstrate how easy it is the send a chat message to any of them.
 * 
 * This function is registered as '/hello' and will activate whenever '/hello' is sent in game chat by any entity capable of executing
 * commands.
 */

using Oddmatics.RozWorld.API.Server;
using Oddmatics.RozWorld.API.Server.Entities;
using System.Collections.Generic;

namespace ExamplePlugin
{
    /// <summary>
    /// Represents the IPlugin class that will be loaded by the server.
    /// </summary>
    public partial class ExamplePlugin : IPlugin
    {
        // Notice the structure of this function, commands always take the parameters you see here:
        private bool CommandHello(ICommandCaller sender, IList<string> args)
        {
            // This is the command function body, will be executed whenever /hello is sent

            // You can check whether the sender is the server console itself or a player like so
            if (sender is IRwServer)
                sender.SendMessage("Hello server operator!"); // This function will send a chat message to the sender
            else
            {
                // If the sender is not the server, it must be a player of some kind. The following
                // will cast the sender as a Player object, on which we can then check to see if
                // they are human or bot:
                var player = (Player)sender;

                if (player.IsRealPlayer)
                    sender.SendMessage("Hello player!");
                else
                    sender.SendMessage("Hello bot!");
            }

            // Finally, since the command was executed successfully, we can return true back to the caller
            return true;
        }
    }
}
