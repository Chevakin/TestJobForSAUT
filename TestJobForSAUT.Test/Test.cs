using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestJobForSAUT.Comparer;
using TestJobForSAUT.Core;
using Xunit;

namespace TestJobForSAUT.Test
{
    public class Test
    {
        [Fact]
        public void ThreeDifference_FromTA()
        {
            //Arrange

            var first = new Person
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Address = new Address
                {
                    City = "Екатеринбург",
                    Street = "Ленина",
                    House = 1
                }
            };

            var second = new Person
            {
                FirstName = "Иван",
                LastName = "Сидоров",
                Address = new Address
                {
                    City = "Екатеринбург",
                    Street = "Малышева",
                    House = 4
                }
            };

            IEnumerable<Difference> correctDifferences = new Difference[]
            {
                new Difference("LastName", "Иванов", "Сидоров"),
                new Difference("Address.Street", "Ленина", "Малышева"),
                new Difference("Address.House", $"{1}", $"{4}"),
            };

            IComparer comparer = new SimpleObjectComparer();

            //Act

            var differences = comparer.Compare(first, second);

            //Assert

            correctDifferences
                .Should()
                .BeEquivalentTo(differences);

        }
    }
}
