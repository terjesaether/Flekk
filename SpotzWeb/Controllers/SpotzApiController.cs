using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using SpotzWeb.Models;
using Newtonsoft.Json;

namespace SpotzWeb.Controllers
{
    public class SpotzApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SpotzApi
        public IQueryable<Spotz> GetSpotzes()
        {
            return db.Spotzes;
        }

        // GET: api/SpotzApi/5
        [ResponseType(typeof(Spotz))]
        public async Task<IHttpActionResult> GetSpotz(Guid id)
        {
            Spotz spotz = await db.Spotzes.FindAsync(id);
            if (spotz == null)
            {
                return NotFound();
            }

            return Ok(spotz);
        }

        // PUT: api/SpotzApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSpotz(Guid id, Spotz spotz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != spotz.SpotzId)
            {
                return BadRequest();
            }

            db.Entry(spotz).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotzExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SpotzApi
        [ResponseType(typeof(Spotz))]
        public async Task<IHttpActionResult> PostSpotz(Spotz spotz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Spotzes.Add(spotz);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SpotzExists(spotz.SpotzId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = spotz.SpotzId }, spotz);
        }

        // DELETE: api/SpotzApi/5
        [ResponseType(typeof(Spotz))]
        public async Task<IHttpActionResult> DeleteSpotz(Guid id)
        {
            Spotz spotz = await db.Spotzes.FindAsync(id);
            if (spotz == null)
            {
                return NotFound();
            }

            db.Spotzes.Remove(spotz);
            await db.SaveChangesAsync();

            return Ok(spotz);
        }

        [System.Web.Http.Route("api/fileupload/{id}")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> FileUpload(string id)
        {
            Spotz spotz = await db.Spotzes.FindAsync(Guid.Parse(id));
            if (spotz == null)
            {
                return NotFound();
            }

            var request = HttpContext.Current.Request;
            var file = request.Files[0];
            var title = request.Form["title"];
            var filename = request.Form["filename"];
            var imageId = Guid.NewGuid();
            try
            {
                var newImage = new Image
                {
                    Data = ConvertToByteArray(file.InputStream),
                    Timestamp = DateTime.Now,
                    Filename = title,
                    ImageId = imageId,
                    ImageUrl = Url.Route("Images", new { id = imageId }),
                    Spotz = spotz
                };

                spotz.Images.Add(newImage);
                db.Entry(spotz).State = EntityState.Modified;

                db.Images.Add(newImage);
                db.SaveChanges();
                return Json(new { status = "complete", imgurl = newImage.ImageUrl });
                //return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { status = "error", message = e });


            }
        }

        private static byte[] ConvertToByteArray(Stream inputStream)
        {
            var stream = new MemoryStream();

            inputStream.CopyTo(stream);

            return stream.ToArray();
        }


        [System.Web.Http.Route("api/SpotzApi/AddComment/")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddComment(AddedComment addedComment)
        {
            if (addedComment == null)
            {
                return NotFound();
            }
            if (addedComment.Comment == null)
            {
                return Json(new { status = "error", message = "No commment added!" });
                //return await HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            var idGuid = Guid.Parse(addedComment.Id);

            Spotz spotz = await db.Spotzes.FindAsync(idGuid);

            var newComment = new Comment
            {
                Timestamp = DateTime.Now,
                User = db.Users.Find(addedComment.UserId),
                CommentId = Guid.NewGuid(),
                Text = addedComment.Comment,
                Spotz = spotz

            };
            try
            {
                spotz?.Comments.Add(newComment);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Error creating comment!" });
            }

            return Json(new { status = "success", message = "Comment created!", comment = addedComment.Comment, user = newComment.User.UserName, gravatarurl = newComment.User.GravatarUrl });
            //return Json(newComment);
        }

        [System.Web.Http.Route("api/UpdateDescriptionText/")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> UpdateDescriptionText(UpdateDescriptionText updateDescriptionText)
        {

            if (updateDescriptionText.Text == "")
            {
                return Json(new { status = "error", message = "No text updated!" });
            }

            var idGuid = Guid.Parse(updateDescriptionText.Id);

            var spotz = await db.Spotzes.FindAsync(idGuid);

            if (spotz != null)
            {
                spotz.Description = updateDescriptionText.Text;
                try
                {
                    db.Entry(spotz).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return Json(new { status = "error", message = "Error updating description!" });
                }
            }

            return Json(new { status = "success", message = "Description updated!", text = updateDescriptionText.Text });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpotzExists(Guid id)
        {
            return db.Spotzes.Count(e => e.SpotzId == id) > 0;
        }
    }
}