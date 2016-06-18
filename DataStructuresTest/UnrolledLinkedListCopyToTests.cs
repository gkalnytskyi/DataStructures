using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListCopyToTests
    {
        UnrolledLinkedList<int> _List;

        [Test]
        public void CopyTo_throws_exception_if_argument_array_is_null()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => _List.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_throws_exception_if_index_is_less_than_zero()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);
            var array = new int[9];

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _List.CopyTo(array, -1));
        }

        [Test, Sequential]
        public void CopyTo_throws_exception_if_data_does_not_fit_into_array_starting_at_index(
            [Values(9, 5)] int arraySize,
            [Values(2, 0)] int startIndex)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);
            var array = new int[arraySize];

            // Act, Assert
            Assert.Throws<ArgumentException>(() => _List.CopyTo(array, startIndex));
        }

        [Test]
        public void CopyTo_copies_items_to_array_starting_at_index_zero()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 6);
            var array = new int[10];

            // Act
            _List.CopyTo(array, 0);

            // Assert
            var expectedCollection = Enumerable.Range(1, 6).Concat(Enumerable.Repeat(0, 4));
            Assert.That(array, Is.EquivalentTo(expectedCollection));
        }

        [Test]
        public void CopyTo_copies_items_to_array_starting_at_non_zero_index()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 6);
            var array = new int[10];

            // Act
            _List.CopyTo(array, 2);

            // Assert
            var expectedCollection = Enumerable.Repeat(0, 2).Concat(Enumerable.Range(1, 6)).Concat(Enumerable.Repeat(0, 2));
            Assert.That(array, Is.EquivalentTo(expectedCollection));
        }
    }
}
