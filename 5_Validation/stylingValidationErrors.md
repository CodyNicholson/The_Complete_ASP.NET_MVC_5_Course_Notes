# Styling Validation Errors

When a field/input in your application becomes invalid, ASP.NET will automatically give that field/input the class "field-validation-error"/"input-validation-error"

Therefore, you can style the invald fields and inputs anyway you like using your applications site.css file, or another .css file you define and include yourself

```css
.field-validation-error {
    color: red;
}

.input-validation-error {
    border: 2px solid red;
}
```
