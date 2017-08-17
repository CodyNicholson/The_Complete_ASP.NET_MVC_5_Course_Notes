# Camel Notation For API JSON Data

To make our JSON data look more clean we can configure it to follow **Camel Notation**

Camel Notation means that every variable name will start with a lowercase word and every following word will be uppercased like this: thisIsAnExampleOfCamelNotation

***

To do this we will go to the WebApiConfig.cs file located in the App_Start directory

In this file we can create a settings variable, then we add Camel Noation, and lastly we add the indentation on in the first three lines of the Register() method

```cs
namespace Vidly
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
```
