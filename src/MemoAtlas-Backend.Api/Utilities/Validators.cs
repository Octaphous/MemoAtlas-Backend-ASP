using MemoAtlas_Backend.Api.Exceptions;

namespace MemoAtlas_Backend.Api.Utilities;

public static class Validators
{
    public static bool ValidateOptionalDateSpan(DateOnly? startDate, DateOnly? endDate, int maxDaySpan = 0)
    {
        if (startDate != null && endDate != null)
        {
            if (startDate > endDate)
            {
                throw new InvalidPayloadException("Start date cannot be later than end date.");
            }

            int daySpan = endDate.Value.DayNumber - startDate.Value.DayNumber;

            if (daySpan > maxDaySpan && maxDaySpan > 0)
            {
                throw new InvalidPayloadException($"Date range cannot exceed {maxDaySpan} days.");
            }
        }

        return true;
    }
}