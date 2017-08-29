# Build Configurations

Imagine we want to create a build configuration for testing or staging environments. In these environments we probably have different settings. For example, connection strings can be different, our application settings can be different, we may use a different mail server or a message box and so on. So, on the tool bar in visual studio from the solution configuration dropdown list we select **Configuration Manager**. Then in the **Active solution configurations** dropdown select **New**. Now we will create a new build configuration for our testing environment. We can copy the settings from an existing template. For environments like testing and staging we should use the release build template, and leave debug for only when you're developing on your development server. Now we can ad the build configuration and set that as the selected configuration.

Now that we have a new build configuration if we go to solution explorer and right-click on the **Web.config** file we see a new menu option called **Add Config Transform**. Clicking this option will create new config files. We can take the connection strings that are commented out in these new config files and uncomment them to override the connection string for our database.

In the **Web.Testing.config** file we comment out the connection string XML and change the name attribute in the add tag to *DefaultConnection* because in our parent **Web.config** file, this is the name we use for our connection string. Back in the **Web.Testing.config**, you will see that we have a different connection string than in the parent file. Data Source is **ReleaseSQLServer**. So when we deploy our application, this configuration will transform our parent **Web.config**. We can add any other settings that are specific to our testing environment in the child files.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
    <connectionStrings>
      <add name="DefaultConnection" 
        connectionString="Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True" 
        xdt:Transform="SetAttributes" xdt:Locator="Match(name)"/>
    </connectionStrings>
  <system.web>
    <compilation xdt:Transform="RemoveAttributes(debug)" />
  </system.web>
</configuration>
```

Finally, in solution explorer, in **~/Properties/PublishProfiles** You will see all the publish profiles we have created. These files are there so that we can deploy to production, or to testing if we create that profile.
