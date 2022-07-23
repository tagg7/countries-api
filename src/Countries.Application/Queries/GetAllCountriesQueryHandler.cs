using AutoMapper;
using Countries.Application.Dtos;
using Countries.Application.Interfaces;
using MediatR;

namespace Countries.Application.Queries;

public record GetAllCountriesQuery() : IRequest<IList<CountryDto>>;

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IList<CountryDto>>
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public GetAllCountriesQueryHandler(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }
    
    public async Task<IList<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _countryService.GetAll(cancellationToken);
        var countryDtos = _mapper.Map<IList<CountryDto>>(countries);

        return countryDtos;
    }
}