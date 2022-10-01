using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.Models
{
    public class Borrow
    {
        public int BorrowID { get; set; }
        public string BorrowDate { get; set; }
        public string ReturnDate { get; set; }

        public int BookID { get; set; }
        public Student Borrower { get; set; }
    }
}
