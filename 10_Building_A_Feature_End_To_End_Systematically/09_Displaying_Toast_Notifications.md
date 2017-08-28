# Displaying Toast Notifications

### Installing Toastr

To display our toast notifications we are going to use a popular jQuery plugin called **toastr**. In package manager console run: **install-package toastr**. Now we need to go to our **BundleConfig.cs** file and add **"~/content/toastr.css"** to the css bundle and **~/scripts/toastr.js** to the lib bundle.

***

### Using Toastr

In our **New.cshtml** we want to display a toast message when our program is done, and a different toast message when our program fails. To do this we use **toastr.success()** and **toastr.error()** like so:

```js
            $("#newRental").submit(function (e) {
                e.preventDefault();

                $.ajax({
                    url: "/api/newRentals",
                    method: "post",
                    data: vm
                })
                    .done(function () {
                        toastr.success("Rentals successfully recorded.");
                    })
                    .fail(function () {
                        toastr.error("Something caused an error.");
                    });
            });
```
