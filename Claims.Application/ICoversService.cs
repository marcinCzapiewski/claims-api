using Claims.Domain.Models;

namespace Claims.Application;
public interface ICoversService
{
    decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
}
