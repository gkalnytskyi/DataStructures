using System;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    class UnrolledLinkedListTest
    {
        UnrolledLinkedList<int> _List;

        [SetUp]
        public void TestSetUp()
        {
            _List = new UnrolledLinkedList<int>(8);
        }

        [Test]
        public void Adding_and_Reading_one_item()
        {
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
            TestUtils.GenerateSequence(_List, 10);

            // Act
            _List.Clear();

            // Assert
            Assert.That(_List.Count, Is.EqualTo(0));
            Assert.That(_List.GetEnumerator().MoveNext(), Is.False);
        }

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
            var template = Enumerable.Range(1, 3).Concat(Enumerable.Range(5,4));
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

        [Test]
        public void CopyTo_throws_exception_if_argument_array_is_null()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => _List.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_throws_exception_if_index_is_less_than_zero()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);
            var array = new int[9];

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _List.CopyTo(array, -1));
        }

        [Test, Sequential]
        public void CopyTo_throws_exception_if_data_does_not_fit_into_array_starting_at_index(
            [Values(9, 5)] int arraySize, [Values(2, 0)] int startIndex)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 9);
            var array = new int[arraySize];

            // Act, Assert
            Assert.Throws<ArgumentException>(() => _List.CopyTo(array, startIndex));
        }

        [Test]
        public void CopyTo_copies_items_to_array_starting_at_index_zero()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 6);
            var array = new int[10];

            // Act
            _List.CopyTo(array, 0);

            // Assert
            var template = Enumerable.Range(1, 6).Concat(Enumerable.Repeat(0, 4));
            Assert.That(array, Is.EquivalentTo(template));
        }

        [Test]
        public void CopyTo_copies_items_to_array_starting_at_non_zero_index()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(4, 6);
            var array = new int[10];

            // Act
            _List.CopyTo(array, 2);

            // Assert
            var template = Enumerable.Repeat(0, 2).Concat(Enumerable.Range(1, 6)).Concat(Enumerable.Repeat(0, 2));
            Assert.That(array, Is.EquivalentTo(template));
        }

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
            [Values(0,2,5,8)] int index,
            [Values(1,3,6,9)] int result)
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
        public void Insert_throws_ArgumentOutOfRangeException_for_invalid_index(
            [Values(-1, 18, -100, 13)] int index,
            [Values(7, 15, 6, 11)] int listCount)
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, listCount);

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _List.Insert(index, 42));
        }


        [Test]
        public void Insert_item_at_the_begining_of_list()
        {
            // Arrange
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, 17);

            // Act
            _List.Insert(0, 42);

            // Assert
            Assert.That(_List.Count, Is.EqualTo(18));
            Assert.That(_List, Is.EquivalentTo(Enumerable.Range(42, 1).Concat(Enumerable.Range(1, 17))));
        }

        [Test]
        public void Insert_item_at_position_Count([Values(0, 7, 15, 17)] int count)
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
            _List = TestUtils.GetUnrolledLinkedListWithItems(8, count);

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
    }
}
