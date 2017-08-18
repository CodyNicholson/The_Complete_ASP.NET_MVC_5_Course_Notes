# Single Page Applications

With out implementation we're generating the Customers table on the client. In other words, we are regenerating a part of the view on the client. We could take this to the next level and generate the view of the entire page on the client. With this architecture on the server-side it will have a number of APIs that are purely responsible for getting or modifying data. All the views will be generated on the client which means we won't be using MVC razor views. Applications built with this kind of architecture are called **Single-Page Applications**.

In a single-page application, when a user clicks on a link, instead of sending a request to the server to get an entire markup for the new page, we make an AJAX call to an API, get the required data, and generate the view on a client. Then we replace the view in the main content area with another view.

Essentially we have a single page whose content gets updated as the user navigates from one area of the application to another.

### What is the benefit?

It makes the user experience faster and smoother because we don't have to reload an entire page as the user navigates from one area to another, so the page doesn't flicker.
