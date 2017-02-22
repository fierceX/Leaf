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
    public class SingleChoicesController : ApiController
    {
        private SingleModel db = new SingleModel();

        // GET: api/SingleChoices
        public IQueryable<SingleChoice> GetSingleChoice()
        {
            return db.SingleChoice;
        }

        // GET: api/SingleChoices/?subject=初中数学
        [ResponseType(typeof(SingleChoice))]
        public IHttpActionResult GetSingleChoice(string subject)
        {
            var query = from _model in db.SingleChoice
                        where _model.subject == subject
                        select _model;
            return Ok(query);
        }

        // GET: api/SingleChoices/5
        [ResponseType(typeof(SingleChoice))]
        public IHttpActionResult GetSingleChoice(int id)
        {
            SingleChoice singleChoice = db.SingleChoice.Find(id);
            if (singleChoice == null)
            {
                return NotFound();
            }

            return Ok(singleChoice);
        }

        // PUT: api/SingleChoices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSingleChoice(int id, SingleChoice singleChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != singleChoice.id)
            {
                return BadRequest();
            }

            db.Entry(singleChoice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SingleChoiceExists(id))
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

        // POST: api/SingleChoices
        [ResponseType(typeof(SingleChoice))]
        public IHttpActionResult PostSingleChoice(SingleChoice singleChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SingleChoice.Add(singleChoice);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = singleChoice.id }, singleChoice);
        }

        // DELETE: api/SingleChoices/5
        [ResponseType(typeof(SingleChoice))]
        public IHttpActionResult DeleteSingleChoice(int id)
        {
            SingleChoice singleChoice = db.SingleChoice.Find(id);
            if (singleChoice == null)
            {
                return NotFound();
            }

            db.SingleChoice.Remove(singleChoice);
            db.SaveChanges();

            return Ok(singleChoice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SingleChoiceExists(int id)
        {
            return db.SingleChoice.Count(e => e.id == id) > 0;
        }
    }
}