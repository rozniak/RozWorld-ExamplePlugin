using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.API.Server;
using System;
using System.Timers;

namespace ExamplePlugin
{
    /// <summary>
    /// Represents the IPlugin class that will be loaded by the server.
    /// 
    /// This is an example plugin used to demonstrate the simplicity of developing a plugin, here, this
    /// plugin will:
    ///     > Output a startup message
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
            RwCore.Server.Logger.Out("[PLUGIN] This is an example plugin starting up!");

            // Initialise your stuff here
            BroadcastTimer = new Timer(5000);
            BroadcastTimer.Elapsed += new ElapsedEventHandler(BroadcastTimer_Elapsed);
            BroadcastTimer.Enabled = true;
            BroadcastTimer.Start();
        }

        void BroadcastTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RwCore.Server.BroadcastMessage("Hello world!");
        }
    }
}
