using LibraryService.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using LibraryServiceReference;

namespace LibraryService.WebClient.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILogger<LibraryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(SearchType searchType, string searchString)
        {
            LibraryWebServiceSoapClient client = new LibraryWebServiceSoapClient
                (LibraryWebServiceSoapClient.EndpointConfiguration.LibraryWebServiceSoap);

            try
            {
                if (!string.IsNullOrEmpty(searchString) && searchString.Length >= 3)
                {
                    switch (searchType)
                    {
                        case SearchType.Title:
                            return View(new BookCategoryViewModel
                            {
                                Books = client.GetBooksByTitle(searchString)
                            });
                        case SearchType.Category:
                            return View(new BookCategoryViewModel
                            {
                                Books = client.GetBooksByCategory(searchString)
                            });
                        case SearchType.Author:
                            return View(new BookCategoryViewModel
                            {
                                Books = client.GetBooksByAuthor(searchString)
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

            return View(new BookCategoryViewModel { Books = new Book[] { } });
        }
    }
}