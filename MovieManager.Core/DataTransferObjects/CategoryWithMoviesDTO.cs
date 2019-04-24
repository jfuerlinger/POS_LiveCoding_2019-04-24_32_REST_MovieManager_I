using MovieManager.Core.Entities;
using System.Linq;

namespace MovieManager.Core.DataTransferObjects
{
    public class CategoryWithMoviesDTO
    {
        public string CategoryName { get; set; }
        public MovieDTO[] Movies { get; set; }

        public CategoryWithMoviesDTO(Category category)
        {
            CategoryName = category.CategoryName;
            Movies = category
                .Movies
                .Select(movie => new MovieDTO() // TODO: in Konstruktor verlagern!
                {
                    Duration = movie.Duration,
                    Title = movie.Title,
                    Year = movie.Year
                }).ToArray();
        }
    }


}
