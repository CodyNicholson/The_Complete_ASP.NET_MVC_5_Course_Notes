# Data Annotations

So far we have looked at the Data Annotations [Required] and [StringLength(255)], but here is a full list of all of the Data Annotations:

- [Required]
- [StringLength(255)]
- [Range(1,10)]
- [Compare("OtherProperty")]
- [Phone]
- [EmailAddress]
- [URL]
- [RegularExpression("...")]

-

All of these Data Annotations each have their own specific validation message, but we can overwrite those messages with our own unique message in the model like this:

```cs
[Required(ErrorMessage = "Please enter a valid Customer Name")]
[StringLength(255)]
public string Name { get; set; }
```
