using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NS.MBX.Web.Models;

namespace NS.MBX.Web.Controllers
{
    public class TipoDocumentoViewModelsController : Controller
    {
        private readonly NSMBXWebContext _context;

        public TipoDocumentoViewModelsController(NSMBXWebContext context)
        {
            _context = context;
        }

        // GET: TipoDocumentoViewModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoDocumentoViewModel.ToListAsync());
        }

        // GET: TipoDocumentoViewModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDocumentoViewModel = await _context.TipoDocumentoViewModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tipoDocumentoViewModel == null)
            {
                return NotFound();
            }

            return View(tipoDocumentoViewModel);
        }

        // GET: TipoDocumentoViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDocumentoViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipDoc,DesDoc")] TipoDocumentoViewModel tipoDocumentoViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDocumentoViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDocumentoViewModel);
        }

        // GET: TipoDocumentoViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDocumentoViewModel = await _context.TipoDocumentoViewModel.SingleOrDefaultAsync(m => m.Id == id);
            if (tipoDocumentoViewModel == null)
            {
                return NotFound();
            }
            return View(tipoDocumentoViewModel);
        }

        // POST: TipoDocumentoViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TipDoc,DesDoc")] TipoDocumentoViewModel tipoDocumentoViewModel)
        {
            if (id != tipoDocumentoViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDocumentoViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDocumentoViewModelExists(tipoDocumentoViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDocumentoViewModel);
        }

        // GET: TipoDocumentoViewModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDocumentoViewModel = await _context.TipoDocumentoViewModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tipoDocumentoViewModel == null)
            {
                return NotFound();
            }

            return View(tipoDocumentoViewModel);
        }

        // POST: TipoDocumentoViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tipoDocumentoViewModel = await _context.TipoDocumentoViewModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.TipoDocumentoViewModel.Remove(tipoDocumentoViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDocumentoViewModelExists(string id)
        {
            return _context.TipoDocumentoViewModel.Any(e => e.Id == id);
        }
    }
}
