using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using System.Linq;

namespace MovieManager.Web.Controllers
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

        [HttpGet]
        public string[] Get()
        {
            return _unitOfWork
                .CategoryRepository
                .GetAll()
                .Select(c => c.CategoryName)
                    .ToArray();
        }

        [HttpGet]
        [Route("{id}/movies")]
        public MovieDTO[] GetMoviesByCategory(int id)
        {
            return _unitOfWork.CategoryRepository.GetById(id)?.Movies
                .Select(m => new MovieDTO()
                {
                    Title = m.Title,
                    Year = m.Year,
                    Category = m.Category?.CategoryName
                }).ToArray();
        }


    }
}