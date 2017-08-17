# Client-Side Validation

Having server-side validation is great for making our application secure, but we also want to have **Client-Side Validation** for two reasons:

1. Immediate feedback
2. No waste of server-side resources

-

### Enabling Client-Side Validation

If you look at your BundleConfig.cs file in the App_Start folder you will see this:

```
bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
```

This is the jQuery validation script included in the project

To enable it in your views you can put this at the very bottom of your .cshtml file:

```html
@section scripts
{
    @Scripts.Render("~/bundles/jqueryval")
}
```

This enables the jQuery validation bundle which will validate your fields based on the Data Annotations you defined in your models without needing to go back and forth to the server
