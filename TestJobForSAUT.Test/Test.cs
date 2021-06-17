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
        public void ThreeDifference_FromTA_ReturnsSameDifferenceObjects()
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

        [Fact]
        public void FiveDifferences_AllPropsIsDifference_ReturnsSameDifferenceObjects()
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
                FirstName = "Андрей",
                LastName = "Сидоров",
                Address = new Address
                {
                    City = "Москва",
                    Street = "Малышева",
                    House = 4
                }
            };

            IEnumerable<Difference> correctDifferences = new Difference[]
            {
                new Difference("FirstName", "Иван", "Андрей"),
                new Difference("LastName", "Иванов", "Сидоров"),
                new Difference("Address.City", "Екатеринбург", "Москва"),
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

        [Fact]
        public void ZeroDifference_NonePropsDifference_returnEmptyIEnumerable()
        {
            //Arrange

            var diff = new Person
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

            IEnumerable<Difference> correctDifferences = new Difference[0];

            IComparer comparer = new SimpleObjectComparer();

            //Act

            var differences = comparer.Compare(diff, diff);

            //Assert

            correctDifferences
                .Should()
                .BeEquivalentTo(differences);
        }

        [Fact]
        public void Incorrect_FirstDifferenceIsNull_returnArgumentNullException()
        {
            //Arrange

            var diff = new Person
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

            IComparer comparer = new SimpleObjectComparer();

            //Act

            var excepltion = Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, diff));

            //Assert

            excepltion.Should()
                .NotBeNull()
                .And
                .Match<ArgumentNullException>(e => e.ParamName == "first");
        }

        [Fact]
        public void Incorrect_SecondeDifferenceIsNull_returnArgumentNullException()
        {
            // Arrange

            var diff = new Person
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

            IComparer comparer = new SimpleObjectComparer();

            //Act

            var excepltion = Assert.Throws<ArgumentNullException>(() => comparer.Compare(diff, null));

            //Assert

            excepltion.Should()
                .NotBeNull()
                .And
                .Match<ArgumentNullException>(e => e.ParamName == "second");
        }

        [Fact]
        public void Incorrect_InputObjectsIsIncorrectType_returnException()
        {
            //Arrange

            var obj = new ArgumentException();

            IComparer comparer = new SimpleObjectComparer();

            //Act

            var excepltion = Assert.Throws<Exception>(() => comparer.Compare(obj, obj));

            //Assert

            excepltion.Should()
                .NotBeNull()
                .And
                .Match<Exception>(e => e.Message == $"Тип {typeof(ArgumentException).FullName} некорректен, т.к. содержит минимум одно свойство некорректного типа");
        }

        [Fact]
        public void Incorrect_ObjectContainsNullProps_returnException()
        {
            // Arrange

            var diff = new Person
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Address = null
            };

            IComparer comparer = new SimpleObjectComparer();

            //Act

            var excepltion = Assert.Throws<Exception>(() => comparer.Compare(diff, diff));

            //Assert

            excepltion.Should()
                .NotBeNull()
                .And
                .Match<Exception>(e => e.Message == "Значение как минимум одного из свойств null");
        }
    }
}
