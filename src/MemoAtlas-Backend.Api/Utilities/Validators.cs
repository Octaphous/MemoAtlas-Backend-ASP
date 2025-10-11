using MemoAtlas_Backend.Api.Exceptions;

namespace MemoAtlas_Backend.Api.Utilities;

public static class Validators
{
    public static bool ValidateOptionalDateSpan(DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate != null && endDate != null && startDate > endDate)
        {
            throw new InvalidPayloadException("Start date cannot be later than end date.");
        }

        return true;
    }
}