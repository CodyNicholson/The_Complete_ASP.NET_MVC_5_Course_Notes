# Release Builds

In terms of the application tier, we have looked at **Output Caching** and **Data Caching** strategies. Another simple optimization technique is using **Release Builds**.

In Visual Studio there is a drop down menu next to the run application button on the top toolbar where you can select the build mode. By default it is set to debug. When you compile your application with debug mode, the compiler adds additional data in assemblies which are used for debugging. When it comes to deployment we don't need this stuff. So we should use the release build mode which will produce slightly faster and smaller assemblies.

It is safe to implement this application tier optimization without performance profiling because they don't add any complexity and it is simple
