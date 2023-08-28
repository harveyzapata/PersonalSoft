using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaHarveyNet.Models;
using PruebaHarveyNet.Services;
using System;

using System.Text;
using System.Threading.Tasks;
using PruebaHarveyNet.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;
using System.Net;


namespace PruebaHarveyNet.Controllers;


[Controller]
[Route("api/[controller]")]

public class PolizaController : Controller
{

    private readonly MongoDBService _mongoDBService;
    private readonly string secretKey;
    

    public PolizaController(MongoDBService mongoDBService, IConfiguration config)
    {
        _mongoDBService = mongoDBService;
        secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
    }

  

    [HttpPost]
    [Route("Valid")]
    public IActionResult Valid([FromBody] UserLogin request)
    {
        if(request.Mail == "admin@gmail.com" && request.Password == "123456")
        {
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Mail));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenCOnfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenCreado = tokenHandler.WriteToken(tokenCOnfig);

            return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
        }
        else
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
        }
    }



    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Poliza>>> Get()
    {
        var polizas = await _mongoDBService.GetAsync();
        return Ok(polizas);
        
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] Poliza poliza)
    {
        if (ModelState.IsValid)
        {
            Boolean isValid = poliza.ValidateDates(poliza.DateCreatePoliza, poliza.DateEndPoliza);
            if (!isValid)
            {
                return BadRequest("La fecha final debe ser mayor a la Fecha de creación");
            }
            await _mongoDBService.CreateAsync(poliza);
            return CreatedAtAction(nameof(Get), new { id = poliza.Id }, poliza);
        }
        return BadRequest(ModelState);
    }



    [HttpGet("buscar")]
    [Authorize]
    public IActionResult BuscarPoliza(string placaVehiculo, string numeroPoliza)
    {
        var poliza = _mongoDBService.BuscarPolizaPorPlacaYNumero(placaVehiculo, numeroPoliza);
        if (poliza == null)
        {
            return NotFound("No se encontró la póliza.");
        }
        var polizaDto = new Poliza
        {
            Id = poliza.Id,
            NumberPoliza = poliza.NumberPoliza,
            NameClient = poliza.NameClient,
            Identification = poliza.Identification,
            Birthdate = poliza.Birthdate,
            DateCreatePoliza = poliza.DateCreatePoliza,
            DateEndPoliza = poliza.DateEndPoliza,
            Coverage = poliza.Coverage,
            MaximumValuePoliza = poliza.MaximumValuePoliza,
            PolizaPlanName = poliza.PolizaPlanName,
            CityClient = poliza.CityClient,
            AddressClient = poliza.AddressClient,
            LicensePlate = poliza.LicensePlate,
            ModelVehicle = poliza.ModelVehicle,
            InspectionVehicle = poliza.InspectionVehicle
            
            // Asigna más propiedades al DTO según corresponda
        };
        return Ok(polizaDto);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        if(ObjectId.TryParse(id, out _))
        {
            await _mongoDBService.DeleteAsync(id);
            return Ok("Póliza eliminida correctamente");
            // will never enter here.
        }   
        return NotFound("No se encontró la póliza.");
    }
   
}

