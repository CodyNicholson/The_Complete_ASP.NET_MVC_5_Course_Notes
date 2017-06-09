# Data Transfer Objects

Our API that we created returns Customer Objects - Which is a problem if we ever want to change the Customer model

To solve this issue we need a different model, so we can create a **Data Transfer Object (DTO)** 

The DTO is a plain data structure that is used to transfer data from the client to the server, or vice versa

By creating DTO's we reduce the chances of our API breaking as we refactor our domain models - like the Customer.cs model

**Your API's should never receive or return domain objects**

***

### Creating Our DTO For The Customer Model

Recall our Customer Model seen in the code below

Our DTO should have all of the properties that our model has so that we can pass the DTO through our API controller instead of our model - This means hackers will not be able to access the actual objects, they will instead be able to access the DTO object

```cs
namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a valid Customer Name")] // Name is not nullable anymore, it is now required
        [StringLength(255)] // Name is not unlimited characters anymore, it is 255
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public MembershipType MembershipType { get; set; } //Navigation Property

        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }

        [Display(Name = "Date of Birth")] // Will display in the view as "Date of Birth" instead of "Birthdate"
        [Min18YearsIfAMember]
        public DateTime? Birthdate { get; set; }

        public static readonly byte Unknown = 0;
        public static readonly byte PayAsYouGo = 1;
    }
}
```

The DTO for our Customer Model can be found below

Notice we removed the "[Display(Name = "")]" because the DTO will never need to display its' name

Also notice that we did not include the **MembershipType** property because MembershipType has its own model, and therefore should get its own DTO if needed - but it is not needed in this case

```cs
namespace Vidly.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a valid Customer Name")] // Name is not nullable anymore, it is now required
        [StringLength(255)] // Name is not unlimited characters anymore, it is 255
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public byte MembershipTypeId { get; set; }

        [Min18YearsIfAMember]
        public DateTime? Birthdate { get; set; }
    }
}
```
