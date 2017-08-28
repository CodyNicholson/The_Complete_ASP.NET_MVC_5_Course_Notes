# Implementing Client-Side Validation

In this section we will add custom validation to our New Rentals Form in our **New.cshtml** file. Currently, in **BundleConfig.cs**, we have a separate bundle for jQuery validation:

```
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate*"));
```

We can merge this with our lib bundle if we are going to use it in a lot of different places. For now we will keep them separate and include jQuery validation only on pages where we need it. So in the **@scripts** section in our **New.cshtml** file we can call **@Scripts.Render("~/bundles/jqueryval")**.

```
@section scripts
{
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        $(document).ready(function () {
```

Next we will go to the top of the form, in the customers input we can use standard Html validation attributes like **required** and jQuery validation will understand this.

```
<form id="newRental">
    <div class="form-group">
        <label>Customer</label>
        <div class="tt-container">
            <input id="customer" required type="text" value="" class="frm-control" />
        </div>
    </div>
```

Finally, we need to plug in the validation. We go to the place in our @scripts section where we handle the form submission event: **$("#newRental").submit(function (e)**. Before this, we will use jQuery to get a reference to our form at id: **newRental**, and then call **validate()**. This plugs validation into this form.

```
            $("#newRental").validate();

            $("#newRental").submit(function (e) {
                e.preventDefault();
```

There is a problem here. With the current implementation our form will be submitted even if it is not valid. So we need to get the code inside the submit handler and move it inside the **validate()**. First, we pass an object to the **validate()** that is the configuration of our validation. We set a field **submitHandler** to be a function that will be called to submit the form to the server if it is valid. Now we move the code from the **submit()** into the **validate()**:

```
            $("#newRental").validate({
                submitHandler: function () {
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
                }
            });

            $("#newRental").submit(function (e) {

            });
```

We can then delete the **submit()**

Now our validation message will appear when we try to submit an invalid form, but notice the text is black. We should make this red. We go to the **site.css** file and we can add css to make the text black. The class of our validation message elements is **error**. We also want our invalid fields to have a red border.

```
.field-validation-error,
label.error {
    color: red;
}

.input-validation-error,
input.error {
    border: 2px solid red;
}
```

These are the basics of validation

Currently our validation only gives us an error if the field is empty and you try to submit. It will not give an error if you try and submit with an invalid customer name, so we should add this.

Back in the **New.cshtml**, before calling the **validate()** method, we can add a custom validator using: **$.validator.addMethod()**. We can give this method the name **validCustomer**. Then we can create our validation function that jQuery will call. We return **vm.customerId** and **vm.customerId != 0**. Then we specify an error message right after this. 

```
            $.validate.addMethod("validCustomer", function () {
                return vm.customerId && vm.customerId !== 0;
            }, , "Please select a valid customer.");

            $("#newRental").validate(
```

Now to apply this we go back to our customer input and apply a data attribute:

```
        <label>Customer</label>
        <div class="tt-container">
            <input id="customer" data-rule-validCustomer="true" required type="text" value="" class="frm-control" />
        </div>
```

The jQuery validation plugin looks for custom attributes that start with data-rule

***

In order to prevent the form from being submitted normally, we need to use a different technique than the **e.preventDefault()** because we no longer have **e**. By returning false at the end of the function, we can make it so that the form will not be submitted unless it is valid.

```
            $("#newRental").validate({
                submitHandler: function (e) {
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
                    return false;
                }
            });
```

We also need to add the **name** attribute to our input textboxes because jQuery relies on this attribute to do its validation.

```
    <div class="form-group">
        <label>Customer</label>
        <div class="tt-container">
            <input id="customer" name="customer" data-rule-validCustomer="true" required type="text" value="" class="frm-control" />
        </div>
    </div>

    <div class="form-group">
        <label>Movie</label>
        <div class="tt-container">
            <input id="movie" name="movie" type="text" value="" class="frm-control" />
        </div>
    </div>
```

Also, when we have successfully record rentals, we need to clear the form. So we get the inputs and the movies list use typeahead and the empty() method to clear the values:

```
                        .done(function () {
                            toastr.success("Rentals successfully recorded.");

                            $("#customer").typeahead("val", "");
                            $("#movie").typeahead("val", "");
                            $("#movies").empty();
                        })
```

The three lines above will clear the form but we also have to reset our view model:

```
                        .done(function () {
                            toastr.success("Rentals successfully recorded.");

                            $("#customer").typeahead("val", "");
                            $("#movie").typeahead("val", "");
                            $("#movies").empty();

                            vm = { movieIds: [] };
                        })
```

Finally, we need to reset the state of the form in terms of its validation. So the **validate()** method returns a validator object, and we can save that and use that object and call **validator.resetForm()** to reset the form in terms of validation.
