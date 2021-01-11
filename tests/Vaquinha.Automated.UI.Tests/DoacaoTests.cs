using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>, 
                                               IClassFixture<EnderecoFixture>, 
                                               IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because:"logo exibido");
		}
		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/Doacoes/Create/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement campoValor = null;
			campoValor = _driver.FindElement(By.Id("valor"));
			campoValor.SendKeys(doacao.Valor.ToString());

			IWebElement campoNome = null;
			campoNome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
			campoNome.SendKeys(doacao.DadosPessoais.Nome);

			IWebElement campoEmail = null;
			campoEmail = _driver.FindElement(By.Id("DadosPessoais_Email"));
			campoEmail.SendKeys(doacao.DadosPessoais.Email);

			IWebElement campoMensagem = null;
			campoMensagem = _driver.FindElement(By.Id("DadosPessoais_MensagemApoio"));
			campoMensagem.SendKeys(doacao.DadosPessoais.MensagemApoio);

			IWebElement campoEndereço = null;
			campoEndereço = _driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco"));
			campoEndereço.SendKeys(doacao.EnderecoCobranca.TextoEndereco);

			IWebElement campoNumeroRua = null;
			campoNumeroRua = _driver.FindElement(By.Id("EnderecoCobranca_Numero"));
			campoNumeroRua.SendKeys(doacao.EnderecoCobranca.Numero);

			IWebElement campoCidade = null;
			campoCidade = _driver.FindElement(By.Id("EnderecoCobranca_Cidade"));
			campoCidade.SendKeys(doacao.EnderecoCobranca.Cidade);

			IWebElement campoEstado = null;
			campoEstado = _driver.FindElement(By.Id("estado"));
			campoEstado.SendKeys(doacao.EnderecoCobranca.Estado);

			IWebElement campoCEP = null;
			campoCEP = _driver.FindElement(By.Id("cep"));
			campoCEP.SendKeys(doacao.EnderecoCobranca.CEP);

			IWebElement campoComplemento = null;
			campoCEP = _driver.FindElement(By.Id("EnderecoCobranca_Complemento"));
			campoCEP.SendKeys(doacao.EnderecoCobranca.Complemento);

			IWebElement campoTelefone = null;
			campoTelefone = _driver.FindElement(By.Id("telefone"));
			campoTelefone.SendKeys(doacao.EnderecoCobranca.Telefone);

			IWebElement campoTitular = null;
			campoTitular = _driver.FindElement(By.Id("FormaPagamento_NomeTitular"));
			campoTitular.SendKeys(doacao.FormaPagamento.NomeTitular);

			IWebElement campoNumeroCartao = null;
			campoNumeroCartao = _driver.FindElement(By.Id("cardNumber"));
			campoNumeroCartao.SendKeys(doacao.FormaPagamento.NumeroCartaoCredito);

			IWebElement campoValidade = null;
			campoValidade = _driver.FindElement(By.Id("validade"));
			campoValidade.SendKeys(doacao.FormaPagamento.Validade);

			IWebElement campoCVV = null;
			campoCVV = _driver.FindElement(By.Id("cvv"));
			campoCVV.SendKeys(doacao.FormaPagamento.CVV);

			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			//Assert
			_driver.Url.Should().Contain("/Doacoes/Create");
		}
	}
}