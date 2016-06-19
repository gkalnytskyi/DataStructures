using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListInsertItemTests
    {
        UnrolledLinkedList<int> _List;

        [Test]
        public void AddFirst_adds_item_to_the_begining_of_the_list(
            [Values(0, 7, 8, 9)] int count)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            _List.Insert(0, 42);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(count + 1));
            Assert.That(_List, Is.EquivalentTo(Enumerable.Range(42, 1).Concat(Enumerable.Range(1, count))));
        }



        [Test]
        public void Insert_Item_to_the_begining_of_the_list(
            [Values(0, 7, 8, 9)] int count)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            _List.Insert(0, 42);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(count + 1));
            Assert.That(_List, Is.EquivalentTo(Enumerable.Range(42, 1).Concat(Enumerable.Range(1, count))));
        }

        [Test]
        public void Insert_item_at_the_end_of_the_list([Values(0, 7, 15, 17)] int count)
        {
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            _List.Insert(count, 42);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(count + 1));
            Assert.That(_List, Is.EquivalentTo(Enumerable.Range(1, count).Concat(Enumerable.Range(42, 1))));
        }

        [Test, Pairwise]
        public void Insert_item_at_the_middle_of_the_list(
            [Values(1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12)] int index,
            [Values(8, 9)] int nodeCapacity)
        {
            // Arrange
            const int count = 15;
            _List = TestUtils.GetUnrolledLinkedListWithItems(nodeCapacity, count);

            // Act
            _List.Insert(index, 42);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(count + 1));
            Assert.That(_List, Is.EquivalentTo(
                Enumerable.Range(1, index).
                    Concat(Enumerable.Range(42, 1)).
                    Concat(Enumerable.Range(index + 1, count - index))));
        }

        [Test]
        public void Insert_item_at_the_middle_of_the_list_when_node_is_not_full()
        {
            // Arrange
            const int count = 22;
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);
            _List.Remove(12);
            _List.Remove(14);

            // Act
            _List.Insert(12, 42);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(count - 2 + 1));
            var expectedResult = Enumerable.Range(1, 11).
                                    Concat(Enumerable.Range(13, 1)).
                                    Concat(Enumerable.Range(42, 1)).
                                    Concat(Enumerable.Range(15, 8));
            Assert.That(_List, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void Insert_updates_LastNode()
        {
            // Arrange
            const int count = 8;
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            _List.Insert(8, 42);
            _List.Add(11);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(count + 2));
            var expectedResult = Enumerable.Range(1, count).
                                    Concat(Enumerable.Range(42, 1)).
                                    Concat(Enumerable.Range(11, 1));
            Assert.That(_List, Is.EquivalentTo(expectedResult));
        }
    }
}
