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
 * ~03-ExamplePlugin-ChatHook.cs~ (Writing a simple chat hook command)
 * 
 * In this code file, we will utilise the chat hooking feature in order to perform an example of taking control of a user's chat
 * system for contextual command verification. This can be helpful for verifying an action a user is taking, if it is an important
 * one, or simply asking for more information during a given context of a command.
 * 
 * Hooking is done via a request method, this is such that hooking can be managed between control of commands that request a chat
 * hook (no command can override another command's hook without permission). If hooking is available (no command is currently
 * hooked), a token is provided in exchange for the delegate that will be called when a chat message is received by the sender that
 * was 'hooked'. This token is then used to release the hook, allowing another command to take control should they need it.
 */

using Oddmatics.RozWorld.API.Server;
using System.Collections.Generic;

namespace ExamplePlugin
{
    /// <summary>
    /// Represents the IPlugin class that will be loaded by the server.
    /// </summary>
    public partial class ExamplePlugin : IPlugin
    {
        // Store the token in a place safely for use later in order to release a hook
        private int HookToken;


        private bool CommandHookTest(ICommandCaller sender, IList<string> args)
        {
            // We should attempt to retrieve a token before asking the question, to make
            // sure that the question can be answered.
            HookToken = sender.HookChatToCallback(ChatHookTest);

            if (HookToken != -1) // HookChatToCallback returns -1 as the 'token' upon a failure
            {
                // Now ask the question to the sender, since we know they can answer it:
                sender.SendMessage("Do you like ice cream? (yes/no)");

                return true;
            }
           
            // The hook request was denied, so this command failed.
            return false;
        }

        private bool ChatHookTest(ICommandCaller sender, string message)
        {
            // Here we can now check the answer that the sender has given, this function
            // will be called when the user sends a message after using /hooktest.
            if (message.ToLower() == "yes")
                sender.SendMessage("That's nice, I like ice cream as well!");
            else
                sender.SendMessage("That's a shame!");

            // Now release the chat hook using the token obtained originally from the
            // request.
            //
            // NOTE: You do not necessarily have to release the hook on the first 'answer',
            // you can keep the hook in place as long as it is required, or even remove it
            // as a result of another function, as long as you share the token.
            //
            // This could mean that you use a System.Timers.Timer to release a hook if a
            // user does not respond to a question or request in a certain time period.
            sender.ReleaseChatHook(HookToken);
            HookToken = 0;

            return true;
        }
    }
}
