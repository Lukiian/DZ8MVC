using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ADO;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace DZ6MVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Repository repository;

        public StudentsController(Repository repository)
        {
            this.repository = repository;
        }

        public IActionResult Students()
        {
            return View(repository.GetAllStudents());
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = repository.GetStudentById(id);
            ViewData["Action"] = "Edit";
            return View("Create", student);
        }

        [HttpPost]
        public IActionResult Edit(Student model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View("Create", model);
            }
            repository.UpdateStudent(model);

            return RedirectToAction("Students");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            repository.DeleteStudent(id);
            return RedirectToAction("Students");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Action"] = "Create";
            var student = new Student();
            return View("Create", student);
        }


        [HttpPost]
        public IActionResult Create(Student model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                return View("Create", model);
            }

            repository.CreateStudent(model);
            return RedirectToAction("Students");

        }

    }
}