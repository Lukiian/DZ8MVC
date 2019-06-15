using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ADO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Models.Models;

namespace DZ6MVC.Controllers
{
    public class HomeTasksController : Controller
    {
        private readonly Repository repository;

        public HomeTasksController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Create(int courseId)
        {
            ViewData["Action"] = "Create";
            ViewData["CourseId"] = courseId;
            var assessmentStudent = new HomeTask();
            return View("Create", assessmentStudent);
        }

        [HttpPost]
        public IActionResult Create(HomeTask homeTask, int courseId)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                ViewData["CourseId"] = courseId;
                return View("Create", homeTask);
            }
            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", courseId);

            repository.CreateHomeTask(homeTask, courseId);
            return RedirectToAction("Edit", "Courses", routeValueDictionary);
        }

        public IActionResult Delete(int homeTaskId, int courseId)
        {
            repository.DeleteHomeTask(homeTaskId);

            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", courseId);
            return RedirectToAction("Edit", "Courses", routeValueDictionary);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            HomeTask homeTask = repository.GetHomeTaskById(id);
            ViewData["Action"] = "Edit";
            return View("Create", homeTask);
        }

        [HttpPost]
        public IActionResult Edit(HomeTask homeTask)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return View(homeTask);
            }

            var homeTask1 = repository.GetHomeTaskById(homeTask.Id);

            var routeValueDictionary = new RouteValueDictionary();
            repository.UpdateHomeTask(homeTask);
            routeValueDictionary.Add("id", homeTask1.Course.Id);
            return RedirectToAction("Edit", "Courses", routeValueDictionary);
        }

        [HttpGet]
        public IActionResult Assessment(int id)
        {
            var homeTask1 = repository.GetHomeTaskById(id);
            AssessmentStudent assessmentViewModel =
                new AssessmentStudent
                {
                    Date = homeTask1.Date,
                    Description = homeTask1.Description,
                    Title = homeTask1.Title,
                    HomeTaskStudents = new List<AssessmentStudent_Student>(),
                    HomeTaskId = homeTask1.Id
                };
            if (homeTask1.HomeTaskAssessments.Any())
            {
                foreach (var homeTaskHomeTaskAssessment in repository.GetHomeTaskAssessmentsByHomeTaskId(id))
                {
                    assessmentViewModel.HomeTaskStudents.Add(new AssessmentStudent_Student()
                    {
                        StudentName = homeTaskHomeTaskAssessment.Student.Name,
                        StudentId = homeTaskHomeTaskAssessment.Student.Id,
                        IsComplete = homeTaskHomeTaskAssessment.IsComplete,
                        HomeTaskAssessmentId = homeTaskHomeTaskAssessment.Id
                    });
                }

            }
            else
            {
                foreach (var student in homeTask1.Course.Students)
                {
                    assessmentViewModel.HomeTaskStudents.Add(new AssessmentStudent_Student() { StudentName = student.Name, StudentId = student.Id });
                }
            }
            return View(assessmentViewModel);
        }

        [HttpPost]
        public IActionResult Assessment(AssessmentStudent assessmentStudent)
        {
            var homeTask = repository.GetHomeTaskById(assessmentStudent.HomeTaskId);

            if (homeTask.HomeTaskAssessments.Any())
            {
                List<HomeTaskAssessment> assessments = new List<HomeTaskAssessment>();
                foreach (var homeTaskStudent in assessmentStudent.HomeTaskStudents)
                {
                    assessments.Add(new HomeTaskAssessment() { Id = homeTaskStudent.HomeTaskAssessmentId, Date = DateTime.Now, IsComplete = homeTaskStudent.IsComplete });
                }
                repository.UpdateHomeTaskAssessments(assessments);
            }
            else
            {
                foreach (var homeTaskStudent in assessmentStudent.HomeTaskStudents)
                {
                    var student = repository.GetStudentById(homeTaskStudent.StudentId);
                    homeTask.HomeTaskAssessments.Add(new HomeTaskAssessment { HomeTask = homeTask, IsComplete = homeTaskStudent.IsComplete, Student = student, Date = DateTime.Now });
                }
                repository.CreateHomeTaskAssessments(homeTask.HomeTaskAssessments);

            }
            return RedirectToAction("Courses", "Courses");
        }
    }
}