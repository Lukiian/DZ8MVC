using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ADO;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace DZ6MVC.Controllers
{
    public class LecturersController : Controller
    {
        private readonly Repository repository;

        public LecturersController(Repository repository)
        {
            this.repository = repository;
        }

        public IActionResult Lecturers()
        {
            return View(repository.GetAllLecturers());
        }

        public IActionResult Create()
        {
            ViewData["Action"] = "Create";
            var lecturer = new Lecturer();
            return View("Create", lecturer);
        }

        public IActionResult Edit(int id)
        {
            ViewData["Action"] = "Edit";
            var lecturer = repository.GetLecturerById(id);
            return View("Create", lecturer);
        }

        [HttpPost]
        public IActionResult Edit(Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View("Create", lecturer);
            }
            repository.UpdateLecturer(lecturer);
            return RedirectToAction("Lecturers");
        }

        [HttpPost]
        public IActionResult Create(Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View("Create", lecturer);
            }
            repository.CreateLecturer(lecturer);
            return RedirectToAction("Lecturers");
        }

        
        public IActionResult Delete(int id)
        {
            repository.DeleteLecturer(id);
            return RedirectToAction("Lecturers");
        }
    }
}