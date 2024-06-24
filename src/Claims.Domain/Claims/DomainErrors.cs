using Claims.Domain.Shared;

namespace Claims.Domain.Claims;
public static class DomainErrors
{
    public static class Claims
    {
        public static readonly Error DamageCostTooHigh = new(
            "Claim.Creating.DamageCost",
            $"Claim damage cost cannot exceed maximum value of {Claim.MaximumDamageCost}");

        public static readonly Error OutsideCoverPeriod = new(
            "Claim.Creating",
            $"Claim cannot be created outside related cover period.");
    }
    
}
