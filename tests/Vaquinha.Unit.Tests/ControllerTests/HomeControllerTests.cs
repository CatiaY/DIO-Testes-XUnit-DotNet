using Microsoft.Extensions.Logging;
using Moq;
using Vaquinha.MVC.Controllers;
using Vaquinha.Domain;
using Xunit;
using System.Threading.Tasks;
using Vaquinha.Service;
using NToastNotify;
using System.Collections.Generic;
using Vaquinha.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using AutoMapper;

namespace Vaquinha.Unit.Tests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly IHomeInfoService _homeInfoService;
        private readonly IDoacaoService _doacaoService;

        private readonly Mock<IHomeInfoRepository> _homeInfoRepository = new Mock<IHomeInfoRepository>();
        private readonly Mock<IDoacaoRepository> _doacaoRepository = new Mock<IDoacaoRepository>();
        private readonly Mock<ICausaRepository> _causaRepository = new Mock<ICausaRepository>();

        private readonly Mock<ILogger<HomeController>> _logger = new Mock<ILogger<HomeController>>();
        private IDomainNotificationService _domainNotificationService = new DomainNotificationService();
        private readonly Mock<IToastNotification> _toastNotification = new Mock<IToastNotification>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<GloballAppConfig> _globalSettings = new Mock<GloballAppConfig>();

        private readonly HomeViewModel _HomeViewModel;

        public HomeControllerTests()
        {
            _doacaoService = new DoacaoService(_mapper.Object, _doacaoRepository.Object, _domainNotificationService);
            _homeInfoService = new HomeInfoService(_mapper.Object, _doacaoService,
                                _globalSettings.Object, _homeInfoRepository.Object, _causaRepository.Object);
            _HomeViewModel = ObterDadosParaTeste();
        }

        [Trait("HomeController", "HomeController_IndexRetornaViewResultComDadosIniciais")]
        [Fact]
        public async Task HomeController_IndexRetornaViewResult_ComDadosIniciais()
        {
            // Arrange            
            _homeInfoRepository.Setup(repo => repo.RecuperarDadosIniciaisHomeAsync())
                .ReturnsAsync(_HomeViewModel);
            
            var homeController = new HomeController(_logger.Object, _homeInfoService, _toastNotification.Object);            

            // Act: Executa o método Index
            var retorno = await homeController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(retorno);
            viewResult.Model.Should().BeOfType<HomeViewModel>();
            ((HomeViewModel)viewResult.Model).Should().Be(_HomeViewModel);
        }

        private HomeViewModel ObterDadosParaTeste()
        {
            return new HomeViewModel
            {
                ValorTotalArrecadado = 0,
                QuantidadeDoadores = 0,
                ValorRestanteMeta = 30000,
                PorcentagemTotalArrecadado = 0,
                TempoRestanteDias = 164,
                TempoRestanteHoras = 21,
                TempoRestanteMinutos = 57,
                Doadores = new List<DoadorViewModel>() { },
                Instituicoes = new List<CausaViewModel>() { }
            };
        }
    }
}
