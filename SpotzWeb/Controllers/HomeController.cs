using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SpotzWeb.Models;


namespace SpotzWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var vm = new List<SpotzDetailViewModel>();
            foreach (var spotz in _db.Spotzes.ToList())
            {
                vm.Add(new SpotzDetailViewModel
                    (
                    spotz
                    ));
            }
            //var allSpotz = _db.Spotzes.ToList();
            return View(vm);
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View(_db.Users.ToList());
        }

        // GET: Spotz/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spotz spotz = await _db.Spotzes.FindAsync(id);
            if (spotz == null)
            {
                return HttpNotFound();
            }

            var vm = new SpotzDetailViewModel(spotz);

            return View(vm);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AddComment(AddedComment addedComment)
        {

            if (addedComment == null)
            {
                return HttpNotFound();
            }

            var idGuid = Guid.Parse(addedComment.Id);

            var spotz = _db.Spotzes.Find(idGuid);

            var newComment = new Comment
            {
                Timestamp = DateTime.Now,
                User = _db.Users.Find(Guid.Parse(addedComment.UserId)),
                CommentId = Guid.NewGuid(),
                Text = addedComment.Comment,
                Spotz = spotz

            };
            spotz?.Comments.Add(newComment);

            return Json(new { status = "success", message = "customer created" });
        }

    }


}