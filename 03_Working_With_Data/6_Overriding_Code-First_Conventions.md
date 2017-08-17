# Overriding Code-First Conventions

Entity Framework does a lot of work for us, but sometimes we want to overwrite the conventions that it uses. We can do this using Data Annotations.

```cs
namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required] // Name is not nullable anymore, it is now required
        [StringLength(255)] // Name is not unlimited characters anymore, it is 255
        public string Name { get; set; }
        public bool Super { get; set; }
        public MembershipType MembershipType { get; set; } //Navigation Property
        public byte MembershipTypeId { get; set; }
    }
}
```

Normally the Name field would be able to contain an unlimited amount of characters, and also is not required to even have a value. Now that we have the [Required] and [StringLength(255)] data annotations above it, it must have a value and that value must not exceed 255 characters.
