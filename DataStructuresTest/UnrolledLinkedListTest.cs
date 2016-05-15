using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    class UnrolledLinkedListAddRemoveTests
    {
        UnrolledLinkedList<int> _List;

        [Test]
        public void Adding_and_Reading_one_item()
        {
            // Arrange
            _List = new UnrolledLinkedList<int>(8);

            // Act
            _List.Add(1);
            var enumerator = _List.GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Adding_and_Reading_Multiple_items_to_List_Sequentially()
        {
            // Arrange
            _List = new UnrolledLinkedList<int>(8);

            // Act
            _List.Add(1);
            _List.Add(2);
            _List.Add(3);
            _List.Add(4);
            _List.Add(5);

            var result = new int[5];
            var i = 0;
            foreach (var item in _List)
            {
                result[i] = item;
                i++;
            }

            // Assert
            Assert.That(result, Is.EquivalentTo(new int[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void Clearing_Unrolled_Linked_List()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 10);

            // Act
            _List.Clear();

            // Assert
            Assert.That(_List.Count, Is.EqualTo(0));
            Assert.That(_List.GetEnumerator().MoveNext(), Is.False);
        }


        [Test]
        public void Contains_does_not_find_element_when_it_is_not_in_the_collection()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);

            // Act
            bool result = _List.Contains(51);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_finds_element_when_it_is_in_the_collection()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);

            // Act
            bool result = _List.Contains(7);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test, Sequential]
        public void RemoveAt_throws_IndexOutOfRangeException_for_invalid_index(
            [Values(-1, 18, -100, 13, 15, 0)] int index,
            [Values(7, 15, 6, 11, 15, 0)] int count)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act, Assert
            Assert.Throws<IndexOutOfRangeException>(() => _List.RemoveAt(index));
        }

        [Test]
        public void RemoveAt_item_from_the_list_by_index(
            [Values(0, 1, 7, 8, 9, 10, 12)] int index)
        {
            // Arrange
            const int count = 15;
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            _List.RemoveAt(index);

            // Assert
            var expectedResult = Enumerable.Range(1, count).
                                    Except(Enumerable.Range(index + 1, 1));
            Assert.That(_List.Count, Is.EqualTo(count - 1));
            Assert.That(_List, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void Remove_item_from_list_by_index_from_not_full_node(
            [Values(8, 9)] int nodeCapacity)
        {
            // Arrange
            const int count = 15;
            _List = TestUtils.GetUnrolledLinkedListWithItems(nodeCapacity, count);
            int initiallyRemoved = nodeCapacity / 2;
            for (int i = 1; i < initiallyRemoved + 1; ++i)
            {
                _List.Remove(i);
            }

            // Act
            _List.RemoveAt(2);

            // Assert
            var expectedResult = Enumerable.Range(5, 2).Concat(Enumerable.Range(8, count - 7));
            Assert.That(_List.Count, Is.EqualTo(count - initiallyRemoved - 1));
            Assert.That(_List, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void RemoveAt_and_add_element_at_the_end_of_the_list()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 9);

            // Act
            for (int i = 0; i < 5; i++)
            {
                _List.RemoveAt(3);
            }
            _List.AddLast(10);

            // Assert
            var expectedCollection = Enumerable.Range(1, 3).
                Concat(Enumerable.Range(9, 2));
            Assert.That(_List, Is.EquivalentTo(expectedCollection));
        }
    }
}
