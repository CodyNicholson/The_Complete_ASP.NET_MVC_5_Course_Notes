# Building The Frontend

In this section we will build the frontend and learn a different way to build forms. Before, we used a helper function like **@Html.BeginForm()**, **@Html.LabelFor()**, **@Html.TextBoxFor()**, **@Html.ValidationMessageFor()**, and so on. Like in this example from the **CustomersForm.cshtml**:

```
@using (@Html.BeginForm("Save", "Customers")) // The "using" block will create an ending "</form> tag for us
{
    @Html.ValidationSummary(true, "Please fix the following errors:")
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Name)
        @Html.TextBoxFor(m => m.Customer.Name, new {@class = "form-control"})
        @Html.ValidationMessageFor(m => m.Customer.Name)
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.MembershipTypeId)
        @Html.DropDownListFor(m => m.Customer.MembershipTypeId, new SelectList(Model.MembershipTypes, "Id", "Name"), "Select Membership Type", new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.Customer.MembershipTypeId)
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Birthdate)
        @Html.TextBoxFor(m => m.Customer.Birthdate, "{0:d MMM yyyy}", new {@class = "form-control"})
        @Html.ValidationMessageFor(m => m.Customer.Birthdate)
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.Customer.IsSubscribedToNewsletter) Subscribe to newsletter?
        </label>
    </div>
    @Html.HiddenFor(m => m.Customer.Id)
    @Html.AntiForgeryToken()
    <button type="submit" class="btn btn-primary">Save</button>
}
```

In this kind of form, when the user clicks the submit button they have to wait for the server to respond. Quite often the browser goes blank for a second or two if not more until the server responds. There is a different way to post data with a server which creates a faster and smoother user experience. We use **AJAX** - which means instead of posting data using a traditional html form, we call the server asynchronously in the background. This way the page doesn't flicker until the server responds. In this section we're going to use AJAX to call our new rental API.

Now let's build a very basic form for renting movies. We will start by adding a **RentalsController.cs**. We will create an action called **New()** that we will use to return the form to the client. We then create a view that contains our form called **New.cshtml** in the **~/Views/Rentals/** directory.

We don't need to set the layout in our view because there is a default value for it stored in the **~/Views/ViewStart.cshtml** file. The contents of the **ViewStart.cshtml** file will be placed at the top of all views. We can see the Layout is set so we don't need to specify it in every view. To create a form we're not going to use **@Html.BeginForm()** because that would create a traditional Html form. Instead we're just going to use raw Html.

In a traditional Html form we set the action in the form tag. This action specifies the end point that this form will be posted to. When we use **@Html.BeginForm** and specify an action and a controller, this method will get the URL for that action and put it here. Like: **/controller/action**. But we are not going to use the action attribute. We have a helper method called **Ajax.BeginForm()**, but this helper method is designed to call actions in MVC controllers, not API controllers. If we want to call an action in a web API, the syntax gets really ugly and that's why we should use raw Html. Now let's go over a technique to quickly create markup called **Zencoding**.

In our form tag we want to have a **div** with the class **form-group**. Inside this div, we want to have a label. Next to this label we want to have an **input** with the attribute type set to **text**, and we want this input to have the class **form.control**. We write this as:

```
<form>
    div.form-group>label+input[type='text'].form-control
</form>
```

If we press tab after our Zencoded line, the Web Essentials Plugin will generate this markup for us based on our Zencode:

```cs
<form>
    <div class="form-group">
        <label></label>
        <input type="text" value="" class="frm-control" />
    </div>
</form>
```

We will set our label to **Customer**. We will create a duplicate of the **div** for our **Movie**. Then we will create a submit button with two classes: **button.btn.btn-primary**.

```cs
<form>
    <div class="form-group">
        <label>Customer</label>
        <input type="text" value="" class="frm-control" />
    </div>

    <div class="form-group">
        <label>Movie</label>
        <input type="text" value="" class="frm-control" />
    </div>

    <button class="btn btn-primary">Submit</button>
</form>
```

Lastly, let's put a link to this form on our navigation bar. We go to the **~/Views/Shared/NavBar.cshtml** file to do this. We create a new action link for our new navigation bar option:

```
<div class="navbar navbar-inverse navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            @Html.ActionLink("Vidly", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
        </div>
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li>@Html.ActionLink("New Rental", "New", "Rentals")</li>
                <li>@Html.ActionLink("Customers", "Index", "Customers")</li>
                <li>@Html.ActionLink("Movies", "Index", "Movies")</li>
            </ul>
            @Html.Partial("_LoginPartial")
        </div>
    </div>
</div>
```
