using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListItemIndexTests
    {
        UnrolledLinkedList<int> _List;

        [Test]
        public void Items_get_throws_when_index_is_out_of_range([Values(-1, 15, 16)] int index)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 15);

            // Act, Assert
            Assert.Throws<IndexOutOfRangeException>(() => { var res = _List[index]; });
        }

        [Test]
        public void Items_set_throws_when_index_is_out_of_range(
            [Values(-1, 15, 16)] int index)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 15);

            // Act, Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _List[index] = 8; });
        }

        [Test, Sequential]
        public void Retrieving_item_by_index(
            [Values(0, 2, 5, 8)] int index,
            [Values(1, 3, 6, 9)] int result)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 15);

            // Act, Assert
            Assert.That(_List[index], Is.EqualTo(result));
        }


        [Test, Sequential]
        public void Setting_item_by_index(
            [Values(0, 2, 5, 8)] int index,
            [Values(8, 7, 6, 5)] int result)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 15);

            // Act
            _List[index] = result;

            // Act, Assert
            Assert.That(_List[index], Is.EqualTo(result));
        }

        [Test, Sequential]
        public void IndexOf_returns_first_index_of_an_element(
            [Values(17, 1, 3, 5, 9, 15, 16, -1)] int elem,
            [Values(-1, 0, 2, 4, 8, 14, -1, -1)] int index)
        {
            // Assert
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 15);

            // Act, Assert
            Assert.That(_List.IndexOf(elem), Is.EqualTo(index));
        }

        [Test, Sequential]
        public void Insert_throws_IndexOutOfRangeException_for_invalid_index(
            [Values(-1, 18, -100, 13)] int index,
            [Values(7, 15, 6, 11)] int listCount)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, listCount);

            // Act, Assert
            Assert.Throws<IndexOutOfRangeException>(() => _List.Insert(index, 42));
        }
    }
}
