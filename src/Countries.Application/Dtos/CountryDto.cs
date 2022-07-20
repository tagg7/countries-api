namespace Countries.Application.Dtos;

public class CountryDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public int Population { get; set; }
    
    public float GdpInBillions { get; set; }
}