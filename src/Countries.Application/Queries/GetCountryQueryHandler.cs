using AutoMapper;
using Countries.Application.Dtos;
using Countries.Application.Interfaces;
using MediatR;

namespace Countries.Application.Queries;

public record GetCountryQuery(string Id) : IRequest<CountryDto>;

public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, CountryDto>
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public GetCountryQueryHandler(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }
    
    public async Task<CountryDto> Handle(GetCountryQuery request, CancellationToken cancellationToken)
    {
        var country = await _countryService.Get(request.Id, cancellationToken);
        var countryDto = _mapper.Map<CountryDto>(country);

        return countryDto;
    }
}