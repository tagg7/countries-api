using AutoMapper;
using Countries.Application.Dtos;
using Countries.Application.Interfaces;
using MediatR;

namespace Countries.Application.Queries;

public record GetAllCountriesQuery() : IRequest<ICollection<CountryDto>>;

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, ICollection<CountryDto>>
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public GetAllCountriesQueryHandler(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }
    
    public async Task<ICollection<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _countryService.GetAll(cancellationToken);
        var countriesDtos = _mapper.Map<ICollection<CountryDto>>(countries);

        return countriesDtos;
    }
}