using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RapidAuto.MVC.Controllers;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RapidAuto.MVC.UnitTests
{
    public class UtilisateursControllerUnitTest
    {
        private readonly Mock<IUtilisateurMVCService> _mockUtilisateurService;
        private readonly Mock<ILogger<UtilisateursController>> _mockLoggerService;
        private readonly Fixture _fixture;

        public UtilisateursControllerUnitTest()
        {
            _mockUtilisateurService = new Mock<IUtilisateurMVCService>();
            _mockLoggerService = new Mock<ILogger<UtilisateursController>>();
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Index_Retourne__ViewResult()
        {
            //Etant donné
            _fixture.RepeatCount = 5;
            var users = _fixture.Create<IEnumerable<Utilisateur>>().ToList();

            _mockUtilisateurService.Setup(e => e.ObtenirTout()).Returns(() => Task.FromResult(users));
            var usersController = new UtilisateursController(_mockUtilisateurService.Object,_mockLoggerService.Object);

            //Quand
            var viewResult = await usersController.Index() as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var usersResult = viewResult.Model as IEnumerable<Utilisateur>;
            usersResult.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task Details_IdNull_Retourne_NotFound()
        {
            //Etant donné
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var actionResult = await usersController.Details( null) ;

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            _mockUtilisateurService.Verify(e => e.Obtenir(It.IsAny<int>()), Times.Never);
        }


        [Fact]
        public async Task Details_IdNonNull_Et_UserInexistant_Retourne_NotFound()
        {
            //Etant donné            
            _mockUtilisateurService.Setup(e => e.Obtenir(It.IsAny<int>())).Returns(() => Task.FromResult<Utilisateur>(null));
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            var utilisateur = _fixture.Create<Utilisateur>();
            //Quand
            var actionResult = await usersController.Details(utilisateur.Id);

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            _mockUtilisateurService.Verify(e => e.Obtenir(It.IsAny<int>()), Times.Once);
        }



        [Fact]
        public async Task Details_IdNonNull_Et_UserExistant_Retourne_ViewResult()
        {
            //Etant donné
            //Initialiser un User
            var utilisateur = _fixture.Create<Utilisateur>();

            //Initialiser une nouvelle instance de Mock en spécifiant l'interface du service à substituer
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            //Définir le résultat qui sere retourné lorque la fonction sera appelée
            _mockUtilisateurService.Setup(e => e.Obtenir(It.IsAny<int>())).Returns(() => Task.FromResult(utilisateur));

            

            //Quand
            var viewResult = await usersController.Details(utilisateur.Id) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var etudiantResult = viewResult.Model as Utilisateur;
            etudiantResult.Should().Be(utilisateur);
        }

        [Fact]
        public async Task Create_Retourne_ViewResult()
        {
            //Etant donné
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var viewResult = await usersController.Create() as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().BeNull();
            usersController.ViewData.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_UserValid_Retourne_RedirectToAction()
        {
            //Etant donné
            var user = _fixture.Create<Utilisateur>();
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Erreur 400")
            };
            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>())).Returns(() => Task.FromResult<HttpResponseMessage>(responseMessage));

            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var redirectToActionResult = await usersController.Create(user) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
            _mockUtilisateurService.Verify(e => e.Ajouter(It.IsAny<Utilisateur>()));
        }

        [Fact]
        public async Task Create_UserInvalide_Retourne_ViewResult()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>()));

            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            usersController.ModelState.AddModelError("Nom", "Le champ est obligatoire");

            //Quand
            var viewResult = await usersController.Create(utilisateur) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            _mockUtilisateurService.Verify(e => e.Ajouter(It.IsAny<Utilisateur>()), Times.Never);
            var userResult = viewResult.Model as Utilisateur;
            userResult.Should().Be(utilisateur);
        }


        [Fact]
        public async Task Edit_IdNull_Retourne_NotFound()
        {
            //Etant donné
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var actionResult = await usersController.Details(null);

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            _mockUtilisateurService.Verify(e => e.Obtenir(It.IsAny<int>()), Times.Never);
        }


        [Fact]
        public async Task Edit_IdNonNull_Et_UserInexistant_Retourne_NotFound()
        {
            //Etant donné            
            _mockUtilisateurService.Setup(e => e.Obtenir(It.IsAny<int>())).Returns(() => Task.FromResult<Utilisateur>(null));
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            var utilisateur = _fixture.Create<Utilisateur>();
            //Quand
            var actionResult = await usersController.Details(utilisateur.Id);

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            _mockUtilisateurService.Verify(e => e.Obtenir(It.IsAny<int>()), Times.Once);
        }



        [Fact]
        public async Task Edit_IdNonNull_Et_UserExistant_Retourne_ViewResult()
        {
            //Etant donné
            //Initialiser un User
            var utilisateur = _fixture.Create<Utilisateur>();

            //Initialiser une nouvelle instance de Mock en spécifiant l'interface du service à substituer
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            //Définir le résultat qui sere retourné lorque la fonction sera appelée
            _mockUtilisateurService.Setup(e => e.Obtenir(It.IsAny<int>())).Returns(() => Task.FromResult(utilisateur));



            //Quand
            var viewResult = await usersController.Details(utilisateur.Id) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var userResult = viewResult.Model as Utilisateur;
            userResult.Should().Be(utilisateur);
        }
        [Fact]
        public async Task Edit_IdEstPasEgualeIdUser_Retourne__NotFound()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var id = utilisateur.Id++;
            _mockUtilisateurService.Setup(e => e.Obtenir(It.IsAny<int>())).Returns(() => Task.FromResult(utilisateur));
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var actionResult = await usersController.Edit(id,utilisateur);

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            Assert.NotEqual(id, utilisateur.Id);
        }


        [Fact]
        public async Task Edit_UserValid_Retourne_RedirectToAction()
        {
            //Etant donné
            var user = _fixture.Create<Utilisateur>();
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Erreur 400")
            };
            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>())).Returns(() => Task.FromResult<HttpResponseMessage>(responseMessage));

            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var redirectToActionResult = await usersController.Create(user) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
            _mockUtilisateurService.Verify(e => e.Ajouter(It.IsAny<Utilisateur>()));
        }

        [Fact]
        public async Task Edit_UserInvalide_Retourne_ViewResult()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>()));

            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            usersController.ModelState.AddModelError("Nom", "Le champ est obligatoire");

            //Quand
            var viewResult = await usersController.Create(utilisateur) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            _mockUtilisateurService.Verify(e => e.Ajouter(It.IsAny<Utilisateur>()), Times.Never);
            var userResult = viewResult.Model as Utilisateur;
            userResult.Should().Be(utilisateur);
        }

        [Fact]
        public async Task Supprimer_IdNull_Retourne_NotFound()
        {
            //Etant donné
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            //Quand
            var actionResult = await usersController.Supprimer(null);

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            _mockUtilisateurService.Verify(e => e.Supprimer(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Supprimer_IdNonNull_Et_UserInexistant_Retourne_NotFound()
        {
            //Etant donné            
            _mockUtilisateurService.Setup(e => e.Obtenir(It.IsAny<int>())).Returns(() => Task.FromResult<Utilisateur>(null));
            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);
            var utilisateur = _fixture.Create<Utilisateur>();
            //Quand
            var actionResult = await usersController.Details(utilisateur.Id);

            //Alors
            actionResult.Should().BeOfType(typeof(NotFoundResult));
            _mockUtilisateurService.Verify(e => e.Obtenir(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Supprimer_IdNonNull_Et_UserExistant_Retourne_ViewResult()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>()));

            var usersController = new UtilisateursController(_mockUtilisateurService.Object, _mockLoggerService.Object);

            var redirectToActionResult = await usersController.Supprimer(utilisateur.Id) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
            _mockUtilisateurService.Verify(e => e.Supprimer(It.IsAny<int>()));
        }
    }
}