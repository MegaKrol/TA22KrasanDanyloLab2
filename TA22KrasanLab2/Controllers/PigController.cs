using Microsoft.AspNetCore.Mvc;
using TA22KrasanLab2.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TA22KrasanLab2.Data;
using TA22KrasanLab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TA22KrasanLab2.Controllers
{
    [Authorize]
    public class PigController : Controller
    {
        public enum PigSortField
        {
            Name,
            Weight
        }
        private FarmContext _context;

        public PigController(FarmContext context)
        {
            _context = context;
        }

        public IActionResult Index(
                            string? searchString,
                            double? minWeight,
                            PigSortField sortField = PigSortField.Name,
                            int page = 1,
                            int pageSize = 5)
        {
            // Початковий запит
            var pigs = _context.Pigs.AsQueryable();

            // filtration
            if (!string.IsNullOrEmpty(searchString))
            {
                pigs = pigs.Where(p => p.Name.Contains(searchString));
            }

            if (minWeight.HasValue)
            {
                pigs = pigs.Where(p => p.Weight >= minWeight.Value);
            }

            // sorting
            pigs = sortField switch
            {
                PigSortField.Weight => pigs.OrderBy(p => p.Weight),
                _ => pigs.OrderBy(p => p.Name)
            };

            // pagination
            int totalItems = pigs.Count();
            var items = pigs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.MinWeight = minWeight;
            ViewBag.SortField = sortField;

            return View(items);
        }

        //[HttpGet]
        //public Pig GetPig(int id)
        //{
        //    return _context.Pigs.Where(e => e.Id == id).FirstOrDefault();
        //}


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var createPigVM = new CreatePigVM();

            return View(createPigVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreatePigVM createPigVM)
        {
            if (ModelState.IsValid)
            {
                var Pig = new Pig
                {
                    Name = createPigVM.Name,
                    Weight = createPigVM.Weight,
                    Description = createPigVM.Description
                };
                _context.Add(Pig);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {

            var pig = _context.Pigs.Where(pig => pig.Id == id).FirstOrDefault();

            var EditPigVM = new EditPigVM
            {
                Id = pig.Id,
                Name = pig.Name,
                Description = pig.Description,
                Weight = pig.Weight
            };

            return View(EditPigVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(EditPigVM editPigVM)
        {
            if (ModelState.IsValid)
            {
                var pig = new Pig
                {
                    Id = editPigVM.Id,
                    Name = editPigVM.Name,
                    Description = editPigVM.Description,
                    Weight = editPigVM.Weight
                };

                _context.Update(pig);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var pig = _context.Pigs.Where(pig => pig.Id == id).FirstOrDefault();

            _context.Remove(pig);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
