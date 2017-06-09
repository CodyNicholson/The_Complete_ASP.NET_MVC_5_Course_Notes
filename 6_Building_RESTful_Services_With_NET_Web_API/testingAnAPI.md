# Testing An API

After creating our API, we can view our api by going to the URL in our project: /api/customers

Here you will see all of the data in our database in XML

This is possible because ASP.NET MVC has a **Media Formatter** which will format the database data int JSON/XML based on what the client asks

If we do not set the **Content-Type** header in our requests then by default our data will be formatted into XML

***

### Testing Our API With Postman

Postman is a Google Chrome app you can install to test your APIs without breaking anything

Once you download and setup the application you can give it the URL that contains your data, "http://localhost:61167/api/customers", and then test posting data using your API by setting the URL bar type to **POST**, the Header **key** to **Content-Type**, the **Header key value** to **application/json**, and the body to **Raw JSON** with an some data in JSON format without an ID (Because the Id will be generated for you when you post the customer)

#### JSON Data Example

```js
{
  "Name": "Liam",
  "IsSubscribedToNewsletter": true,
  "MembershipType": null,
  "MembershipTypeId": 2,
  "Birthdate": "1996-07-28T00:00:00"
}
```

Then when you click **SEND** the test should run without error and return "200 OK" - meaning that your request was accepted
