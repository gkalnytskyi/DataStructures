using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListInsertItemTests
    {
        UnrolledLinkedList<int> _list;

        [Test]
        public void Insert_Item_to_the_begining_of_the_list(
            [Values(0, 7)] int count)
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            _list.Insert(0, 42);

            // Assert
            var expected_list = Enumerable.Range(42, 1).
                Concat(Enumerable.Range(1, count)).ToArray();
            var expected_array = new int[8];
            Array.Copy(expected_list, expected_array, expected_list.Length);
            Assert.That(_list.Count, Is.EqualTo(count + 1));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(count + 1));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(expected_array));
        }

        [Test]
        public void Insert_item_at_the_end_of_the_list()
        {
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 7);

            // Act
            _list.Insert(7, 42);

            // Assert
            var expected_list = Enumerable.Range(1, 7).Concat(Enumerable.Range(42, 1));
            var expected_array = new int[] { 5, 6, 7, 42 };
            Assert.That(_list.Count, Is.EqualTo(8));
            Assert.That(_list, Is.EquivalentTo(expected_list));
            Assert.That(_list._LastNode.Count, Is.EqualTo(4));
            Assert.That(_list._LastNode.data, Is.EquivalentTo(expected_array));
        }

        [Test]
        public void Insert_item_at_the_end_of_list_with_full_last_node_appends_new_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 8);
            var second_node = _list._LastNode;

            // Act
            _list.Insert(8, 42);

            // Assert
            var expected_list = Enumerable.Range(1, 8).Concat(Enumerable.Range(42, 1));
            var expected_array = new int[] { 42, 0, 0, 0 };
            Assert.That(_list.Count, Is.EqualTo(9));
            Assert.That(_list, Is.EquivalentTo(expected_list));
            Assert.That(second_node.next, Is.EqualTo(_list._LastNode));
            Assert.That(_list._LastNode.previous, Is.EqualTo(second_node));
            Assert.That(_list._LastNode.Count, Is.EqualTo(1));
            Assert.That(_list._LastNode.data, Is.EquivalentTo(expected_array));
        }

        [Test]
        public void Insert_item_at_the_middle_of_the_list_to_not_full_node()
        {
            // Arrange
            const int count = 11;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._FirstNode.next;
            _list.RemoveAt(7);

            // Act
            _list.Insert(4, 42);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(count));
            Assert.That(second_node.Count, Is.EqualTo(4));
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 42, 5, 6, 7}));
        }

        [Test]
        public void Insert_item_at_the_middle_of_the_last_not_full_node()
        {
            // Arrange
            const int count = 7;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._LastNode;

            // Act
            _list.Insert(4, 42);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(count + 1));
            Assert.That(second_node.Count, Is.EqualTo(4));
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 42, 5, 6, 7 }));
            Assert.That(second_node.next, Is.Null);
            Assert.That(_list._LastNode, Is.EqualTo(second_node));
        }

        [Test]
        public void Insert_item_to_full_node_in_the_middle_of_the_list_causes_it_to_split(
            [Values(5,6)] int index)
        {
            // Arrange
            const int count = 10;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._FirstNode.next;

            // Act
            _list.Insert(index, 42);

            // Assert
            var new_node = second_node.next;
            Assert.That(_list.Count, Is.EqualTo(count + 1));
            Assert.That(second_node.Count, Is.EqualTo(3));
            Assert.That(new_node.Count, Is.EqualTo(2));
            Assert.That(new_node.previous, Is.EqualTo(second_node));
            Assert.That(new_node.next, Is.EqualTo(_list._LastNode));
            Assert.That(_list._LastNode.previous, Is.EqualTo(new_node));
        }

        [Test]
        public void Insert_item_at_full_node_before_mid_index_in_the_middle_of_the_list()
        {
            // Arrange
            const int count = 10;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._FirstNode.next;

            // Act
            _list.Insert(5, 42);

            // Assert
            var new_node = second_node.next;
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 5, 42, 6, 0 }));
            Assert.That(new_node.data, Is.EquivalentTo(new int[] { 7, 8, 0, 0 }));
        }

        [Test]
        public void Insert_item_at_full_node_after_mid_index_in_the_middle_of_the_list()
        {
            // Arrange
            const int count = 10;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._FirstNode.next;

            // Act
            _list.Insert(6, 42);

            // Assert
            var new_node = second_node.next;
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 5, 6, 42, 0 }));
            Assert.That(new_node.data, Is.EquivalentTo(new int[] { 7, 8, 0, 0 }));
        }

        [Test]
        public void Insert_item_to_full_last_node_causes_it_to_split(
            [Values(5, 6)] int index)
        {
            // Arrange
            const int count = 8;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._LastNode;

            // Act
            _list.Insert(index, 42);

            // Assert
            var new_node = second_node.next;
            Assert.That(_list.Count, Is.EqualTo(count + 1));
            Assert.That(second_node.Count, Is.EqualTo(3));
            Assert.That(new_node.Count, Is.EqualTo(2));
            Assert.That(new_node.previous, Is.EqualTo(second_node));
            Assert.That(new_node.next, Is.Null);
            Assert.That(_list._LastNode, Is.EqualTo(new_node));
        }

        [Test]
        public void Insert_item_to_full_last_node_before_mid_index()
        {
            // Arrange
            const int count = 8;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._LastNode;

            // Act
            _list.Insert(5, 42);

            // Assert
            var new_node = second_node.next;
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 5, 42, 6, 0 }));
            Assert.That(new_node.data, Is.EquivalentTo(new int[] { 7, 8, 0, 0 }));
        }

        [Test]
        public void Insert_item_to_full_last_node_after_mid_index()
        {
            // Arrange
            const int count = 8;
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, count);
            var second_node = _list._LastNode;

            // Act
            _list.Insert(6, 42);

            // Assert
            var new_node = second_node.next;
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 5, 6, 42, 0 }));
            Assert.That(new_node.data, Is.EquivalentTo(new int[] { 7, 8, 0, 0 }));
        }

        [Test]
        public void Insert_at_the_begining_of_list_to_full_node_causes_it_to_split()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 4);

            // Act
            _list.Insert(0, 42);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(5));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new[] { 42, 1, 2, 0 }));
            var second_node = _list._FirstNode.next;
            Assert.That(second_node.Count, Is.EqualTo(2));
            Assert.That(second_node.data, Is.EquivalentTo(new[] { 3, 4, 0, 0 }));
            Assert.That(second_node.previous, Is.EqualTo(_list._FirstNode));
            Assert.That(_list._LastNode, Is.EqualTo(second_node));
        }

        [Test]
        public void Insert_at_the_begining_of_list_to_full_node_causes_it_to_split_and_tail_updated()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 5);

            // Act
            _list.Insert(0, 42);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(6));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new[] { 42, 1, 2, 0 }));
            var second_node = _list._FirstNode.next;
            Assert.That(second_node.Count, Is.EqualTo(2));
            Assert.That(second_node.data, Is.EquivalentTo(new[] { 3, 4, 0, 0 }));
            Assert.That(second_node.next.previous, Is.EqualTo(second_node));
            Assert.That(second_node.previous, Is.EqualTo(_list._FirstNode));
            Assert.That(second_node.next.Count, Is.EqualTo(1));
            Assert.That(second_node.next.data, Is.EquivalentTo(new[] { 5, 0, 0, 0 }));
            Assert.That(second_node.next, Is.EqualTo(_list._LastNode));
        }
    }
}
