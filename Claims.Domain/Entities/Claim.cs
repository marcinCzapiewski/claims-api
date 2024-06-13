namespace Claims.Domain.Entities;
public class Claim
{
    public string Id { get; set; }
    public Cover Cover { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public ClaimType Type { get; set; }
    public decimal DamageCost { get; set; }
}
