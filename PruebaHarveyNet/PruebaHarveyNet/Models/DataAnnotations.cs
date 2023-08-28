using System;
using System.ComponentModel.DataAnnotations;

public class DateNotInPastAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime date = (DateTime)value;
        return date.Date >= DateTime.Now.Date;
    }
}

