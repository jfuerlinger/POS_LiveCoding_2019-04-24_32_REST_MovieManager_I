using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Persistence;
using MovieManager.Web.DataTransferObjects;
using System.Linq;

namespace MovieManager.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public string[] GetCategories()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                return unitOfWork
                    .CategoryRepository
                    .GetAll()
                    .Select(category => category.CategoryName)
                    .ToArray();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public CategoryWithMoviesDTO GetCategoryById(int id)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var category = unitOfWork.CategoryRepository.GetByIdWithMovies(id);
                return new CategoryWithMoviesDTO(category);
            }
        }
    }
}