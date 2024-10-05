namespace SynFlood;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
sealed class BitfieldLengthAttribute(uint length) : Attribute
{
    public uint Length { get; } = length;
}
