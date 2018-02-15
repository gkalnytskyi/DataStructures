using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListCopyToTests
    {
        UnrolledLinkedListBuilder<int> listBuilder;

        [SetUp]
        public void Init()
        {
            listBuilder = new UnrolledLinkedListBuilder<int>();
            listBuilder.SetNodeCapacity(4);
        }

        [Test]
        public void CopyTo_throws_exception_if_argument_array_is_null()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 9)).Build();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => list.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_throws_exception_if_index_is_less_than_zero()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 9)).Build();
            var array = new int[9];

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(array, -1));
        }

        [Test, Sequential]
        public void CopyTo_throws_exception_if_data_does_not_fit_into_array_starting_at_index(
            [Values(9, 5)] int arraySize,
            [Values(2, 0)] int startIndex)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 9)).Build();
            var array = new int[arraySize];

            // Act, Assert
            Assert.Throws<ArgumentException>(() => list.CopyTo(array, startIndex));
        }

        [Test]
        public void CopyTo_copies_items_to_array_starting_at_index_zero()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 6)).Build();
            var array = new int[10];

            // Act
            list.CopyTo(array, 0);

            // Assert
            var expectedCollection = new int[] { 1, 2, 3, 4, 5, 6, 0, 0, 0, 0 };
            Assert.That(array, Is.EquivalentTo(expectedCollection));
        }

        [Test]
        public void CopyTo_copies_items_to_array_starting_at_non_zero_index()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 6)).Build();
            var array = new int[10];

            // Act
            list.CopyTo(array, 2);

            // Assert
            var expectedCollection = new int[] { 0, 0, 1, 2, 3, 4, 5, 6, 0, 0 };
            Assert.That(array, Is.EquivalentTo(expectedCollection));
        }
    }
}
