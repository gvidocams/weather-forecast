namespace Infrastructure.Utilities;

public interface IDateTimeWrapper
{
    DateTime UtcNow { get; }
}