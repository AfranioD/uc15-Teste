using ChapterBET6.Controllers;
using ChapterBET6.Interfaces;
using ChapterBET6.Models;
using ChapterBET6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace TesteIntegracao
{
    public class LoginControllerTeste
    {
        [Fact]
        public void LoginController_Retornar_Usuario_Invalido()
        {
            //Arrange - Prepara??o
            var repositoryFake = new Mock<IUsuarioRepository>();

            repositoryFake.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            var controller = new LoginController(repositoryFake.Object);

            LoginViewModel dadosUsuario = new LoginViewModel();
            dadosUsuario.email = "batata@email.com";
            dadosUsuario.senha = "batata";


            //Act - A??o
            var resultado = controller.Login(dadosUsuario);


            //Assert - Verifica??o
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        [Fact]
        public void LoginController_Retornar_Token() 
        {
             //Arrange - Prepara??o
             Usuario usuarioRetornado = new Usuario();
             usuarioRetornado.Email = "email@email.com";
             usuarioRetornado.Senha = "1234";
             usuarioRetornado.Tipo = "0";
             usuarioRetornado.Id = 1;



            var repositoryFake = new Mock<IUsuarioRepository>();
            
            repositoryFake.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetornado);


             LoginViewModel dadosUsuario = new LoginViewModel();
             dadosUsuario.email = "batata@email.com";
             dadosUsuario.senha = "batata";

            var controller = new LoginController(repositoryFake.Object);

            string issuerValido = "chapter.webapi";

            //Act - A??o
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosUsuario);
            
            string tokenstring = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenJwt = jwtHandler.ReadJwtToken(tokenstring);


           //Assert - Verifica??o
           Assert.Equal(issuerValido, tokenJwt.Issuer);
        
        }
            
    }
}