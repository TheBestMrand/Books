﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class BookReview
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
