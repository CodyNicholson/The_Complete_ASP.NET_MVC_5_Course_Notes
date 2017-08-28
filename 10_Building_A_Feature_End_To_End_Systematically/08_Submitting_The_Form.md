# Submitting The Form

We will start back at our **New.cshtml** file. First we will give this form the id **newRental** because we are going to use jQuery to get this element.

```
<form id="newRental">
    <div class="form-group">
        <label>Customer</label>
```

At the bottom of our our javascript code we will get the element with id **newRental** and we will call the **submit()** function which takes a function as a parameter. We will pass the argument **e** to this function which represents the submit event. In our function we will first call **e.preventDefault()**. This will prevent the form from submitting normally. If you don't do this, you will get a traditional HTML form. Instead, we want to use AJAX. So we call **$.ajax()** with **url** "/api/newRentals", **method** "post", and **data** vm. The **vm** has the customerId and the MovieId, which looks exactly like our **newRentalDto**. Finally we need to handle the success and fail scenarios, so we chain the **done()** method here and give it a function argument with no arguments. We also chain the **fail()** argument after that and pass it a function argument that also takes no arguments. To test if these work, we will simply log a message.

Here is the code from the bottom of the **$(document).ready()** call:

```
            $("#newRental").submit(function (e) {
                e.preventDefault();

                $.ajax({
                    url: "/api/newRentals",
                    method: "post",
                    data: vm
                })
                    .done(function () {
                        console.log("done");
                    })
                    .fail(function () {

                    });
            });
```
