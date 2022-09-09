using LibraryService.Core;
using LibraryService.Models;
using LibraryService.Services;
using System.Linq;
using System.Web.Services;

namespace LibraryService
{
    /// <summary>
    /// Summary description for LibraryWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LibraryWebService : WebService
    {
        private readonly ILibraryRepositoryService _libraryRepositoryService;

        public LibraryWebService()
        {
            _libraryRepositoryService = new LibraryRepository(new LibraryDatabaseContext());
        }

        [WebMethod]
        public Book[] GetBooksByTitle(string title)
        {
            return _libraryRepositoryService.GetByTitle(title).ToArray();
        }

        [WebMethod]
        public Book[] GetBooksByAuthor(string authorName)
        {
            return _libraryRepositoryService.GetByAuthor(authorName).ToArray();
        }

        [WebMethod]
        public Book[] GetBooksByCategory(string category)
        {
            return _libraryRepositoryService.GetByCategory(category).ToArray();
        }
    }
}
