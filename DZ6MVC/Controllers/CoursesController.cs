using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ADO;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace DZ6MVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Repository repository;

        public CoursesController(Repository repository)
        {
            this.repository = repository;
        }

        
        public IActionResult Courses()
        {
            return View(repository.GetAllCourses());
        }

        public IActionResult Delete(int id)
        {
            repository.DeleteCourse(id);

            return RedirectToAction("Courses");
        }

        public IActionResult Create()
        {
            ViewData["action"] = nameof(Create);
            return View("Edit", new Course());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course course = repository.GetCourse(id);
            
            ViewData["action"] = nameof(Edit);

            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course courseParameter)
        {
            repository.UpdateCourse(courseParameter);
            return RedirectToAction(nameof(Courses));
        }

        [HttpPost]
        public IActionResult Create(Course courseParameter)
        {
            LecturersController a = new LecturersController(repository);

            if (!ModelState.IsValid)
            {
                ViewData["action"] = nameof(Create);
                return a.View(repository.GetAllLecturers());
            }
            repository.CreateCourse(courseParameter);
            return RedirectToAction(nameof(Courses));
        }

        [HttpGet]
        public IActionResult AddStudent(int Id)
        {
            var students = repository.GetAllStudents();
            var course = repository.GetCourse(Id);
            CourseStudent model = new CourseStudent
            {
                Id = Id,
                EndDate = course.EndDate,
                Name = course.Name,
                StartDate = course.StartDate,
                Students = new List<CourseStudent_Student>()
            };

            foreach (var student in students)
            {
                bool isAssigned = course.Students.Any(p => p.Id == student.Id);
                model.Students.Add(new CourseStudent_Student() { StudentId = student.Id, StudentFullName = student.Name, IsAssigned = isAssigned });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddStudent(CourseStudent courseStudent)
        {
            repository.SetStudentsToCourse(courseStudent.Id, courseStudent.Students.Where(p => p.IsAssigned).Select(student => student.StudentId));

            return RedirectToAction("Courses");
        }

        [HttpGet]
        public IActionResult AddLecturers(int id)
        {
            var lecturers = repository.GetAllLecturers();
            var course = repository.GetCourse(id);
            CourseLecturer model = new CourseLecturer
            {
                Id = id,
                EndDate = course.EndDate,
                Name = course.Name,
                StartDate = course.StartDate,
                Lecturers = new List<Lecturers>()
            };

            foreach (var lecturer in lecturers)
            {
                bool isAssigned = course.Lecturers.Any(p => p.Id == lecturer.Id);
                model.Lecturers.Add(new Lecturers() { LecturerId = lecturer.Id, Name = lecturer.Name, IsAssigned = isAssigned });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddLecturers(CourseLecturer courseLecturer)
        {
            repository.SetLecturersToCourse(courseLecturer.Id, courseLecturer.Lecturers.Where(p => p.IsAssigned).Select(lecturer => lecturer.LecturerId));

            return RedirectToAction("Courses");

        }
    }
}