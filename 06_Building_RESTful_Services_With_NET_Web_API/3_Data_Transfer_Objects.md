# Data Transfer Objects

Our API that we created returns Customer Objects - Which is a problem if we ever want to change the Customer model

To solve this issue we need a different model, so we can create a **Data Transfer Object (DTO)** 

The DTO is a plain data structure that is used to transfer data from the client to the server, or vice versa

By creating DTO's we reduce the chances of our API breaking as we refactor our domain models - like the Customer.cs model

**Your API's should never receive or return domain objects**

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

***

When we built our API, we gave access to our User objects to anyone

This is dangerous because anyone can update the information in the User object

***

To solve this problem we can Use a DTO to exclude the properties of our model that can be updated

This is the code for our Customer model:

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

The DTO for this model would look this this:

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

Notice that we did not include the MembershipType property here. That is because - since the membershipType has its own model - it gets its own DTO, but that is not needed for this situation.

Also notice that we don't include and of the Display name data annotations because they are already defined in the Customer model, and are not needed here

***

### Auto Mapper

In the Customer controller, anywhere we return a customer object we need to map it to our DTO

We can do this using the **Auto Mapper**

To install automapper simply run this command through the NuGet Package Manager: Install-Package automapper -version:4.1

***

To do this we need to add a MappingProfile.cs class to the App_Start directory:

```cs
namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Dto
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<CustomerDto, Customer>();

            // Dto to Domain
            Mapper.CreateMap<CustomerDto, Customer>().ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<MovieDto, Movie>().ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}
```

***

We need to load our Mappers when the application is started, so go to Global.asax.cs in the root directory for the project and Initialize the MappingProfile.cs class we created above:

```cs
namespace Vidly
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
```
