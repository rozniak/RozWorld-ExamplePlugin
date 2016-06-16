# RozWorld-ExamplePlugin
An example plugin for RozWorld server using the RozWorld API.

## What is this
This is a basic library with an ExamplePlugin class that implements IPlugin, it demonstrates the simplicity of developing plugins for RozWorld. More features/tutorials will be added in future as RozWorld is updated/completed.

## Building this ting
Download and build the RozWorld-API, after downloading this project, make sure that the reference to the RozWorld-API build is correct and then build this project. Then just place the output library (should be ExamplePlugin.dll) into the server's "\plugins" folder.

Running the server should now automatically load the plugin from the library and you should see the output in the server's logger.