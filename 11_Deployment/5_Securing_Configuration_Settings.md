# Securing Configuration Settings

Our application settings currently present a security risk, and we will go over how to fix it. This fix does present some complexity, so this should only be used in an application where security is a concern.

Currently we are storing the Facebook appSecret as plain text. Also, in the connection string section we don't have a user name and password, like in most real-world applications. When we check this code into our source control repository these secrets are visible to anyone who has access to that repository. This is especially a big concern if you're using a public repository like the on GitHub. So we need to exclude this from source control. 

To do this we go to solution explorer, right-click the project, add a new item. In the template in the Web tab select **Web Configuration File** and give it the name **AppSettings.config**. We can delete the default contents of the file and start fresh. Back in **Web.config** we can cut and paste our appSettings XML section into our **AppSettings.config** file. 

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

Now back in the **Web.config** we can add the appSettings section again, but this time we set the attribute **configSource** to **AppSettings.config**. Now from source control management perspective, we can exclude the **AppSettings.config** file from source control, and this will prevent anyone from finding our our secrets.

```
  <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-Vidly-20161229075632.mdf;Initial Catalog=aspnet-Vidly-20161229075632;Integrated Security=True" providerName="System.Data.SqlClient" />
  </connectionStrings>
  <appSettings configSource="AppSettings.config"></appSettings>
  <system.web>
```

You can also repeat this exact same process with other XML sections in your **Web.config**, like connectionStrings. Just use the attribute **configSource**. However, now there is another risk. These external **Web.config** files will be deployed with our application. On the target web server, these configuration files include our secrets in plain text. If a hacker gets access to the server, they can find all our secrets and get even more access. Potentially, they could get full access to the database and read a lot of private data or execute a script to mess up the data. To prevent this, we need to encrypt these files.

We will go over a simplified version of this process to see how everything works. In reality, there is a bit more complication to this. Let's say we are ready to deploy our application. We go to publish wizard and deploy to the file system. Before uploading these files to our web server, we need to encrypt our settings. So we search for Visual Studio Tools in our computers programs. Here we will find the developer command prompt. We can run this file as administrator. Here we are going to use one of the tools in .Net framework. We type: **aspnet_regiis -pef "appSettings" "C:/deploy" -prov "DataProtectionConfigurationProvider"**. With this provider, only the machine that encrypts our **Web.config** is the one that can decrypt it. Basically with this command we are telling the framework to encrypt the appSetting section of **Web.config**.

Now our appSettings have been encrypted to:

```
<appSettings configProtectionProvider="DataProtectionConfigurationProvider">
  <EncryptedData>
    <CipherData>
      <CipherValue>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAnrcYakJB8EeiAwu0Oy2w3gQAAAACAAAAAAAQZgAAAAEAACAAAABotv5Jv7bdr69PuNgXuF1nBiTs28kYrOyh4e0WtPZadgAAAAAOgAAAAAIAACAAAAAAyqbFqZh5gncTVN/9Ho7xCZ9hjZ8bLiaEGsfjmFQtSYADAABD3ykkbS1MwQK43qt0jwgbg7D99aShoVBzY91f8QcXPUMMhFdlbH38qDqtVGXepTzHtLlNUX8SuO+PndnxeJcyOiFZZkA1OTq2DaBWMP/srSnfK7X0rpeaIa74d+hpkJAS1OFZr8ZvELudKp1yzQossX585lyCh4znnotgA/+Cu9tngLt34V9S5uf9W3Q0R/nhk58JPAooPENKr9q1R9ygtI6WHPHQ4fHEawRc9XvOunjK+IeKGOu4zIULzRLPEd0rdtc6XPd5fFBUeC6/mcffI1RwLpEqjTG9HApp5DroGs7VUzlaphpKXe+P6En2zvT8CQOajSXm2PFihG8GjvHCqI8JPvOUIpuUkgdvUiwTImOVFEvNMEg5J+oHUerm94CIn08LzHUl674zvSpN7gU9vWgKzvibwI/1cvU/O6R0gKiJEYcDSJdEZJr3O+Io3+Jk+S7GdwzFAvK5B32/Qzd/NQrnJ1CUxrYPLcdKLdNqed74fa0/NhpoUaGrK1yBWM1HRKORLMH3xoPxYd6S5XrN/qS3eeDxujucCZTgE0Pso2PmHQAk0cc2dNbuQd8uXMQj0aHF/JbcPRbdFWb8KAoL1U71xU5fGuCxWOrdxMZU9UID6x9WB7fDr48JkmHOwbzf6vOKQokUqqTwausMJByz/0sIQFKni9/LOlPL9AVgcMOj47w1i2882Mm7p+3VfvDdMRM7hSEp0eiQUEbgFwbyLq5QFUtPYLwx9nizRfrjMUyDia0ydcweaCG5dYbpRpeO5Mjwo7CB+qAKJxEjRkEzE9O2/fX7Q1m8uAPjj5COTxCRac6bIWJsNQQBAZ6RKdeXRZMTEvBTYfmhkHawrUf2EPH/l3c7itkiJv8pRp4Ea85UPfwdqLdoLbZbIBRX4a4WjY/zYYHU9LRBOQijf1+FquxfEs9FmtIwX65Ijv3GoYD/wGPGv9Gp25tXJlS82xsGoJBC+ZGJRsMvxBDO06JzxFfE5n8MHLJ0uRnJSwbOUmnxYITEhdF75WynI9oeuE9CbfJIhNrMNxa1mrMIVMU68s3r4MfQiXBC50QwFCUUHD5VNic+Ms2g18EsmhiDEOb5pwl1mAYUsV4VkIKsvhzYtoVFXE+P8nuMhftpNXZbrGpE1EJDkvcsmB5Q4Avs3aMrYvDYKjdLoUgv196+L2d70lqU0Ys3Q7lGPo0wYRTJjkAAAAC8GXgvB4ISL1GtUB6wxKOjEw1e+ys7hpQ28UUVYq9R0oAQRFxtagE/eCFBeW6spjdTlDojY0iYGCSdENUFgQTM</CipherValue>
    </CipherData>
  </EncryptedData>
</appSettings>

```

Even though our appSettings is encrypted, ASP.NET MVC runtime can decrypt and extract the values. To decrypt this using visual studios tools administrator developer command prompt we can run: **aspnet_regiis -pdf "appSettings" "C:\deploy"**. Now we have our values:

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
