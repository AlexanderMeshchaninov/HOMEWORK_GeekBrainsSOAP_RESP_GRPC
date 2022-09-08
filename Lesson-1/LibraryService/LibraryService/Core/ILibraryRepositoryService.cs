using LibraryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Core
{
    public interface ILibraryRepositoryService : IRepository<Book, string>
    {
        IList<Book> GetByTitle(string title);

        IList<Book> GetByAuthor(string authorName);

        IList<Book> GetByCategory(string category);
    }
}