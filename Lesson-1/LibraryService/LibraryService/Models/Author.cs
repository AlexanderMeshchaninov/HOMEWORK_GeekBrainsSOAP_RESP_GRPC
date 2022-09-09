using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Models
{
    public class Author
    {
        public string Name { get; set; }
        public string Lang { get; set; }

        public override string ToString() => $"Name:{Name}, Language:{Lang}";
    }
}