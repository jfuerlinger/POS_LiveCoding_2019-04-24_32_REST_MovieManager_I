using MovieManager.Core.Entities;
using System.Threading.Tasks;

namespace MovieManager.Core.Contracts
{
    public interface IMovieRepository
    {
        int GetCount();
        Movie[] GetAllByCatId(int id);
        void Insert(Movie movie);
        Movie GetById(int id);
        void Delete(Movie movie);
        Movie GetLongestMovie();
        void AddRange(Movie[] movies);
        Movie[] GetAll();
        void Add(Movie movie);
    }
}
