# Changing The Model

When you want to change the model, say add more columns to your tables or remove some, you should take small steps. That is, change one thing at a time and update the database gradually so you are less likely to have errors.

We change the model by just modifying the fields we had in our existing classes, or adding whole new classes that we would like to make tables of:

```cs
namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Super { get; set; }
        public MembershipType MemType { get; set; } //Navigation Property
        public byte MembershipTypeID { get; set; }
    }
}
```

```cs
namespace Vidly.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }
        public short SignUpFee { get; set; }
        public byte DurationInMonths { get; set; }
        public byte DiscountRate { get; set; }
    }
}
```

In the above examples I added two new fields to the Customer class and created the MembershipType class as well. Since MembershipType is a field of the Customer class and DbContext already knows about the the Customer class, I do not need to add MembershipType to the IdentityModels.cs file. 

Now that the changes are made I can run the "add-migration {Migration Name}" with the Migration name "AddMembershipType". Once this is done I go look at my new migration C-Sharp file in the Migrations folder to check that it looks good. Finally I can run the "update-database" command.
