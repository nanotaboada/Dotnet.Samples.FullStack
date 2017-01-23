using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;


namespace Dotnet.Samples.FullStack.Data.Tests
{
    [TestClass]
    public class FullStackDataModelTests
    {
        [TestMethod][Ignore]
        public void GivenDbContext_WhenSelectingAllItemsInDbSet_ThenResultShouldNotBeEmpty()
        {
            // Arrange
            using (var dbContext = new DbContext("name=FullStackDbContext"))
            {
                var dbSet = dbContext.Set<Book>();

                // Act
                var result = from books in dbSet select books;

                // Assert
                result.Should().NotBeEmpty();
            }
        }
    }
}
