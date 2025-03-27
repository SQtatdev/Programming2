using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace KooliProjekt.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PagedResult<Match> model)
        {
            return View(model);
        }
    }
}