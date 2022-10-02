using Homework_05.Data;
using Homework_05.ViewModels;
using Homework_05.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.Controllers
{
    public class LibraryController : Controller
    {
        LibraryDataService DataService = new LibraryDataService();

        public IActionResult Index()
        {
            BooksVM BVM = new BooksVM();
            BVM.BookTypes = DataService.GetBookTypes();
            BVM.Authors = DataService.GetAuthors();
            BVM.Books = DataService.GetBooks();
            return View(BVM);
        }
        public IActionResult ReturnBook(int bookId, int studentId)
        {
            DataService.ReturnBook(bookId, studentId);
            DataService.GetBookBorrowsById(bookId);
            BooksBorrowedVM BBVM = new BooksBorrowedVM();
            BBVM.Books = DataService.GetBorrowedBookById(bookId);
            BBVM.Borrows = DataService.GetBookBorrowsById(bookId);

            //foreach ()
            //{

            //}
            return View("ViewBookDetails", BBVM);
        }
        public IActionResult BorrowBook(int bookId, int studentId)
        {
            DataService.BorrowBook(bookId, studentId);
            BooksBorrowedVM BBVM = new BooksBorrowedVM();
            return View("ViewBookDetails", BBVM);
        }
        public ActionResult ViewBookDetails(int bookId)
        {
            DataService.GetBookBorrowsById(bookId);
            BooksBorrowedVM BBVM = new BooksBorrowedVM();
            BBVM.Books = DataService.GetBorrowedBookById(bookId);
            BBVM.Borrows = DataService.GetBookBorrowsById(bookId);

            return View(BBVM);
        }
        public void StudentStatus(int bookid)
        {
            foreach (var student in DataService.GetStudents())
            {
                foreach (var borrow in DataService.GetBookBorrowsById(bookid))
                {
                    if (String.IsNullOrEmpty(borrow.ReturnDate))
                    {
                        student.HasBook = true;
                    }
                    else
                    {
                        student.HasBook = false;
                    }
                }
            }
        }

        public ActionResult ViewStudents(int bookid)
        {

            StudentStatus(bookid);
            List<Class> classes = new List<Class>();
            foreach (Student student in DataService.GetStudents())
            {
                Class cl = new Class
                {
                    Name = student.StudentClass
                };
                if (classes.Where(n => n.Name == student.StudentClass).Count() == 0)
                {
                    classes.Add(cl);
                }
            }


            StudentsVM SVM = new StudentsVM
            {
                Books = DataService.GetBorrowedBookById(bookid),
                Students = DataService.GetStudents(),
            };
            return View(SVM);
        }

        public ActionResult SearchBook(string bookName = null, int authorId = 0, int bookTypeId = 0)
        {
            BooksVM BVM = new BooksVM();
            BVM.BookTypes = DataService.GetBookTypes();
            BVM.Authors = DataService.GetAuthors();
            // Search Name, Type and Author specific search using contains
            if (bookName != null && authorId != 0 && bookTypeId != 0)
            {
                BVM.Books = DataService.GetBooks().Where(book => book.BookName.Contains(bookName.Trim()) && book.BookType.TypeID == bookTypeId && book.Author.AuthorID == authorId).ToList();
            }
            // Search for a Name using contains
            if (bookName != null)
            {
                BVM.Books = DataService.GetBooks().Where(book => book.BookName.Contains(bookName.Trim())).ToList();
            }
            // Search for a Author
            if (authorId != 0)
            {
                BVM.Books = DataService.GetBooks().Where(book => book.Author.AuthorID == authorId).ToList();
            }
            // Search for the Type of book
            if (bookTypeId != 0)
            {
                BVM.Books = DataService.GetBooks().Where(book => book.BookType.TypeID == bookTypeId).ToList();
            }
            return View("Index", BVM);
        }

        [HttpPost]
        public ActionResult SearchStudent(int bookid, string studentName = null, string _class = null)
        {
            StudentStatus(bookid);
            List<Class> classes = new List<Class>();
            foreach (Student student in DataService.GetStudents())
            {
                Class cl = new Class
                {
                    Name = student.StudentClass
                };
                if (classes.Where(n => n.Name == student.StudentClass).Count() == 0)
                {
                    classes.Add(cl);
                }
            }
            StudentsVM studentVM = new StudentsVM();
            studentVM.Class = classes;
            studentVM.Books = DataService.GetBorrowedBookById(bookid);
            if (_class != "Select a Class" || _class != null)
            {
                studentVM.Students = DataService.GetStudents().Where(cl => cl.StudentClass == _class).ToList();
            }
            if (studentName != "")
            {
                studentVM.Students = DataService.GetStudents().Where(cl => cl.StudentName.Contains(studentName)).ToList();
            }
            if (studentName != "" && ((_class != "Select a Class" || _class != null)))
            {
                studentVM.Students = DataService.GetStudents().Where(cl => cl.StudentName.Contains(studentName.Trim()) && cl.StudentClass == _class).ToList();
            }

            return View("ViewStudents", studentVM);

        }
    }
}
