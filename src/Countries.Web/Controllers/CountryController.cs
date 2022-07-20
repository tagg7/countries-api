using Countries.Application.Commands;
using Countries.Application.Dtos;
using Countries.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Countries.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ControllerBase
{
    private readonly ISender _mediator;

    public CountryController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<CountryDto> Get(string id)
    {
        var query = new GetCountryQuery(id);
        var countryDto = await _mediator.Send(query);
        return countryDto;
    }
    
    [HttpGet("all")]
    public async Task<ICollection<CountryDto>> GetAll()
    {
        var query = new GetAllCountriesQuery();
        var countriesDtos = await _mediator.Send(query);
        return countriesDtos;
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> Post(string id, CountryDto country)
    {
        if (id != country.Id)
            return BadRequest();

        var command = new UpdateCountryCommand(country);
        await _mediator.Send(command);
        
        return NoContent();
    }
}