using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Persistence;
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
    }
}