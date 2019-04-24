using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using System.Linq;

namespace MovieManager.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/categories
        [HttpGet]
        public string[] Get()
        {
            return _unitOfWork
                .CategoryRepository
                .GetAll()
                .Select(c => c.CategoryName)
                .ToArray();
        }

        // GET api/categories/3/movies
        [HttpGet]
        [Route("{id}/movies")]
        public ActionResult<MovieDTO[]> GetMoviesByCategory(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var movies = category.Movies;
            var resources = movies
                .Select(m => new MovieDTO()
                {
                    Title = m.Title,
                    Year = m.Year,
                    Category = m.Category?.CategoryName
                })
                .ToArray();

            return resources;
        }
    }
}