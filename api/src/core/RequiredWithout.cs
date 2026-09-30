using System.ComponentModel.DataAnnotations;

namespace core;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredWithoutAttribute : ValidationAttribute
{
    private readonly string _otherProperty;

    public RequiredWithoutAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);
        if (otherPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property : {_otherProperty}");
        }

        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

        bool isOtherPropertyEmpty = otherPropertyValue == null || string.IsNullOrWhiteSpace(otherPropertyValue.ToString());
        bool isCurrentPropertyEmpty = value == null || string.IsNullOrWhiteSpace(value.ToString());

        if (isOtherPropertyEmpty && isCurrentPropertyEmpty)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Field {validationContext.DisplayName} is mandatory when {_otherProperty} is not given."
            );
        }

        return ValidationResult.Success;
    }
}
