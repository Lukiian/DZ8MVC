using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DZ6MVC
{
    public class CourseLecturer
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<Lecturers> Lecturers { get; set; }
    }

    public class Lecturers
    {
        public int LecturerId { get; set; }

        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}
