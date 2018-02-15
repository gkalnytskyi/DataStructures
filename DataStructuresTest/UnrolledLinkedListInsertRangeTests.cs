using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListInsertRangeTests
    {
        UnrolledLinkedListBuilder<int> listBuilder;

        [SetUp]
        public void Init()
        {
            listBuilder = new UnrolledLinkedListBuilder<int>();
        }

        [Test]
        public void InsertRange_throws_IndexOutOfRangeException_for_invalid_index(
            [Values(-1, 9, 15)] int index)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNodesFromCollection(Enumerable.Range(1, 7)).
                Build();

            // Act Assert
            Assert.Throws<IndexOutOfRangeException>(() => list.InsertRange(index, Enumerable.Range(0, 5)));
        }

        [Test]
        public void InsertRange_throws_ArgumentNullException_for_null_collection()
        {
            // Arrange
            UnrolledLinkedList<int>  list =
                listBuilder.SetNodeCapacity(8).
                AddNodesFromCollection(Enumerable.Range(1, 4)).
                Build();
            // Act Assert
            Assert.Throws<ArgumentNullException>(() => list.InsertRange(4, null));
        }

        [Test, Sequential]
        public void InsertRange_inserts_items_in_correct_order_for_IEnumerable(
            [Values(0, 8, 8, 5, 5, 5, 8, 9)] int count,
            [Values(0, 8, 0, 2, 2, 2, 4, 4)] int index,
            [Values(3, 6, 6, 2, 3, 5, 13, 13)] int insertCollectionCount)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNodesFromCollection(Enumerable.Range(1, count)).
                Build();
            
            var newCollection = Enumerable.Range(0, insertCollectionCount).Select(x => 10 * x);

            // Act
            list.InsertRange(index, newCollection);

            // Assert
            var expectedCollection = Enumerable.Range(1, index).
                Concat(newCollection).
                Concat(Enumerable.Range(index + 1, count - index)).ToArray();
            Assert.That(list.Count, Is.EqualTo(count + insertCollectionCount));
            Assert.That(list, Is.EquivalentTo(expectedCollection));
        }

        [Test, Sequential]
        public void InsertRange_and_then_add_element_for_IEnumerable(
            [Values(0, 8, 8, 5, 8)] int count,
            [Values(0, 8, 0, 2, 7)] int index,
            [Values(3, 6, 6, 3, 3)] int insertCollectionCount)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNodesFromCollection(Enumerable.Range(1, count)).
                Build();

            var newCollection = Enumerable.Range(0, insertCollectionCount).Select(x => x * 10);

            // Act
            list.InsertRange(index, newCollection);
            list.Add(42);

            // Assert
            Assert.That(list.Count, Is.EqualTo(count + insertCollectionCount + 1));
            var expectedCollection = Enumerable.Range(1, index).
                Concat(newCollection).
                Concat(Enumerable.Range(index + 1, count - index)).
                Concat(new[] { 42 });
            Assert.That(list.Count, Is.EqualTo(count + insertCollectionCount + 1));
            Assert.That(list, Is.EquivalentTo(expectedCollection));
        }

        [Test, Sequential]
        public void InsertRange_inserts_items_in_correct_order_for_ICollection(
            [Values(0, 8, 8, 5, 5, 5, 8, 9)] int count,
            [Values(0, 8, 0, 2, 2, 2, 4, 4)] int index,
            [Values(3, 6, 6, 2, 3, 5, 13, 13)] int insertCollectionCount)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNodesFromCollection(Enumerable.Range(1, count)).
                Build();

            var newCollection = Enumerable.Range(0, insertCollectionCount).Select(x => x * 10).ToList();

            // Act
            list.InsertRange(index, newCollection);

            // Assert
            var expectedCollection = Enumerable.Range(1, index).
                Concat(newCollection).
                Concat(Enumerable.Range(index + 1, count - index)).ToArray();
            Assert.That(list.Count, Is.EqualTo(count + insertCollectionCount));
            Assert.That(list, Is.EquivalentTo(expectedCollection));
        }

        [Test, Sequential]
        public void InsertRange_and_then_add_element_for_ICollection(
            [Values(0, 8, 8, 5, 8)] int count,
            [Values(0, 8, 0, 2, 7)] int index,
            [Values(3, 6, 6, 3, 3)] int insertCollectionCount)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNodesFromCollection(Enumerable.Range(1, count)).
                Build();

            var newCollection = Enumerable.Range(0, insertCollectionCount).Select(x => x * 10).ToList();

            // Act
            list.InsertRange(index, newCollection);
            list.Add(42);

            // Assert
            var expectedCollection = Enumerable.Range(1, index).
                Concat(newCollection).
                Concat(Enumerable.Range(index + 1, count - index)).
                Concat(new[] { 42 }).ToArray();
            Assert.That(list.Count, Is.EqualTo(count + insertCollectionCount + 1));
            Assert.That(list, Is.EquivalentTo(expectedCollection));
        }
    }
}
