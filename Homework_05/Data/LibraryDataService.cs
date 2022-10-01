using Homework_05.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.Data
{
    public class LibraryDataService
    {
        private IConfiguration appConfig;
        private static LibraryDataService instance;
        public String connectionString = "Data Source=DESKTOP-R84BLOF\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=true;";
        public SqlConnection currConn;

        public static LibraryDataService getInstance()
        {
            if (instance == null)
            {
                instance = new LibraryDataService();
            }
            return instance;
        }

        public void setConnectionString(String someConnStr)
        {
            connectionString = appConfig.GetConnectionString("PersonalComputerConnection");
            someConnStr = connectionString;
        }
        public bool openConnection()
        {
            bool status = true;
            try
            {
                String conString = appConfig.GetConnectionString("PersonalComputerConnection");
                currConn = new SqlConnection(conString);
                currConn.Open();
            }
            catch (Exception exc)
            {
                status = false;
            }
            return status;
        }
        public bool closeConnection()
        {
            if (currConn != null)
            {
                currConn.Close();
            }
            return true;
        }

        public List<Book> GetBooks()
        {
            List<Book> Books = new List<Book>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM books", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book Book = new Book();
                            Book.BookName = reader["name"].ToString();
                            Book.BookID = Convert.ToInt32(reader["bookId"]);
                            Book.BookType = GetBookTypeByID((int)reader["typeId"]);
                            Book.Author = GetAuthorByID(Convert.ToInt32(reader["authorId"]));
                            Book.BookPoint = Convert.ToInt32(reader["point"]);
                            Book.BookPageCount = (int)reader["pagecount"];

                            Books.Add(Book);
                        }
                    }
                }
                con.Close();
            }
            foreach (var Book in Books)
            {
                var borrows = GetBookBorrowsById(Book.BookID);
                foreach (var borrow in borrows)
                {
                    if (String.IsNullOrEmpty(borrow.ReturnDate))
                    {
                        Book.Status = "Out";
                    }
                    else
                    {
                        Book.Status = "Avaliable";
                    }
                }
            }
            return Books;
        }
        public List<Borrow> GetBookBorrows()
        {
            List<Borrow> Borrows = new List<Borrow>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM borrows", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Borrow bookBorrow = new Borrow
                            {
                                BorrowID = (int)reader["borrowId"],
                                BookID = (int)reader["bookid"],
                                Borrower = GetStudentById((int)reader["studentId"]),
                                BorrowDate = reader["takenDate"].ToString(),
                                ReturnDate = reader["broughtDate"].ToString()
                            };


                        }
                    }
                }
                con.Close();
            }
            return Borrows;
        }
        public List<Borrow> GetBookBorrowsById(int id)
        {
            List<Borrow> BookBorrows = new List<Borrow>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM borrows WHERE borrows.bookId = " + id, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Borrow bookBorrow = new Borrow
                            {
                                BorrowID = Convert.ToInt32(reader["borrowId"]),
                                BookID = Convert.ToInt32(reader["bookId"]),
                                Borrower = GetStudentById(Convert.ToInt32(reader["studentId"])),
                                BorrowDate = Convert.ToString(reader["takenDate"]),
                                ReturnDate = Convert.ToString(reader["broughtDate"])
                            };
                            BookBorrows.Add(bookBorrow);
                        }
                    }
                }
                con.Close();
            }
            return BookBorrows;
        }
        public Student GetStudentById(int id)
        {
            Student student = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE studentId = " + id, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            student = new Student
                            {
                                StudentID = (int)reader["studentId"],
                                StudentName = (string)reader["name"],
                                StudentSurname = (string)reader["surname"],
                                StudentClass = (string)reader["class"],
                                StudentPoint = (int)reader["point"]
                            };
                        }
                    }
                }
                con.Close();
            }
            return student;
        }
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM students ", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                StudentID = (int)reader["studentId"],
                                StudentName = (string)reader["name"],
                                StudentSurname = (string)reader["surname"],
                                StudentClass = (string)reader["class"],
                                StudentPoint = (int)reader["point"]
                            };
                            students.Add(student);
                        }
                    }
                }
                conn.Close();
            }
            return students;
        }
        public Book GetBorrowedBookById(int id)
        {
            Book Book = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE bookId = " + id, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book = new Book();
                            Book.BookName = reader["name"].ToString();
                            Book.BookID = Convert.ToInt32(reader["bookId"]);
                            Book.BookType = GetBookTypeByID((int)reader["typeId"]);
                            Book.Author = GetAuthorByID(Convert.ToInt32(reader["authorId"]));
                            Book.BookPoint = Convert.ToInt32(reader["point"]);
                            Book.BookPageCount = (int)reader["pagecount"];
                        }
                    }
                }
                con.Close();
            }

            var borrows = GetBookBorrowsById(Book.BookID);
            foreach (var borrow in borrows)
            {
                if (String.IsNullOrEmpty(borrow.ReturnDate))
                {
                    Book.Status = "Out";
                }
                else
                {
                    Book.Status = "Avaliable";
                }
            }

            return Book;
        }
        public BookType GetBookTypeByID(int id)
        {
            BookType bookType = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM types WHERE typeId = " + id, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookType = new BookType
                            {
                                TypeID = (int)reader["typeId"],
                                TypeName = reader["name"].ToString()
                            };
                        }
                    }
                }
                con.Close();
            }
            return bookType;
        }
        public List<BookType> GetBookTypes()
        {
            List<BookType> bookTypes = new List<BookType>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM types", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookType bookType = new BookType
                            {
                                TypeID = (int)reader["typeId"],
                                TypeName = reader["name"].ToString()
                            };
                            bookTypes.Add(bookType);
                        }
                    }
                }
                con.Close();
            }
            return bookTypes;
        }

        // Get Author by id 
        public Author GetAuthorByID(int id)
        {
            Author author = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE authorId = " + id, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            author = new Author
                            {
                                AuthorID = (int)reader["authorId"],
                                AuthorName = reader["name"].ToString(),
                                AuthorSurname = reader["surname"].ToString()
                            };
                        }
                    }
                }
                con.Close();
            }
            return author;
        }

        public List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM authors ", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Author author = new Author
                            {
                                AuthorID = (int)reader["authorId"],
                                AuthorName = reader["name"].ToString(),
                                AuthorSurname = reader["surname"].ToString()
                            };
                            authors.Add(author);
                        }
                    }
                }
                con.Close();
            }
            return authors;
        }

    }
}
