using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Test.DataAccess
{
    public class DatabaseContextFactoryTest
    {
        private readonly DatabaseContextFactory _sut;
        private readonly Action<DbContextOptionsBuilder> _builder;
        public DatabaseContextFactoryTest()
        {
            _builder = Mock.Of<Action<DbContextOptionsBuilder>>();
            _sut = new DatabaseContextFactory(_builder);
        }

        [Fact]
        public void Create_Given_ShouldReturnsDatabaseContext()
        {
            // Arrange
            // Act
            var result = _sut.Create();

            // Assert
            result.Should().BeOfType<DatabaseContext>();
            Mock.Get(_builder)
                .Verify(m => m(It.IsAny<DbContextOptionsBuilder>()), Times.Once);
        }
    }
}