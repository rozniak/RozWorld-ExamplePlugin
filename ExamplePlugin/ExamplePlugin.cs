using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.API.Server;
using System;
using System.Collections.Generic;
using System.Timers;

namespace ExamplePlugin
{
    /// <summary>
    /// Represents the IPlugin class that will be loaded by the server.
    /// 
    /// This is an example plugin used to demonstrate the simplicity of developing a plugin, here, this
    /// plugin will:
    ///     > Output a startup message
    ///     > Register a command, /hello, that outputs a message to the sender depending on who it is
    ///     > Output a broadcast message via the server every 5 seconds
    ///     
    /// You can use this plugin code as a starting point for learning to develop your own, guides will be
    /// posted later in development of RozWorld for more advanced features when they're available. :)
    /// </summary>
    public class ExamplePlugin : IPlugin
    {
        public string Description { get { return "An example plugin for RozWorld."; } }
        public string Name { get { return "Example Plugin"; } }
        public string Version { get { return "0.1.0"; } }


        private Timer BroadcastTimer;


        public ExamplePlugin()
        {
            // Attach core start up event to the server's plugin startup event
            RwCore.Server.Starting += new EventHandler(Server_Starting);
        }


        void Server_Starting(object sender, EventArgs e)
        {
            // Register our test command, /hello
            RwCore.Server.Logger.Out("[PLUGIN] This is an example plugin starting up!");
            RwCore.Server.RegisterCommand("hello", CommandHello, "Test command.", "/hello");

            // Set up broadcast timer
            BroadcastTimer = new Timer(5000);
            BroadcastTimer.Elapsed += new ElapsedEventHandler(BroadcastTimer_Elapsed);
            BroadcastTimer.Enabled = true;
            BroadcastTimer.Start();
        }


        void BroadcastTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RwCore.Server.BroadcastMessage("Hello world!");
        }

        bool CommandHello(ICommandCaller sender, IList<string> args)
        {
            // This is the command function, will be called whenever /hello is sent

            // You can check whether the sender is the server console itself or a player like so
            if (sender is IRwServer)
                sender.SendMessage("Hello server operator!");
            else
                sender.SendMessage("Hello player!");

            // More advanced stuff can be done of course, this is just a very small example

            return true;
        }
    }
}
