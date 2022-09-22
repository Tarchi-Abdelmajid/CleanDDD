using BuberDinner.Application.Common.Services;

namespace BuberDinner.Infrastructure.Services;

public class DateTimeProvider : IDateTimePovider
{
    public DateTime UtcNow => DateTime.Now;
}