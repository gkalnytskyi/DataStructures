﻿using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListRemoveItemTests
    {
        UnrolledLinkedList<int> _list;

        [Test]
        public void Remove_non_existing_item_does_not_change_the_collection()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            var result = _list.Remove(9);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_list.Count, Is.EqualTo(8));
            Assert.That(_list, Is.EquivalentTo(Enumerable.Range(1, 8)));
        }

        [Test]
        public void Removing_only_first_of_duplicate_items()
        {
            // Arrange
            _list = new UnrolledLinkedList<int>(8);
            _list.Add(5);
            _list.Add(7);
            _list.Add(9);
            _list.Add(7);
            _list.Add(8);

            // Act
            _list.Remove(7);

            // Assert
            Assert.That(_list, Is.EquivalentTo(new[] { 5, 9, 7, 8 }));
        }

        [Test]
        public void Removing_only_first_of_duplicate_items_when_they_go_in_sequence()
        {
            // Arrange
            _list = new UnrolledLinkedList<int>(8);
            _list.Add(5);
            _list.Add(7);
            _list.Add(7);
            _list.Add(9);
            _list.Add(8);

            // Act
            _list.Remove(7);

            // Assert
            Assert.That(_list, Is.EquivalentTo(new[] { 5, 7, 9, 8 }));
        }

        [Test]
        public void Removing_item_equal_to_default_value_for_the_type()
        {
            // Arrange
            _list = new UnrolledLinkedList<int>(3);
            _list.Add(3);
            _list.Add(9);
            _list.Add(1);
            _list.Add(default(int)); // It's 0
            _list.Add(4);

            // ensuring that there is a default element value
            // before actual default value we wish to delete
            _list.Remove(1);
            // Act
            _list.Remove(default(int));

            // Assert
            Assert.That(_list.Count, Is.EqualTo(3));
            Assert.That(_list, Is.EquivalentTo(new[] { 3, 9, 4 }));
        }

        [Test]
        public void Remove_Item_From_UnrolledLinkedList_when_it_is_at_the_end_of_list()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            bool result = _list.Remove(8);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_list.Count, Is.EqualTo(7));
            Assert.That(_list, Is.EquivalentTo(Enumerable.Range(1, 7)));
        }

        [Test]
        public void Remove_Item_From_UnrolledLinkedList_when_it_is_at_the_end_of_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            var result = _list.Remove(4);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_list.Count, Is.EqualTo(7));
            var template = Enumerable.Range(1, 3).Concat(Enumerable.Range(5, 4));
            Assert.That(_list, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_Item_UnrolledLinkedList_when_it_is_in_the_middle_of_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            bool result = _list.Remove(3);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_list.Count, Is.EqualTo(7));
            var template = Enumerable.Range(1, 2).Concat(Enumerable.Range(4, 5));
            Assert.That(_list, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_Item_UnrolledLinkedList_when_it_is_at_the_begining_of_node()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            bool result = _list.Remove(5);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_list.Count, Is.EqualTo(7));
            var template = Enumerable.Range(1, 4).Concat(Enumerable.Range(6, 3));
            Assert.That(_list, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_all_items_from_the_last_node_deletes_it()
        {
            // Arrange
            _list = TestUtils.GetUnrolledLinkedListWithItems(4, 5);

            // Act
            _list.Remove(5);
            
            // Assert
            Assert.That(_list.Count, Is.EqualTo(4));
            Assert.That(_list._FirstNode, Is.EqualTo(_list._LastNode));
            Assert.That(_list._FirstNode.next, Is.Null);
        }
    }
}
