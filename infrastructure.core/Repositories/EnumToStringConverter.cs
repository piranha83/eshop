using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Core.Repositories;

///<inheritdoc/>
public class EnumToStringConverter<TEnum> : ValueConverter<TEnum, string>
where TEnum : struct, Enum
{
    public EnumToStringConverter() :
        base(v => v.ToString(), v => ParseString(v))
    { }

    ///<inheritdoc/>
    private static TEnum ParseString(string value)
    {
        if (Enum.TryParse(value, true, out TEnum result))
        {
            return result;
        }
        
        throw new ArgumentException($"Значение '{value}' некорректное для типа {typeof(TEnum).Name}.", nameof(value));
    }
}