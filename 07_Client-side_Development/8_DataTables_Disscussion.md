# DataTables Discussion

Currently our DataTable gets all customers from our API and stores them internally to do its pagination, searching, and sorting. This approach works well when you have a few hundred to roughly a couple thousand records. This depends on the number of objects you return from your API, and is just an estimate.

In our **Customers API** each customer object has only a few properties. If we returned a larger customer object, then a thousand records would probably be the highest limit. If you have a lot of records then you should do your pagination, searching, and sorting on the server.

To do this we need to explicitly implement these features on the server side. Then, in the DataTables we need to enable server-side processing.

***

Currently in our JavaScript code in the Customers **Index.cshtml** page we don't have proper separation of concerns. Some parts are for the view, other parts are about data access and call our backend API. These are two different responsibilities and should be separate. We need to separate the part that is responsible for the view and the part that is data access. It is also better to extract the JavaScript code and put it into its own JavaScript file.
