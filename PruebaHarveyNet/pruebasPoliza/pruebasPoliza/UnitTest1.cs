
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PruebaHarveyNet.Controllers;
using PruebaHarveyNet.Models;
using PruebaHarveyNet.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace pruebasPoliza.Tests
{
    public class PolizaControllerTests
    {
        private readonly string secretKey;
        
        [Fact]
        public async Task Get_ReturnsListOfPolizas()
        {
            // Arrange
            var mockService = new Mock<MongoDBService>();
            var config = new Mock<IConfiguration>();
            config.Setup(c => c.GetSection("settings").GetSection("secretkey").ToString()).Returns("your_secret_key_here");

            mockService.Setup(service => service.GetAsync()).ReturnsAsync(new List<Poliza>());
            var controller = new PolizaController(mockService.Object, config.Object);


            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Poliza>>>(result);
            var viewResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<List<Poliza>>(viewResult.Value);

        }

        [Fact]
        public async Task Post_AñadeUnaNuevaPoliza_CuandoElInicioDeSesionEsValido()
        {
            // Arrange
            var poliza = new Poliza
            {
                NumberPoliza = "123456",
                NameClient = "Juan Pérez",
                Identification = 123456789,
                Birthdate = new DateTime(2023, 8, 27),
                DateCreatePoliza = new DateTime(2023, 8, 27),
                DateEndPoliza = new DateTime(2023, 8, 31),
                Coverage = "Cobertura básica",
                MaximumValuePoliza = 100000,
                PolizaPlanName = "Plan de auto",
                CityClient = "Guadalajara",
                AddressClient = "Calle 123, Col. Centro",
                LicensePlate = "ABC123",
                ModelVehicle = "Volkswagen Jetta",
                InspectionVehicle = true
            };

            var mongoDBServiceMock = new Mock<MongoDBService>();
            mongoDBServiceMock.Setup(s => s.CreateAsync(It.IsAny<Poliza>())).Returns(Task.CompletedTask);

            var config = new Mock<IConfiguration>();
            config.Setup(c => c.GetSection("settings").GetSection("secretkey").ToString()).Returns("your_secret_key_here");

            // Act
            var controller = new PolizaController(mongoDBServiceMock.Object, config.Object);
            var response = await controller.Post(poliza) as CreatedAtActionResult;

            // Assert
            Assert.IsType<CreatedAtActionResult>(response);
            Assert.NotNull(response);
            Assert.IsType<Poliza>(response.Value);
           
        }

        [Fact]
        public void BuscarPoliza_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<MongoDBService>();
            var config = new Mock<IConfiguration>();
            config.Setup(c => c.GetSection("settings").GetSection("secretkey").ToString()).Returns("your_secret_key_here");

            var controller = new PolizaController(mockService.Object, config.Object);

            var placaVehiculo = "ABC123";
            var numeroPoliza = "123";

            var mockPoliza = new Poliza
            {
                NumberPoliza = "123456",
                NameClient = "Juan Pérez",
                Identification = 123456789,
                Birthdate = new DateTime(2023, 8, 27),
                DateCreatePoliza = new DateTime(2023, 8, 27),
                DateEndPoliza = new DateTime(2023, 8, 31),
                Coverage = "Cobertura básica",
                MaximumValuePoliza = 100000,
                PolizaPlanName = "Plan de auto",
                CityClient = "Guadalajara",
                AddressClient = "Calle 123, Col. Centro",
                LicensePlate = "ABC123",
                ModelVehicle = "Volkswagen Jetta",
                InspectionVehicle = true
            };

            mockService.Setup(service => service.BuscarPolizaPorPlacaYNumero(placaVehiculo, numeroPoliza))
                .Returns(mockPoliza);

            // Act
            var result = controller.BuscarPoliza(placaVehiculo, numeroPoliza);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPoliza = Assert.IsType<Poliza>(okResult.Value);

        }
    }

}
