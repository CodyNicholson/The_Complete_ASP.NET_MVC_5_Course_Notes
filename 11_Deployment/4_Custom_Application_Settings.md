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
