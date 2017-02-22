using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Server.Models;

namespace Server.Controllers
{
    public class GapFillingsController : ApiController
    {
        private GapModel db = new GapModel();

        // GET: api/GapFillings
        public IQueryable<GapFilling> GetGapFilling()
        {
            return db.GapFilling;
        }

        // GET: api/GapFillings/?subject=初中数学
        [ResponseType(typeof(GapFilling))]
        public IHttpActionResult GetSingleChoice(string subject)
        {
            var query = from _model in db.GapFilling
                        where _model.subject == subject
                        select _model;
            return Ok(query);
        }

        // GET: api/GapFillings/5
        [ResponseType(typeof(GapFilling))]
        public IHttpActionResult GetGapFilling(int id)
        {
            GapFilling gapFilling = db.GapFilling.Find(id);
            if (gapFilling == null)
            {
                return NotFound();
            }

            return Ok(gapFilling);
        }

        // PUT: api/GapFillings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGapFilling(int id, GapFilling gapFilling)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gapFilling.id)
            {
                return BadRequest();
            }

            db.Entry(gapFilling).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GapFillingExists(id))
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

        // POST: api/GapFillings
        [ResponseType(typeof(GapFilling))]
        public IHttpActionResult PostGapFilling(GapFilling gapFilling)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GapFilling.Add(gapFilling);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gapFilling.id }, gapFilling);
        }

        // DELETE: api/GapFillings/5
        [ResponseType(typeof(GapFilling))]
        public IHttpActionResult DeleteGapFilling(int id)
        {
            GapFilling gapFilling = db.GapFilling.Find(id);
            if (gapFilling == null)
            {
                return NotFound();
            }

            db.GapFilling.Remove(gapFilling);
            db.SaveChanges();

            return Ok(gapFilling);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GapFillingExists(int id)
        {
            return db.GapFilling.Count(e => e.id == id) > 0;
        }
    }
}