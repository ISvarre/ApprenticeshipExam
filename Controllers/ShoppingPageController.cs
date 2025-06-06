using ApprenticeshipExam.Models.Pages;
using ApprenticeshipExam.Models.ViewModels;
using EPiServer.Web;
using Microsoft.AspNetCore.Mvc;

namespace ApprenticeshipExam.Controllers
{
    public class ShoppingPageController : PageControllerBase<ShoppingPage>
    {
        public ViewResult Index(ShoppingPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            return View("ShoppingPage", model);
        }
    }
}    

