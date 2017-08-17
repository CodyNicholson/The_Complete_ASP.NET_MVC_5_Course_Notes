# What Is A Web API?

When a request arrives at our application, MVC Framework hands off that request to an action in a Controller

This action will return a view most of the time, which is then parsed by Razor View Engine and then eventually HTML markup is returned to the client

In this approach, HTML markup is generated on the server and then returned to the client

There is an alternative way to generate HTML markup - you can do it on the client side

Instead of our action returning markup, they can return raw data

The benefits of returning raw data to the client are:

- Uses less server resources (improves scalability)
- Raw data often requires less bandwidth than HTML markup
- We can build a broad range of clients, like mobile and tablet apps that simply call our **endpoints** to get the data and generate the view locally

We call these endpoints **data services** or **web APIs** because they just return data, not markup

We can also use these services for modifying data
