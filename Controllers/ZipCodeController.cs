using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using ZipCodeRadius.Data;
using ZipCodeRadius.DTOs;
using ZipCodeRadius.Model;

namespace ZipCodeRadius.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ZipCodeController : ControllerBase
{
    private readonly IZipCodeRepo _repository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ZipCodeController(IZipCodeRepo repository, IMapper mapper, ILogger<ZipCodeController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _logger.LogDebug(1, "NLog injected into the ZipCodeController");
    }

    [HttpGet("{zipCode}", Name="GetZipCodeInfoByZipCode")]
    public ActionResult<zipCodeInfoDto> GetZipCodeInfoByZipCode(string zipCode)
    {
        _logger.LogInformation(" --> GetZipCodeInfoByZipCode for zipCode(" + zipCode + ")");

        var zipCodeInfo = _repository.GetZipCodeDataByZipCode(zipCode);

        if(zipCodeInfo == null)
            return NotFound();
        else
            return Ok(_mapper.Map<zipCodeInfoDto>(zipCodeInfo));
    }

    [HttpGet("{zipCode}/radius/{radius}", Name="GetZipCodeInfoWithinZipCode")]
    public ActionResult<zipCodeInfoDto> GetZipCodeInfoWithinZipCode(string zipCode, int radius)
    {
        _logger.LogDebug(" --> GetZipCodeInfoWithinZipCode for zipCode({zipCode})", zipCode);

        var zipCodeInfo = _repository.GetZipCodeInfoWithinZipCode(zipCode, radius);

        if(zipCodeInfo == null)
            return NotFound();
        else
            return Ok(_mapper.Map<IEnumerable<zipCodeInfoDto>>(zipCodeInfo));
    }

    [HttpGet("{zipCode}/population/{population}", Name="GetZipCodeInfoWithZipCodePopulation")]
    public ActionResult<zipCodeResultsDto> GetZipCodeInfoWithZipCodePopulation(string zipCode, int population)
    {
        _logger.LogDebug(" --> GetZipCodeInfoWithZipCodePopulation for zipCode(" + zipCode + ")");

        var zipCodeResults = _repository.GetZipCodeInfoWithZipCodePopulation(zipCode, population);

        if(zipCodeResults.Item3 == null)
            return NotFound();
        else
        {
            zipCodeResultsDto result = new zipCodeResultsDto();

            result.zipCode = zipCode;
            result.resultCount = zipCodeResults.Item3.Count();
            result.radius =  zipCodeResults.Item1;
            result.radiusPopulation = zipCodeResults.Item2;
            result.result = _mapper.Map<IEnumerable<ZipCodeData>, IEnumerable<zipCodeBasicInfoDto>> (zipCodeResults.Item3);
            
            return Ok(result);
        }            
    }
}
