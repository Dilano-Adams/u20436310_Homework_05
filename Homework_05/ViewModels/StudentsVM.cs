﻿using Homework_05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework_05.ViewModels
{
    public class StudentsVM
    {
        public Book Books { get; set; }
        public List<Student> Students { get; set; }
        public List<Class> Class { get; set; }
    }
}
