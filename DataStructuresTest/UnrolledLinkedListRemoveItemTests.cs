using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListRemoveItemTests
    {
        UnrolledLinkedListBuilder<int> listBuilder;

        [SetUp]
        public void Init()
        {
            listBuilder = new UnrolledLinkedListBuilder<int>();
        }

        [Test]
        public void Remove_non_existing_item_does_not_alter_the_collection()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(4).
                AddNodesFromCollection(Enumerable.Range(1, 8)).
                Build();

            // Act
            var result = list.Remove(9);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(list.Count, Is.EqualTo(8));
            Assert.That(list, Is.EquivalentTo(Enumerable.Range(1, 8)));
        }

        [Test]
        public void Removing_only_first_of_duplicate_items()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNode(new int[] { 5, 7, 9, 7, 8 }).
                Build();

            // Act
            list.Remove(7);

            // Assert
            Assert.That(list, Is.EquivalentTo(new[] { 5, 9, 7, 8 }));
        }

        [Test]
        public void Removing_only_first_of_duplicate_items_when_they_go_in_sequence()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(8).
                AddNode(new int[] { 5, 7, 7, 9, 8 }).
                Build();

            // Act
            list.Remove(7);

            // Assert
            Assert.That(list, Is.EquivalentTo(new[] { 5, 7, 9, 8 }));
        }

        [Test]
        public void Removing_item_equal_to_default_value_for_the_type()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(3).
                AddNode(new int[] { 3, 9, default(int) }).
                AddNode(new int[] { 4 }).
                Build();

            // Act
            list.Remove(default(int));

            // Assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list, Is.EquivalentTo(new[] { 3, 9, 4 }));
        }

        [Test]
        public void Remove_Item_From_UnrolledLinkedList_when_it_is_at_the_end_of_list()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(4).
                AddNodesFromCollection(Enumerable.Range(1,8)).
                Build();

            // Act
            bool result = list.Remove(8);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(list.Count, Is.EqualTo(7));
            Assert.That(list, Is.EquivalentTo(Enumerable.Range(1, 7)));
        }

        [Test]
        public void Remove_Item_From_UnrolledLinkedList_when_it_is_at_the_end_of_node()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(4).
                AddNodesFromCollection(Enumerable.Range(1, 8)).
                Build();

            // Act
            var result = list.Remove(4);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(list.Count, Is.EqualTo(7));
            var template = new int[] { 1, 2, 3, 5, 6, 7, 8 };
            Assert.That(list, Is.EquivalentTo(template));
            Assert.That(list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(
                list._FirstNode.data,
                Is.EquivalentTo(new int[] { 1, 2, 3, 0 }));
        }

        [Test]
        public void Remove_Item_UnrolledLinkedList_when_it_is_in_the_middle_of_node()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(4).
                AddNodesFromCollection(Enumerable.Range(1, 8)).
                Build();

            // Act
            bool result = list.Remove(3);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(list.Count, Is.EqualTo(7));
            var template = new int[] { 1, 2, 4, 5, 6, 7, 8 };
            Assert.That(list, Is.EquivalentTo(template));
            Assert.That(list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(
                list._FirstNode.data,
                Is.EquivalentTo(new int[] { 1, 2, 4, 0 }));
        }

        [Test]
        public void Remove_Item_UnrolledLinkedList_when_it_is_at_the_begining_of_node()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(4).
                AddNodesFromCollection(Enumerable.Range(1, 8)).
                Build();

            // Act
            bool result = list.Remove(5);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(list.Count, Is.EqualTo(7));
            var template = new int[] { 1, 2, 3, 4, 6, 7, 8 };
            Assert.That(list, Is.EquivalentTo(template));
            Assert.That(list._LastNode.Count, Is.EqualTo(3));
            Assert.That(
                list._LastNode.data,
                Is.EquivalentTo(new int[] { 6, 7, 8, 0 }));
        }

        [Test]
        public void Remove_all_items_from_the_last_node_deletes_it()
        {
            // Arrange
            UnrolledLinkedList<int> list =
                listBuilder.SetNodeCapacity(4).
                AddNode(new int[] { 1, 2, 3, 4}).
                AddNode(new int[] { 5 }).
                Build();

            // Act
            list.Remove(5);
            
            // Assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list._FirstNode, Is.EqualTo(list._LastNode));
            Assert.That(list._FirstNode.next, Is.Null);
        }
    }
}
