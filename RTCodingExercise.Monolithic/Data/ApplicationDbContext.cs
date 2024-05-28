using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Plate> Plates { get; set; }

        public List<Plate> PlatesWithSalesMarkup(List<Plate> plates)
        {
            var discount = 20; // TODO Store sales markup in the database

            foreach (var plate in plates)
            {
                plate.SalePrice += Math.Round((plate.SalePrice / 100) * discount, 2);
            }

            return plates.ToList();
        }
    }
}