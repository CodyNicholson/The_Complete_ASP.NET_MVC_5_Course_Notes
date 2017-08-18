# Returning Hierarchical Data

Currently we don't get the **MembershipType** from our API, we only get the **MembershipTypeID**. To fix this we need to go to the **~/Controllers/Api/CustomerController.cs** file. In this file we need to eager load the customers with their membership type.

To do this we go to the top of the controller and add: **using System.Data.Entity**. Importing this library gives us access to the **Include()** method. We use it like this in the **GetCustomers()** method:

```cs
        public IHttpActionResult GetCustomers()
        {
            var customerDtos = _context.Customers
                .Include(c => c.MembershipType)
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }
```

Now we need to go to the **CustomerDto** to add the **MembershipType** property. We should not simply add the  **MembershipType** property as a field to the **CustomerDto** because this couples our DTO to our domain object **MembershipType**. We should avoid this by creating a new DTO called **MembershipTypeDTO**:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.DTOs
{
    public class MembershipTypeDto
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}
```

***

Now that we have the **MembershipTypeDto** we can add it to the **CustomersDto**:

```cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

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

        public MembershipTypeDto MembershipType { get; set; }

        //[Min18YearsIfAMember]
        public DateTime? Birthdate { get; set; }
    }
}
```

***

The last step is to setup our mapping profile to finish **Eager Loading** our customer data. We go to the **~/App_Start/MappingProfile.cs** file to teach Auto Mapper to map the **MembershipType** to the **MembershipTypeDto**:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Vidly.DTOs;
using Vidly.Models;

namespace Vidly
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Dto
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<CustomerDto, Customer>();
            Mapper.CreateMap<MembershipType, MembershipTypeDto>();

            // Dto to Domain
            Mapper.CreateMap<CustomerDto, Customer>().ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<MovieDto, Movie>().ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}
```

***

The resulting data will look like this:

```
<ArrayOfCustomerDto xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/Vidly.DTOs">
<CustomerDto>
<Birthdate>1996-07-28T00:00:00</Birthdate>
<Id>4</Id>
<IsSubscribedToNewsletter>true</IsSubscribedToNewsletter>
<MembershipType>
<Id>1</Id>
<Name>Pay as You Go</Name>
</MembershipType>
<MembershipTypeId>1</MembershipTypeId>
<Name>Cody</Name>
</CustomerDto>
<CustomerDto>
<Birthdate i:nil="true"/>
<Id>5</Id>
<IsSubscribedToNewsletter>true</IsSubscribedToNewsletter>
<MembershipType>
<Id>4</Id>
<Name>Annual</Name>
</MembershipType>
<MembershipTypeId>4</MembershipTypeId>
<Name>Tricia</Name>
</CustomerDto>
<CustomerDto>
<Birthdate i:nil="true"/>
<Id>6</Id>
<IsSubscribedToNewsletter>true</IsSubscribedToNewsletter>
<MembershipType>
<Id>3</Id>
<Name>Quarterly</Name>
</MembershipType>
<MembershipTypeId>3</MembershipTypeId>
<Name>Amber</Name>
</CustomerDto>
</ArrayOfCustomerDto>
```

***

Now we can go back to the Customers **Index.cshtml** page and set the **data** object to equal **"membershipType.name"**:

```js

```
