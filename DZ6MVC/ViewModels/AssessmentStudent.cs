using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DZ6MVC
{
    public class AssessmentStudent
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<AssessmentStudent_Student> HomeTaskStudents { get; set; }
        public int HomeTaskId { get; set; }
    }

    public class AssessmentStudent_Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool IsComplete { get; set; }
        public int HomeTaskAssessmentId { get; set; }
    }
}
