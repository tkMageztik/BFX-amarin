using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NS.MBX.Web.Data;
using NS.MBX.Web.Models;

namespace NS.MBX.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/TipoDocumentoApi")]
    public class TipoDocumentoApiController : Controller
    {
        private readonly NSMBXWebContext _context;

        public TipoDocumentoApiController(NSMBXWebContext context)
        {
            _context = context;
        }

        // GET: api/TipoDocumentoApi
        [HttpGet]
        public IEnumerable<TipoDocumentoViewModel> GetTipoDocumentoViewModel()
        {
            return _context.TipoDocumentoViewModel;
        }

        // GET: api/TipoDocumentoApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoDocumentoViewModel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipoDocumentoViewModel = await _context.TipoDocumentoViewModel.SingleOrDefaultAsync(m => m.Id == id);

            if (tipoDocumentoViewModel == null)
            {
                return NotFound();
            }

            return Ok(tipoDocumentoViewModel);
        }

        // PUT: api/TipoDocumentoApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoDocumentoViewModel([FromRoute] string id, [FromBody] TipoDocumentoViewModel tipoDocumentoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoDocumentoViewModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoDocumentoViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoDocumentoViewModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TipoDocumentoApi
        [HttpPost]
        public async Task<IActionResult> PostTipoDocumentoViewModel([FromBody] TipoDocumentoViewModel tipoDocumentoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TipoDocumentoViewModel.Add(tipoDocumentoViewModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoDocumentoViewModel", new { id = tipoDocumentoViewModel.Id }, tipoDocumentoViewModel);
        }

        // DELETE: api/TipoDocumentoApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoDocumentoViewModel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipoDocumentoViewModel = await _context.TipoDocumentoViewModel.SingleOrDefaultAsync(m => m.Id == id);
            if (tipoDocumentoViewModel == null)
            {
                return NotFound();
            }

            _context.TipoDocumentoViewModel.Remove(tipoDocumentoViewModel);
            await _context.SaveChangesAsync();

            return Ok(tipoDocumentoViewModel);
        }

        private bool TipoDocumentoViewModelExists(string id)
        {
            return _context.TipoDocumentoViewModel.Any(e => e.Id == id);
        }
    }
}