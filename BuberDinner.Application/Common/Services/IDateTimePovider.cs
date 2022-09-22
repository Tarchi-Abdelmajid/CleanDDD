namespace BuberDinner.Application.Common.Services;

public interface IDateTimePovider
{
    DateTime UtcNow { get; }
}