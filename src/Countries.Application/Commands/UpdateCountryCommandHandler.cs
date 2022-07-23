using AutoMapper;
using Countries.Application.Dtos;
using Countries.Application.Interfaces;
using Countries.Domain.Entities;
using MediatR;

namespace Countries.Application.Commands;

public record UpdateCountryCommand(CountryDto Country) : IRequest<CountryDto>;

public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDto>
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public UpdateCountryCommandHandler(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }
    
    public async Task<CountryDto> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var country = _mapper.Map<Country>(request.Country);
        country = await _countryService.Update(country, cancellationToken);
        var countryDto = _mapper.Map<CountryDto>(country);
        return countryDto;
    }
}