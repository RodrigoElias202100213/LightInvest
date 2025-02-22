using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInvest.Data;
using LightInvest.Models;
using LightInvest.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LightInvestTests
{
    public class AccountControllerTest
    {

        [Fact]
        public void Login_ReturnsView()
        {
            var ac = new AccountController(null);
            var result = ac.Login();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Register_ReturnsViewWithRegisterViewModel()
        {
            var ac = new AccountController(null);
            var result = ac.Register();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RegisterViewModel>(viewResult.Model);
        }
    }
}
