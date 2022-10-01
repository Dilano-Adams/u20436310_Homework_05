using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public int BookPageCount { get; set; }
        public int BookPoint { get; set; }

        public Author Author { get; set; }
        public BookType BookType { get; set; }
        public string Status { get; set; }
    }
}
