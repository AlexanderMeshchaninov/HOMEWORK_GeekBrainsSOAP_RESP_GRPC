using LibraryService.Models;
using System.Collections.Generic;

namespace LibraryService.Core
{
    public interface ILibraryDatabaseContextService
    {
        IList<Book> Books { get; }
    }
}