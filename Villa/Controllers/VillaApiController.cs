using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villa.DataBase;
using Villa.Model;
using Villa.Model.Dto;

namespace Villa.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public VillaApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult <IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("/getVilla{id:int}",Name ="GetVilla")]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(villa => villa.Id == id);
            
            if(villa==null)
            {
                return BadRequest("Villa Do not Exist");
            }

            return Ok(villa);
        }

        [HttpPost("/addVilla")]
        public ActionResult<VillaDto> addVilla([FromBody] VillaDto villaDTO)
        {
            if (_db.Villas.FirstOrDefault(villa => villa.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                return BadRequest("Villa Exist");
            }

            if(villaDTO==null)
            {
                return BadRequest();
            }
            if(villaDTO.Id>0)
            {
                return BadRequest();
            }
            VillaModel model = new()
            {
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

       [HttpDelete("/deleteVilla{id:int}",Name ="deleteVilla")]
       public IActionResult deleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest("No ID");
            }
            var villa = _db.Villas.FirstOrDefault(villa => villa.Id == id);
            if(villa==null)
            {
                return BadRequest("Villa not found");
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return Ok("Villa Deleted");
       }
        [HttpPut("/updateVilla{id:int}", Name = "UpdateVilla")]
        public IActionResult updateVilla(int id, [FromBody] VillaDto villaDTO)
        {
            if(villaDTO==null||id==0)
            {
                return BadRequest("No Id or Villa");
            }

            VillaModel model = new()
            {
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Update(model);
            _db.SaveChanges();
            return Ok("Updated");
        }
        [HttpPatch("/updateAttri{id:int}",Name = "updateAttribute")]

        public IActionResult updatePatch(int id, JsonPatchDocument<VillaDto> patchDTO)
        {
            if(patchDTO==null||id==null)
            {
               return BadRequest("Missing");
            }

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(villa => villa.Id == id);
            if(villa==null)
            {
                BadRequest("Villa not Exist");
            }

            VillaDto villaDTO = new()
            {
                Id = villa.Id,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            VillaModel model = new()
            {
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            return Ok("Updated");
        }
    }
}
