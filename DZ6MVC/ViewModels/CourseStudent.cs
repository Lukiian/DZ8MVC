using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DZ6MVC
{
    public class CourseStudent
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<CourseStudent_Student> Students { get; set; }
    }

    public class CourseStudent_Student
    {
        public int StudentId { get; set; }

        public string StudentFullName { get; set; }

        public bool IsAssigned { get; set; }
    }
}
