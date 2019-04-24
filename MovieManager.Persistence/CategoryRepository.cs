using Microsoft.EntityFrameworkCore;
using MovieManager.Core.Contracts;
using MovieManager.Core.DTOs;
using MovieManager.Core.Entities;
using System;
using System.Linq;

namespace MovieManager.Persistence
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Liefert eine Liste aller Kategorien sortiert nach dem CategoryName
        /// </summary>
        /// <returns></returns>
        public Category[] GetAll()
        {
            return _dbContext.Categories.OrderBy(c => c.CategoryName).ToArray();
        }

        /// <summary>
        /// Liefert die Kategorie mit der übergebenen Id --> null wenn nicht gefunden
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category GetById(int id)
        {
            return _dbContext.Categories.SingleOrDefault(c => c.Id == id);
        }


        /// <summary>
        /// Liefert eine Liste mit der Anzahl und Gesamtdauer aller Filme je Kategorie
        /// Sortiert nach dem Namen der Kategorie (aufsteigend).
        /// </summary>
        public CategoryStatisticEntry[] GetCategoryStatistics()
        {
            return _dbContext.Movies.GroupBy(m => m.Category).Select(grp =>
                new CategoryStatisticEntry()
                {
                    Category = grp.Key,
                    NumberOfMovies = grp.Count(),
                    TotalDuration = grp.Sum(m => m.Duration)
                })
                .OrderBy(res => res.Category.CategoryName).ToArray();
        }

        /// <summary>
        /// Liefert die Kategorie mit den meisten Filmen
        /// </summary>
        public CategoryStatisticEntry GetCategoryWithMostMovies()
        {
            return _dbContext.Movies.GroupBy(m => m.Category).Select(grp =>
                    new CategoryStatisticEntry()
                    {
                        Category = grp.Key,
                        NumberOfMovies = grp.Count(),
                        TotalDuration = grp.Sum(m => m.Duration)
                    })
                .OrderByDescending(entry => entry.NumberOfMovies)
                .ThenBy(entry => entry.Category)  //!
                .FirstOrDefault();
        }

        /// <summary>
        /// Liefert die Kategorien mit der durchschnittlichen Länge der zugeordneten Filme.
        /// Absteigend sortiert nach der durchschnittlichen Dauer der Filme - bei gleicher
        /// Dauer dann nach dem Namen der Kategorie aufsteigend. 
        /// </summary>
        public (Category Category, double AverageLength)[] GetCategoriesWithAverageLengthOfMovies()
        {
            return _dbContext.Categories
                .Select(category =>
                    ValueTuple.Create(
                        category,
                        category.Movies.Average(movie => movie.Duration)))
                .OrderByDescending(result => result.Item2)
                .ThenBy(result => result.Item1.CategoryName)
                .ToArray();
        }

        public int GetYearWithMostPublicationsForCategory(string categoryName)
        {
            return _dbContext.Movies
                .Where(movie => movie.Category.CategoryName == categoryName)
                .GroupBy(movie => movie.Year)
                .Select(movieGroupByYear =>
                    new
                    {
                        Year = movieGroupByYear.Key,
                        CntOfMovies = movieGroupByYear.Count()
                    })
                .OrderByDescending(movieGroupByYear => movieGroupByYear.CntOfMovies)
                .First().Year;
        }

        public Category GetByIdWithMovies(int id)
        {
            return _dbContext
                .Categories
                .Include(c => c.Movies)
                .FirstOrDefault(c => c.Id == id);
        }


        /// <summary>
        /// Neue Kategorie wird in Datenbank eingefügt
        /// </summary>
        /// <param name="category"></param>
        public void Insert(Category category)
        {
            _dbContext.Categories.Add(category);
        }

    }
}