# Adding A Theme

To add a theme to your MVC 5 application you can go to bootswatch.com, find a theme, download that theme's corresponding CSS file, and include it in your project

After you download the CSS file you like you should put it into the **Content** folder where we store all of our client side assets

Once this is done, we have to include this stylesheet in our project by opening **BundleConfig.cs** in the **App_Start** folder

In the BundleConfig.cs file, you will see (at the bottom under where it initializes the StyleBundle object) the existing CSS stylesheet - you should replace this stylesheet with the one you chose and put into the Content folder
