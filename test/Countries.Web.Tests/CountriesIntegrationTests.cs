using System.Text;
using AutoFixture;
using Countries.Application.Dtos;
using Countries.Application.Interfaces;
using Countries.Domain.Entities;
using Countries.Web.Tests.Setup;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using Xunit;

namespace Countries.Web.Tests;

[Collection("IntegrationTestsCollection")]
public class CountriesIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly Fixture _fixture;
    private readonly TestWebApplicationFactory _factory;

    public CountriesIntegrationTests(TestWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _factory = factory;
    }
    
    [Fact]
    public async Task GetId_Country_Exists_And_Dto_Is_Correct()
    {
        // Arrange
        var country = _fixture
            .Build<Country>()
            .Without(c => c.Id)
            .Create();
        var repo = _factory.GetService<IDbRepository<Country>>();
        country = await repo.Insert(country);
        
        // Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"country/{country.Id}");
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var countryDto = JsonConvert.DeserializeObject<CountryDto>(await response.Content.ReadAsStringAsync());
        countryDto.Should().NotBeNull();
        
        using var scope = new AssertionScope();
        countryDto.Id.Should().Be(country.Id);
        countryDto.Name.Should().Be(country.Name);
        countryDto.Population.Should().Be(country.Population);
        countryDto.GdpInBillions.Should().Be(country.GdpInBillions);
    }
    
    [Fact]
    public async Task GetAll_Multiple_Countries_Exist_And_Count_Is_Correct()
    {
        // Arrange
        var countries = _fixture
            .Build<Country>()
            .Without(c => c.Id)
            .CreateMany();
        var repo = _factory.GetService<IDbRepository<Country>>();
        await repo.InsertMany(countries);
        
        // Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"country/all");
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var countryDtos = JsonConvert.DeserializeObject<IList<CountryDto>>(await response.Content.ReadAsStringAsync());
        countryDtos.Should().NotBeNull();
        countryDtos.Should().HaveCount(countries.Count());
    }
    
    [Fact]
    public async Task Update_Country_Properties_Updated()
    {
        // Arrange
        var country = _fixture
            .Build<Country>()
            .Without(c => c.Id)
            .Create();
        var repo = _factory.GetService<IDbRepository<Country>>();
        country = await repo.Insert(country);
        
        var updatedCountryDto = _fixture
            .Build<CountryDto>()
            .With(c => c.Id, country.Id)
            .Create();
        
        // Act
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(updatedCountryDto), Encoding.UTF8, "application/json");
        await client.PutAsync($"country/{country.Id}", content);
        
        // Assert
        var response = await client.GetAsync($"country/{country.Id}");
        var countryDto = JsonConvert.DeserializeObject<CountryDto>(await response.Content.ReadAsStringAsync());
        countryDto.Should().NotBeNull();
        
        using var scope = new AssertionScope();
        countryDto.Id.Should().Be(updatedCountryDto.Id);
        countryDto.Name.Should().Be(updatedCountryDto.Name);
        countryDto.Population.Should().Be(updatedCountryDto.Population);
        countryDto.GdpInBillions.Should().Be(updatedCountryDto.GdpInBillions);
    }
}