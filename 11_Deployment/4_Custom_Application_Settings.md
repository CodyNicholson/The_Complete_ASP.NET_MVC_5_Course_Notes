# Custom Application Settings

In most applications we have one of more settings that can impact how the application operates. For example, our application may use a mail server to send emails, so it needs to know the address of the mail server as well as the user name and password. As a best practice, we should not store the settings in the code. Instead, we store them in a configuration file, and that's why our **Web.config** exists.

In our **Web.config** we have this section called app settings where we can store application settings using key value pairs:

```
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  </appSettings>
```

By default we have four settings here which are basically there because of the legacy stuff in ASP.NET MVC, and how it has evolved from version 1 to version 5. For example, we have settings for enabling client-side validation or unobtrusive javascript. We don't really need to touch any of this stuff as they're used internally by the MVC framework. Now will will use application settings in a real-world example.

Earlier we added an **appId** and **appSecret** for facebook authentication in our **Startup.Auth.cs** file. The values are hardcoded into this file. We can make this better by moving them into application settings back in the **Web.config** file like this:

```
  <appSettings>
    <add key="FacebookAppId" value="1485256738212345" />
    <add key="FacebookAppSecret" value="5b2b4d06955e4119b602631s3f5g6j8k" />
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  </appSettings>
```

Back in our **Startup.Auth.cs** file we can reference our app settings like this:

```
            app.UseFacebookAuthentication(
               appId: ConfigurationManager.AppSettings["FacebookAppId"],
               appSecret: ConfigurationManager.AppSettings["FacebookAppSecret"]);
```

We have seen how to use the appSettings to hold data that is unique to our build. But how do we make it so that these appSettings vary based on the type of build (Production or testing). This is where our build configurations come into the picture. Let's say that in our production build we want to have a mail server with a certain value, but in our testing build we want the mail server to have a different value. In our parent **Web.config** we might have:

```
  <appSettings>
    <add key="MailServer" value="dev-smtp.vidly.com" />
    <add key="FacebookAppId" value="1485256738210407" />
    <add key="FacebookAppSecret" value="5b2b4d06955e4119b60263753cdc833c" />
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  </appSettings>
```

To change this in our Testing build configuration, we go to the **Web.Testing.config** file. Here we add the appSettings section by using the tag. Then we create a new add tag in the appSetting section to hold the mail server value. In order to override the parent **Web.config** file we need to apply a transformation. The transformation attributes that have been added to the mail server add tag below tells our application that when it finds an element with the same attribute specified in the **Match()** set all of the attributes based on the values in this file, and not the other file. In our **Match** call below, we send it the *key*. So the 

```
  <appSettings>
    <add key="MailServer" value="test-smtp.vidly.com" 
         xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
  </appSettings>
```

To test if the above transformation worked we can go to solution explorer, right-click on the **Web.testing.config** file and then select **Preview Transform**. This will show us the difference between our build configurations. In this case, the **Data Source** is different, the value for the MailServer is different, and the debug is turned off in the production build configuration but turned on in testing build configuration.
