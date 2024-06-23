using Claims.Domain.Shared;

namespace Claims.Domain.Covers;
public static class DomainErrors
{
    public static class Covers
    {
        public static readonly Error StartDateInPast = new(
            "Cover.StartDate",
            "Cover's start date cannot be in the past");

        public static readonly Error StartDateLaterThanEndDate = new(
            "Cover.StartDate",
            "Cover's start date cannot be later than end date");

        public static readonly Error InsurancePeriodTooLong = new(
            "Cover.Creating.InsurancePeriod",
            "Insurance period cannot exceed 1 year");
    }
}
