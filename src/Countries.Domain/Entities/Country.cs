using Countries.Domain.Common;

namespace Countries.Domain.Entities;

public class Country : EntityBase
{
    public string Name { get; set; }
    
    public int Population { get; set; }
    
    public float GdpInBillions { get; set; }
}