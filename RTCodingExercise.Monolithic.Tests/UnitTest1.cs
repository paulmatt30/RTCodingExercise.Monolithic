using Microsoft.EntityFrameworkCore;
using RTCodingExercise.Monolithic.Data;
using RTCodingExercise.Monolithic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RTCodingExercise.Monolithic.Tests
{
    public class UnitTest1
    {
        private static DbContextOptions<ApplicationDbContext> Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "RTCodingExerciseDatabase")
                            .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Plates.Add(new Plate { Id = Guid.NewGuid(), Registration = "ASD123", PurchasePrice = 234.56M, SalePrice = 123.34M, Letters = "ASD", Numbers = 32 });
                context.Plates.Add(new Plate { Id = Guid.NewGuid(), Registration = "ASD567", PurchasePrice = 234.56M, SalePrice = 123.34M, Letters = "ASD", Numbers = 32 });
                context.Plates.Add(new Plate { Id = Guid.NewGuid(), Registration = "ASD345", PurchasePrice = 234.56M, SalePrice = 444.34M, Letters = "ASD", Numbers = 32 });
                context.SaveChanges();
            }

            return options;
        }

        [Fact]
        public void GetAllPlatesTest()
        {
            // Arrange
            DbContextOptions<ApplicationDbContext> options = Setup();

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                // Create List<Plate> plates to pass to PlatesWithSalesMarkup
                List<Plate> plates = new();

                foreach (var item in context.Plates)
                {
                    plates.Add(item);
                }

                // Act
                context.PlatesWithSalesMarkup(plates);

                // Assert
                Assert.Equal(3, plates.Count);
            }
        }

        [Fact]
        public void PlatesWithSalesMarkupAppliedTest()
        {
            DbContextOptions<ApplicationDbContext> options = Setup();

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                List<Plate> platesWithMarkupAppliedLocally = new();

                // Add discount to platesWithMarkupAppliedLocally list
                var discount = 20;

                foreach (var plate in context.Plates)
                {
                    plate.SalePrice += Math.Round((plate.SalePrice / 100) * discount, 2);
                    platesWithMarkupAppliedLocally.Add(plate);
                }

                // Call PlatesWithSalesMarkup method to apply markup 
                List<Plate> plates = new();
                foreach (var plate in context.Plates)
                {
                    plates.Add(plate);
                    
                }

                // Act
                context.PlatesWithSalesMarkup(plates);

                // Assert
                Assert.Equal(plates.First().SalePrice, platesWithMarkupAppliedLocally.First().SalePrice);
            }
        }
    }
}