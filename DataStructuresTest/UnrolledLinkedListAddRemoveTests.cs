using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    class UnrolledLinkedListAddRemoveTests
    {
        UnrolledLinkedListBuilder<int> listBuilder;
        [SetUp]
        public void Init()
        {
            listBuilder = new UnrolledLinkedListBuilder<int>();
            listBuilder.SetNodeCapacity(4);
        }

        [Test]
        public void Creating_an_empty_list_with_specified_node_capacity(
            [Values(2,3,5,8,9,16)] int capacity
            )
        {
            // Act
            UnrolledLinkedList<int> _list = new UnrolledLinkedList<int>(capacity);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(0));
            Assert.That(_list._FirstNode, Is.EqualTo(_list._LastNode));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(0));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[capacity]));
            Assert.That(_list._FirstNode.data.Length, Is.EqualTo(capacity));
            Assert.That(_list._FirstNode.next, Is.Null);
            Assert.That(_list._FirstNode.previous, Is.Null);
        }

        public void Creating_list_with_invalid_capacity_throws_exception(
            [Values(0,-1,-100)] int capacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new UnrolledLinkedList<int>(capacity));
        }

        [Test]
        public void Adding_an_item_to_an_empty_list()
        {
            UnrolledLinkedList<int> _list = new UnrolledLinkedList<int>(8);

            _list.Add(42);

            Assert.That(_list.Count, Is.EqualTo(1));
            Assert.That(_list, Is.EquivalentTo(new int[] { 42 }));
            Assert.That(_list._FirstNode, Is.EqualTo(_list._LastNode));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(1));
            Assert.That(_list._FirstNode.data, Is.EqualTo(new int[8] { 42, 0, 0, 0, 0, 0, 0, 0 }));
            Assert.That(_list._FirstNode.next, Is.Null);
            Assert.That(_list._FirstNode.previous, Is.Null);
        }

        [Test]
        public void Adding_Multiple_Items_into_the_list_Sequentially()
        {
            // Arrange
            UnrolledLinkedList<int> _list = new UnrolledLinkedList<int>(4);

            // Act
            _list.Add(1);
            _list.Add(2);
            _list.Add(3);
            _list.Add(4);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(4));
            Assert.That(_list, Is.EquivalentTo(new int[] { 1, 2, 3, 4 }));
            Assert.That(_list._FirstNode, Is.EqualTo(_list._LastNode));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(4));
            Assert.That(_list._FirstNode.data, Is.EqualTo(new int[4] { 1, 2, 3, 4 }));
            Assert.That(_list._FirstNode.next, Is.Null);
            Assert.That(_list._FirstNode.previous, Is.Null);
        }

        [Test]
        public void Added_items_overflow_to_the_next_created_node()
        {
            // Arrange
            UnrolledLinkedList<int> list = listBuilder.AddNodes(new int[] { 1, 2, 3, 4 }).Build();
            // Act
            list.Add(5);
            // Assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list, Is.EquivalentTo(new int[] { 1, 2, 3, 4, 5 }));
            Assert.That(list._FirstNode, Is.Not.EqualTo(list._LastNode));
            Assert.That(list._LastNode, Is.EqualTo(list._FirstNode.next));
            Assert.That(list._FirstNode.previous, Is.Null);
            Assert.That(list._LastNode.Count, Is.EqualTo(1));
            Assert.That(list._LastNode.data, Is.EquivalentTo(new int[] { 5, 0, 0, 0 }));
            Assert.That(list._LastNode.next, Is.Null);
        }

        [Test]
        public void Clearing_list_removes_all_items()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1,10)).Build();

            // Act
            list.Clear();

            // Assert
            Assert.That(list.Count, Is.EqualTo(0));
            Assert.That(list.GetEnumerator().MoveNext(), Is.False);
            Assert.That(list, Is.Empty);
            Assert.That(list._FirstNode, Is.EqualTo(list._LastNode));
            Assert.That(list._FirstNode.previous, Is.Null);
            Assert.That(list._FirstNode.next, Is.Null);
            Assert.That(list._FirstNode.data, Is.EquivalentTo(new int[4]));
            Assert.That(list._FirstNode.Count, Is.EqualTo(0));
        }


        [Test]
        public void Contains_returns_false_when_element_is_not_found()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 9)).Build();

            // Act
            bool result = list.Contains(51);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_returns_true_if_element_is_found()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 9)).Build();

            // Act
            bool result = list.Contains(7);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test, Sequential]
        public void RemoveAt_throws_IndexOutOfRangeException_for_invalid_index(
            [Values(-1, 18, -100, 13, 15, 0)] int index,
            [Values(7, 15, 6, 11, 15, 0)] int count)
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.
                    SetNodeCapacity(8).
                    AddNodesFromCollection(Enumerable.Range(1, count)).
                    Build();

            // Act, Assert
            Assert.Throws<IndexOutOfRangeException>(() => list.RemoveAt(index));
        }

        [Test]
        public void RemoveAt_item_from_the_begining_of_the_list()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 4)).Build();

            // Act
            list.RemoveAt(0);

            // Assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list._FirstNode, Is.EqualTo(list._LastNode));
            Assert.That(list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(list._FirstNode.data, Is.EquivalentTo(new[] { 2, 3, 4, 0 }));
        }

        [Test]
        public void RemoveAt_item_from_the_begining_of_the_node()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 10)).Build();

            // Act
            list.RemoveAt(4);
            var second_node = list._FirstNode.next;

            // Assert
            var expected_list = Enumerable.Range(1, 4).
                Concat(Enumerable.Range(6, 3)).
                Concat(Enumerable.Range(9, 2));
            Assert.That(list.Count, Is.EqualTo(9));
            Assert.That(second_node.Count, Is.EqualTo(3));
            Assert.That(second_node.data, Is.EquivalentTo(new[] { 6, 7, 8, 0 }));
            Assert.That(second_node.Count, Is.EqualTo(3));
            Assert.That(list, Is.EquivalentTo(expected_list));
        }

        [Test]
        public void RemoveAt_item_from_the_list_by_index(
            [Values(0, 1, 7, 8, 9, 10, 12)] int index)
        {
            // Arrange
            const int count = 15;
            UnrolledLinkedList<int> _list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, count)).Build();

            // Act
            _list.RemoveAt(index);

            // Assert
            var expectedResult = Enumerable.Range(1, count).
                                    Except(Enumerable.Range(index + 1, 1));
            Assert.That(_list.Count, Is.EqualTo(count - 1));
            Assert.That(_list, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void RemoveAt_element_from_the_last_node_with_one_item()
        {
            // Arrange
            UnrolledLinkedList<int> _list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 9)).Build();

            // Act
            var second_node = _list._FirstNode.next;
            _list.RemoveAt(8);

            // Assert
            var expectedCollection = Enumerable.Range(1, 8);
            Assert.That(second_node.next, Is.Null);
            Assert.That(_list._LastNode, Is.EqualTo(second_node));
            Assert.That(_list, Is.EquivalentTo(expectedCollection));
        }

        [Test]
        public void RemoveAt_on_half_full_node_causes_item_transfer()
        {
            // Arrange
            UnrolledLinkedList<int> _list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 12)).Build();

            for (var i = 0; i < 2; i++)
            {
                _list.RemoveAt(4);
            }

            // Act
            _list.RemoveAt(4);
            var second_node = _list._FirstNode.next;

            // Assert
            Assert.That(_list.Count, Is.EqualTo(9));
            Assert.That(second_node.Count, Is.EqualTo(2));
            Assert.That(second_node.data, Is.EquivalentTo(new[] { 8, 9, 0, 0 }));
            Assert.That(second_node.next, Is.Not.Null);
            Assert.That(second_node.next.Count, Is.EqualTo(3));
            Assert.That(second_node.next.data, Is.EquivalentTo(new[] { 10, 11, 12, 0 }));
        }

        [Test]
        public void RemoveAt_on_half_full_node_with_enough_capacity_cuts_away_last_node()
        {
            // Arrange
            UnrolledLinkedList<int> _list =
                listBuilder.AddNodesFromCollection(Enumerable.Range(1, 10)).Build();

            for (var i = 0; i < 2; i++)
            {
                _list.RemoveAt(4);
            }

            // Act
            _list.RemoveAt(4);
            var second_node = _list._FirstNode.next;

            // Assert
            Assert.That(_list.Count, Is.EqualTo(7));
            Assert.That(second_node.Count, Is.EqualTo(3));
            Assert.That(second_node.data, Is.EquivalentTo(new[] { 8, 9, 10, 0 }));
            Assert.That(second_node.next, Is.Null);
            Assert.That(_list._LastNode, Is.EqualTo(second_node));
        }
    }
}
