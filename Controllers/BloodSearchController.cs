using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZipCodeRadius.Data;
using ZipCodeRadius.DTOs;
using ZipCodeRadius.Model;

namespace ZipCodeRadius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodSearchController : ControllerBase
    {
        private readonly IBloodTypesRepo _repository;
        private readonly IZipCodeRepo _zipRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public BloodSearchController(IBloodTypesRepo repository, IZipCodeRepo zipRepo, IMapper mapper, ILogger<BloodSearchController> logger)
        {
            _repository = repository;
            _zipRepo = zipRepo;
            _mapper = mapper;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into the BloodSearchController");
        }

        // GET: api/<BloodSearchController>
        [HttpGet]
        public ActionResult<bloodTypesDto> Get()
        {
            var bloodTypes = _repository.GetBloodTypes();
            if (bloodTypes == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<bloodTypesDto>>(bloodTypes));
        }

        // GET api/<BloodSearchController>/5
        [HttpGet("{bloodType}/{zipCode}")]
        public string Get(string bloodType, string zipCode)
        {
            var bloodInfo = _repository.GetBloodTypeInfo(bloodType);
            var zipCodeInfo = _zipRepo.GetZipCodeDataByZipCode(zipCode);

            return (bloodInfo.PopulationPct/100 * zipCodeInfo.population).ToString();
        }
    }
}
