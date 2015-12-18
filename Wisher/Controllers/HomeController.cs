using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wisher.Data;
using Wisher.HotlineManagment;
using Wisher.UserManagment.Repository;

namespace Wisher.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestFaceBook()
        {
            return View();
        }
    }
}