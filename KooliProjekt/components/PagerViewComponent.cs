using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Models;

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