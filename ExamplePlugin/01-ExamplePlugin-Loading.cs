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
 * ~01-ExamplePlugin-Loading.cs~ (Loading and unloading your plugin)
 * 
 * In this code file, we will handle the loading and unloading of this example plugin by using the Starting and Stopping events on the
 * server respectively. By attaching this plugin to those events, we can manage safe startup and shutdown of the plugin and any resources
 * it uses. This may be useful for plugins for example, database plugins, so that they can open up a connection to an SQL server on startup
 * and disconnect from the SQL server on shutdown.
 * 
 * Here we will also implement the three informative properties that are part of the IPlugin interface itself: Description, Name and
 * Version. These are mostly for server admins, or they can come in handy for your own plugin if you wish to use them in some kind
 * of update reminder service.
 * 
 * Most importantly, and what you will use most often in the Starting event handler, are the registry of commands and permissions used by
 * your plugin. This *must* be done at server startup, as no more commands or permissions may be registered after the server has loaded
 * plugins.
 */

using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.API.Server;
using System;
using System.IO;
using System.Timers;

namespace ExamplePlugin
{
    /// <summary>
    /// Represents the IPlugin class that will be loaded by the server.
    /// </summary>
    public partial class ExamplePlugin : IPlugin // Make sure that your plugin class uses the Server.IPlugin interface
    {
        // This constant defines the directory that shall be used to store this plugin's data
        public static readonly string DIRECTORY_EXAMPLEPLUGIN = Environment.CurrentDirectory + @"\ExamplePlugin";

        public static readonly string FILE_BROADCAST_MESSAGES = DIRECTORY_EXAMPLEPLUGIN + @"\broadcastmsgs.txt";


        // Here we implement the three informative properties, you can see the kind of data to
        // populate them with in your own plugin.
        public string Description { get { return "An example plugin for RozWorld."; } }
        public string Name { get { return "Example Plugin"; } }
        public string Version { get { return "0.1.0"; } }


        public ExamplePlugin()
        {
            // The constructor is where events for Starting and Stopping must be attached,
            // you may also attach to other events here but right now, no commands or
            // permissions can be registered. It is bad practice to use the constructor
            // for much more than the following two lines:
            RwCore.Server.Starting += new EventHandler(Server_Starting);
            RwCore.Server.Stopping += new EventHandler(Server_Stopping);
        }


        private void Server_Starting(object sender, EventArgs e)
        {
            // You can use RwCore.Server.Logger.Out in order to output messages directly into
            // the server logger. It would be helpful for you to put in the name of your plugin
            // so that server administrators know who generated the message.
            RwCore.Server.Logger.Out("[PLUGIN] ExamplePlugin: This is an example plugin starting up!");


            // Here we register the commands that this plugin will use: /broadcaster, /hello and /hooktest
            // You will learn about these commands later.
            RwCore.Server.RegisterCommand("broadcaster", CommandBroadcaster, "Manage example broadcaster.", "/broadcaster ?");
            RwCore.Server.RegisterCommand("hello", CommandHello, "Test command.", "/hello");
            RwCore.Server.RegisterCommand("hooktest", CommandHookTest, "Hook test command.", "/hooktest");


            // FOR LESSON 4:
            // Here we will attach the event for the BroadcastTimer:
            BroadcastTimer.Elapsed += new ElapsedEventHandler(BroadcastTimer_Elapsed);

            // Here we will populate the broadcasting strings from a local folder on the disk.
            // Check if it exists first, make it if it doesn't.
            if (!Directory.Exists(DIRECTORY_EXAMPLEPLUGIN))
                Directory.CreateDirectory(DIRECTORY_EXAMPLEPLUGIN);

            if (File.Exists(FILE_BROADCAST_MESSAGES))
                BroadcastMessages = File.ReadAllLines(FILE_BROADCAST_MESSAGES);
            else
                File.Create(FILE_BROADCAST_MESSAGES);

            Random = new Random();

            BroadcastTimer.Interval = 15000;
            BroadcastTimer.Enabled = true;
            BroadcastTimer.Start();
        }

        private void Server_Stopping(object sender, EventArgs e)
        {
            // This is where the plugin should release resources (similar to Dispose()) for
            // proper shutdown.

            // Here we will detach the event for the BroadcastTimer used in lesson 4:
            BroadcastTimer.Elapsed -= BroadcastTimer_Elapsed;
        }
    }
}
