﻿using System;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListRemoveRangeTests
    {
        UnrolledLinkedList<int> _list;

        [Test]
        public void RemoveRange_throws_ArgumentOutOfRangeException_when_index_is_less_than_zero()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _list.RemoveRange(-1, 0));
        }

        [Test]
        public void RemoveRange_throws_ArgumentOutOfRangeException_when_count_is_less_than_zero()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _list.RemoveRange(4, -1));
        }

        [Test]
        public void RemoveRange_throws_ArgumentException_when_index_and_count_do_not_denote_a_valid_range_of_elements()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act Assert
            Assert.Throws<ArgumentException>(() => _list.RemoveRange(3, 12));
        }

        [Test]
        public void RemoveRange_applied_to_the_start_of_the_list_within_one_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act
            _list.RemoveRange(0, 2);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(4));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(2));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 3, 4, 0, 0 }));
        }

        [Test]
        public void RemoveRange_applied_to_the_start_of_the_list_removes_first_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act
            _list.RemoveRange(0, 4);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(2));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(2));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 5, 6, 0, 0 }));
        }

        [Test]
        public void RemoveRange_applied_to_the_start_of_the_list_spanning_multiple_nodes()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 12);

            // Act
            _list.RemoveRange(0, 9);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(3));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 10, 11, 12, 0 }));
        }

        [Test]
        public void RemoveRange_applied_to_the_start_of_the_list_causes_item_transfer()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act
            _list.RemoveRange(0, 3);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(3));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 4, 5, 6, 0}));
            Assert.That(_list._FirstNode, Is.EqualTo(_list._LastNode));
        }

        [Test]
        public void RemoveRange_applied_to_the_middle_of_the_node_and_within_one_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 6);

            // Act
            _list.RemoveRange(1, 2);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(4));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(2));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 1, 4, 0, 0 }));
        }

        [Test]
        public void RemoveRange_applied_to_the_middle_of_the_node_and_spans_multiple_nodes()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 15);

            // Act
            _list.RemoveRange(2, 8);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(7));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(2));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 1, 2, 0, 0 }));
            var next_node = _list._FirstNode.next;
            Assert.That(next_node.Count, Is.EqualTo(2));
            Assert.That(next_node.data, Is.EquivalentTo(new int[] { 11, 12, 0, 0 }));
        }

        [Test]
        public void RemoveRange_applied_to_the_middle_of_the_node_spans_multiple_nodes_and_causes_items_to_transfer()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 13);

            // Act
            _list.RemoveRange(2, 9);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(4));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(2));
            Assert.That(_list._FirstNode.data, Is.EquivalentTo(new int[] { 1, 2, 0, 0 }));
            var next_node = _list._FirstNode.next;
            Assert.That(next_node.Count, Is.EqualTo(2));
            Assert.That(next_node.data, Is.EquivalentTo(new int[] { 12, 13, 0, 0 }));
        }

        [Test]
        public void RemoveRange_causes_node_to_pull_items_from_more_than_one_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(6, 23);
            _list.RemoveRange(15, 3);
            _list.RemoveRange(9, 3);

            // Act
            _list.RemoveRange(7, 4);

            // Assert
            var second_node = _list._FirstNode.next;
            var third_node = second_node.next;
            Assert.That(_list.Count, Is.EqualTo(13));
            Assert.That(second_node.Count, Is.EqualTo(3));
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 7, 15, 19, 0, 0, 0}));
            Assert.That(third_node.Count, Is.EqualTo(4));
            Assert.That(third_node.data, Is.EquivalentTo(new int[] { 20, 21, 22, 23, 0, 0 }));
        }

        [Test]
        public void RemoveRange_causes_node_to_pull_items_from_more_than_one_node_and_changes_last_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(6, 21);
            _list.RemoveRange(15, 3);
            _list.RemoveRange(9, 3);

            // Act
            _list.RemoveRange(7, 4);

            // Assert
            var second_node = _list._FirstNode.next;
            var third_node = second_node.next;
            Assert.That(_list.Count, Is.EqualTo(11));
            Assert.That(second_node.Count, Is.EqualTo(5));
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 7, 15, 19, 20, 21, 0 }));
            Assert.That(_list._LastNode, Is.EqualTo(second_node));
        }

        [Test]
        public void RemoveRange_removes_full_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 11);
            var last_node_initial = _list._LastNode;

            // Act
            _list.RemoveRange(4, 4);

            // Assert
            var first_node = _list._FirstNode;
            var last_node = _list._LastNode;
            Assert.That(_list.Count, Is.EqualTo(7));
            Assert.That(first_node.next, Is.EqualTo(last_node));
            Assert.That(last_node.previous, Is.EqualTo(first_node));
            Assert.That(_list._LastNode, Is.EqualTo(last_node_initial));
        }

        [Test]
        public void RemoveRange_applied_to_end_of_the_list_updates_tail()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 15);

            // Act
            _list.RemoveRange(5, 10);

            // Assert
            var second_node = _list._FirstNode.next;
            Assert.That(_list.Count, Is.EqualTo(5));
            Assert.That(second_node.Count, Is.EqualTo(1));
            Assert.That(second_node.data, Is.EquivalentTo(new int[] { 5, 0, 0, 0 }));
            Assert.That(second_node.previous, Is.EqualTo(_list._FirstNode));
            Assert.That(_list._LastNode, Is.EqualTo(second_node));
        }

        [Test]
        public void RemoveRange_removes_all_items_from_the_list()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 15);

            // Act
            _list.RemoveRange(0, 15);

            // Assert
            Assert.That(_list.Count, Is.EqualTo(0));
            Assert.That(_list._FirstNode, Is.EqualTo(_list._LastNode));
            Assert.That(_list._FirstNode.Count, Is.EqualTo(0));
            Assert.That(_list._FirstNode.next, Is.Null);
            Assert.That(_list._FirstNode.previous, Is.Null);
        }
    }
}
