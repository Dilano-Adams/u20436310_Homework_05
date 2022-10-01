using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public DateTime StudentBirthday { get; set; }
        public string StudentGender { get; set; }
        public string StudentClass { get; set; }
        public int StudentPoint { get; set; }
        public bool HasBook { get; set; }
    }
}
