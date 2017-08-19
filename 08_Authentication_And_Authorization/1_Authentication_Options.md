# Authentication Options

When you create a new ASP.NET application you will be prompted with a button the change the authentication type. The four options are:

- No authentication
- Individual user accounts
- Organizational accounts
- Windows authentication

**No authentication** is used for applications that don't need authentication services and are accessible to anonymous users. **Individual User Accounts** is the default authentication option for ASP.NET MVC projects, and is suitable for internet websites where you want to provide a form for the user to login. With this option you can also enable social media logins like Facebook, Google, Twitter, and so on. **Organizational Accounts** are useful if you want to enable single-sign-on for internal and cloud apps using active directory. Lastly, **Windows Authentication** is useful for internet applications. In this set up, when the user logs into their desktop computer on the network they will be authenticated to your application automatically.

**Individual User Accounts** is the most popular, so we will use this one for our application
