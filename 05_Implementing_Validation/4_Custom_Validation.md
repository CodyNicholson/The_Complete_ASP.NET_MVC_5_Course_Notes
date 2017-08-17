# Custom Validation

To implement **Custom Validation** we have to create a new class, in this case: Min18YearsIfAMember

```
namespace Vidly.Models
{
    public class Min18YearsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer) validationContext.ObjectInstance;

            if(customer.MembershipTypeId == 1)
                return ValidationResult.Success;

            if(customer.Birthdate == null)
                return new ValidationResult("Date of Birth is required");

            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;

            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("Customer should be at least 18 years old to go on a membership");
        }
    }
}
```

This class gets the customer, checks the birthdate, and acts based on the value put into the birthdate input

-

We include this Custom Validation in our Birthdate input by using a Data Annotation:

```cs
[Display(Name = "Date of Birth")]
[Min18YearsIfAMember]
public DateTime? Birthdate { get; set; }
```
