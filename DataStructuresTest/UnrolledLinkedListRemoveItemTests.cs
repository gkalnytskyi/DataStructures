﻿using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListRemoveItemTests
    {
        UnrolledLinkedList<int> _List;

        [Test]
        public void Remove_non_existing_item_does_not_change_the_collection()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            var result = _List.Remove(9);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_List.Count, Is.EqualTo(8));
            Assert.That(_List, Is.EquivalentTo(Enumerable.Range(1, 8)));
        }

        [Test]
        public void Removing_only_first_of_duplicate_items()
        {
            // Arrange
            _List = new UnrolledLinkedList<int>(8);
            _List.Add(5);
            _List.Add(7);
            _List.Add(9);
            _List.Add(7);
            _List.Add(8);

            // Act
            _List.Remove(7);

            // Assert
            Assert.That(_List, Is.EquivalentTo(new[] { 5, 9, 7, 8 }));
        }

        [Test]
        public void Removing_only_first_of_duplicate_items_when_they_go_in_sequence()
        {
            // Arrange
            _List = new UnrolledLinkedList<int>(8);
            _List.Add(5);
            _List.Add(7);
            _List.Add(7);
            _List.Add(9);
            _List.Add(8);

            // Act
            _List.Remove(7);

            // Assert
            Assert.That(_List, Is.EquivalentTo(new[] { 5, 7, 9, 8 }));
        }

        [Test]
        public void Removing_item_equal_to_default_value_for_the_type()
        {
            // Arrange
            _List = new UnrolledLinkedList<int>(3);
            _List.Add(3);
            _List.Add(9);
            _List.Add(1);
            _List.Add(default(int)); // It's 0
            _List.Add(4);

            // ensuring that there is a default element value
            // before actual default value we wish to delete
            _List.Remove(1);
            // Act
            _List.Remove(default(int));

            // Assert
            Assert.That(_List.Count, Is.EqualTo(3));
            Assert.That(_List, Is.EquivalentTo(new[] { 3, 9, 4 }));
        }

        [Test]
        public void Remove_Item_From_UnrolledLinkedList_when_it_is_at_the_end_of_list()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            bool result = _List.Remove(8);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_List.Count, Is.EqualTo(7));
            Assert.That(_List, Is.EquivalentTo(Enumerable.Range(1, 7)));
        }

        [Test]
        public void Remove_Item_From_UnrolledLinkedList_when_it_is_at_the_end_of_node()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            var result = _List.Remove(4);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_List.Count, Is.EqualTo(7));
            var template = Enumerable.Range(1, 3).Concat(Enumerable.Range(5, 4));
            Assert.That(_List, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_Item_UnrolledLinkedList_when_it_is_in_the_middle_of_node()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            bool result = _List.Remove(3);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_List.Count, Is.EqualTo(7));
            var template = Enumerable.Range(1, 2).Concat(Enumerable.Range(4, 5));
            Assert.That(_List, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_Item_UnrolledLinkedList_when_it_is_at_the_begining_of_node()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            bool result = _List.Remove(5);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_List.Count, Is.EqualTo(7));
            var template = Enumerable.Range(1, 4).Concat(Enumerable.Range(6, 3));
            Assert.That(_List, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_very_last_item_from_the_node([Values(1, 9)] int count)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

            // Act
            bool result = _List.Remove(count);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_List.Count, Is.EqualTo(count - 1));
            var expectedResult = Enumerable.Range(1, count - 1);
            Assert.That(_List, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void Remove_Several_Consequent_Items_from_UnrolledLinkedList()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 8);

            // Act
            _List.Remove(2);
            _List.Remove(3);
            _List.Remove(4);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(5));
            var template = Enumerable.Range(1, 1).Concat(Enumerable.Range(5, 4));
            Assert.That(_List, Is.EquivalentTo(template));
        }

        [Test]
        public void Remove_all_items_of_the_node_deletes_it()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 5);

            // Act
            _List.Remove(5);
            var enumerator = _List.GetEnumerator();
            for (var i = 0; i < 4; ++i)
            {
                enumerator.MoveNext();
            }

            var result = enumerator.MoveNext();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Remove_and_add_element_at_the_end_of_the_list()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 9);

            // Act
            _List.Remove(9);
            _List.Add(10);

            // Assert
            var expectedCollection = Enumerable.Range(1, 8).Concat(Enumerable.Range(10, 1));
            Assert.That(_List, Is.EquivalentTo(expectedCollection));
        }
    }
}
