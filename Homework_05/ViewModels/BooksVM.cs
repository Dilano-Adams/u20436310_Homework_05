using Homework_05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.ViewModels
{
    public class BooksVM
    {
        public List<Book> Books = new List<Book>();
        public List<Author> Authors = new List<Author>();
        public List<BookType> BookTypes = new List<BookType>();
    }
}
