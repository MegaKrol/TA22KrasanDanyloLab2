using Microsoft.AspNetCore.Mvc;
using TA22KrasanLab2.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TA22KrasanLab2.Data;
using TA22KrasanLab2.Models;
using Microsoft.EntityFrameworkCore;

namespace TA22KrasanLab2.Controllers
{
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

        //public IActionResult Index(PigSortField sortField = PigSortField.Name)
        //{
        //    switch (sortField)
        //    {
        //        case PigSortField.Name:
        //            return View(_context.Pigs.OrderBy(x => x.Name).ToList());

        //        case PigSortField.Weight:
        //            return View(_context.Pigs.OrderBy(x => x.Weight).ToList());
        //    }

        //    return View(_context.Pigs.OrderBy(x => x.Name).ToList());
        //}

        public IActionResult Index(
                            string? searchString,
                            double? minWeight,
                            PigSortField sortField = PigSortField.Name,
                            int page = 1,
                            int pageSize = 5)
        {
            // Початковий запит
            var pigs = _context.Pigs.AsQueryable();

            // 🔍 ФІЛЬТРАЦІЯ
            if (!string.IsNullOrEmpty(searchString))
            {
                pigs = pigs.Where(p => p.Name.Contains(searchString));
            }

            if (minWeight.HasValue)
            {
                pigs = pigs.Where(p => p.Weight >= minWeight.Value);
            }

            // 🔽 СОРТУВАННЯ
            pigs = sortField switch
            {
                PigSortField.Weight => pigs.OrderBy(p => p.Weight),
                _ => pigs.OrderBy(p => p.Name)
            };

            // 🔢 ПАГІНАЦІЯ
            int totalItems = pigs.Count();
            var items = pigs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Передаємо дані у View через ViewBag
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
        public IActionResult Create()
        {
            var createPigVM = new CreatePigVM();

            return View(createPigVM);
        }

        [HttpPost]
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
        public IActionResult Delete(int id)
        {
            var pig = _context.Pigs.Where(pig => pig.Id == id).FirstOrDefault();

            _context.Remove(pig);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
