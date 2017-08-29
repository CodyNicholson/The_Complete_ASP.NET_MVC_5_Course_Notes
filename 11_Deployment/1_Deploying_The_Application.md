# Deploying The Application

In solution explorer, right click the project and go to **publish**. Depending on where we're going to deploy our application we can create a publish profile that includes all the settings. Next time we want to deploy again we can select a profile so we don't have to fill out all the settings again. In the publish menu we have three publish targets: **Microsoft Azure Website**, **Import**, and **Custom**.

**Import** is useful when there is an existing publish profile you want to use. In the previous versions of Azure, you would use import. Some web hosting companies may give you a publish profile.

**Custom** is used to create a profile from scratch. If we want to deploy this application to GoDaddy, then we would use this. After giving out custom profile a name, we have to set the **Publish Method**. The ones that you would probably use most of the time are **FTP** and **File System**. We also have **Web Deploy** which only works if the target IIS is configured to support this. It requires a few administrative tasks on the windows server. With this we can deploy directly to an IIS web site. **Web Deploy Package** is similar to Web Deploy except that it doesn't deploy directly to IIS. It creates a ZIP package that can later be used by a different process to deploy to IIS. The target IIS should support web deploy. Normally we use File System, so select that. Set the Target Location to: **C:/Deploy**.

Then move to the settings page. Here we have this Configuration dropdown menu with release and debug. If we are deploying to production then you should use release mode. Select release. Note that under Databases we get a message that database publishing is not supported for the publish method. If we go back to the connections tab and change the published method to **Web Deploy**, we get a checkbox option on the Settings tab to execute a Code-First Migration. With this, when your application starts up it automatically runs on your pending code first migrations on the database. So no matter what version your database is at, it will be automatically migrated to the latest version.

Now with our Publish Method as **File System** and our Target Location as **C:/Deploy** we can publish. In the target folder we have bin which includes the assemblies for our application.