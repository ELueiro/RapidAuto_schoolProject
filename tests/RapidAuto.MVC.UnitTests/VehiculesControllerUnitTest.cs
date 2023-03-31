using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RapidAuto.MVC.Controllers;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RapidAuto.MVC.UnitTests
{
    public class VehiculesControllerUnitTest
    {
        private readonly Mock<ILogger<VehiculesController>> _logger;
        private readonly Mock<IVehiculeMVCService> _mockVehiculeService;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<IFichierMVCService> _mockFichierService;
        private readonly Fixture _fixture;


        //private readonly Mock<IFormFile> file1;
        //private readonly Mock<IFormFile> file2;

        public VehiculesControllerUnitTest()
        {

            _mockVehiculeService = new Mock<IVehiculeMVCService>();
            _config = new Mock<IConfiguration>();
            _mockFichierService = new Mock<IFichierMVCService>();
            _logger = new Mock<ILogger<VehiculesController>>();
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());


        }

        [Fact]
        public async Task Create_Retourne_ViewResult()
        {
            //Etant donné
            _mockVehiculeService.Setup(e => e.Ajouter(It.IsAny<Vehicule>()));

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("Hybrid");
            _config.Setup(x => x.GetSection(It.Is<string>(k => k == "TypeDeVehicule"))).Returns(mockSection.Object);

            var vehiculeController = new VehiculesController(_mockVehiculeService.Object, _config.Object, _mockFichierService.Object, _logger.Object);



            //Quand
            var viewResult = vehiculeController.Create() as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().BeNull();
            vehiculeController.ViewData.Should().NotBeNull();

        }
        [Fact]
        public async Task Create_UneDeDeuxImagesEtNull_Retourne_ViewResult()
        {
            //Etant donné
            var vehicule = _fixture.Create<Vehicule>();

            IFormFile image1 = new FormFile(new MemoryStream(), 0, 5, "Data", "dummy.jpg");
            IFormFile image2 = new FormFile(new MemoryStream(), 0, 5, "Data", "dummy.jpg");
            image2 = null;
            var vehiculeController = new VehiculesController(_mockVehiculeService.Object, _config.Object, _mockFichierService.Object, _logger.Object);

            //Quand
            var viewResult = await vehiculeController.Create(vehicule, image1, image2) as ViewResult;
            //Alors
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().BeNull();
            vehiculeController.ViewData.Should().NotBeNull();

        }
        [Fact]
        public async Task Create_UserValid_Retourne_RedirectToAction()
        {
            //Etant donné
            var vehicule = _fixture.Create<Vehicule>();
            IFormFile image1 = new FormFile(new MemoryStream(), 0, 5, "Data", "dummy.jpg");
            IFormFile image2 = new FormFile(new MemoryStream(), 0, 5, "Data", "dummy.jpg");

            //Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            //mockSection.Setup(x => x.Value).Returns("Hybrid");
            //_config.Setup(x => x.GetSection(It.Is<string>(k => k == "TypeDeVehicule"))).Returns(mockSection.Object);

            Fichier fichier = new Fichier();
            fichier.image1 = image1;
            fichier.image2 = image2;
            fichier.codeVehicule = vehicule.Niv;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _mockFichierService.Setup(e => e.Ajouter(It.IsAny<Fichier>())).Returns(() => Task.FromResult<HttpResponseMessage>(responseMessage));
            





            var vehiculeController = new VehiculesController(_mockVehiculeService.Object, _config.Object, _mockFichierService.Object, _logger.Object);



            //Quand
            var redirectToActionResult = await vehiculeController.Create(vehicule, image1, image2) as RedirectToActionResult;



            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
            _mockVehiculeService.Verify(e => e.Ajouter(It.IsAny<Vehicule>()));
        }



        [Fact]
        public async Task Create_VehiculeEstInvalide_Retourne_ViewResult()
        {
            //Etant donné
            var vehicule = _fixture.Create<Vehicule>();
            _mockVehiculeService.Setup(e => e.Ajouter(It.IsAny<Vehicule>()));



            IFormFile image1 = new FormFile(new MemoryStream(), 0, 5, "Data", "dummy.jpg");
            IFormFile image2 = new FormFile(new MemoryStream(), 0, 5, "Data", "dummy.jpg");



            var vehiculeController = new VehiculesController(_mockVehiculeService.Object, _config.Object, _mockFichierService.Object, _logger.Object);
            vehiculeController.ModelState.AddModelError("Nom", "Le champ est obligatoire");



            //Quand
            var viewResult = await vehiculeController.Create(vehicule, image1, image2) as ViewResult;



            //Alors
            viewResult.Should().NotBeNull();
            _mockVehiculeService.Verify(e => e.Ajouter(It.IsAny<Vehicule>()), Times.Never);
            var vehiculeResult = viewResult.Model as Vehicule;
            vehiculeResult.Should().Be(vehicule);
        }


    }
}

