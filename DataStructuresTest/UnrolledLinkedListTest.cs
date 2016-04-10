using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListTest
    {
        UnrolledLinkedList<int> _List;

        [SetUp]
        public void TestSetUp()
        {
            _List = new UnrolledLinkedList<int>();
        }

        [Test]
        public void Get_Enumerator_Test()
        {
            // Act
            var enumerator = _List.GetEnumerator();

            // Assert
            Assert.That(enumerator, Is.Not.Null);
        }

        [Test]
        public void Enumerator_Throws_Exception_For_Current_if_Not_Moved()
        {
            // Arrange
            var enumerator = _List.GetEnumerator();
            // Act
            Assert.That(
                delegate { var current = enumerator.Current; },
                Throws.InvalidOperationException);
        }

        [Test]
        public void Enumerator_MoveNext_Returns_False_After_At_The_End_Of_Iteration()
        {
            // Arrange
            _List.Add(5);
            var enumerator = _List.GetEnumerator();
            // Act
            enumerator.MoveNext();
            var canMove = enumerator.MoveNext();
            // Assert
            Assert.That(canMove, Is.False);
        }

        [Test]
        public void Element_is_added_to_UnrolledLinkedList()
        {
            // Act
            _List.Add(5);
            var enumerator = _List.GetEnumerator();
            enumerator.MoveNext();
            var item = enumerator.Current;
            // Assert
            Assert.That(item, Is.EqualTo(5));
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
        public void Enumerator_iterates_over_multiple_nodes()
        {
            const int seqLength = 5;
            // Arrange
            _List = new UnrolledLinkedList<int>(2);
            GenerateSequence(_List, seqLength);

            // Act
            var result = new int[seqLength];
            var i = 0;
            foreach (var item in _List)
            {
                result[i] = item;
                i++;
            }

            // Assert
            Assert.That(result, Is.EquivalentTo(Enumerable.Range(1,seqLength)));
        }


        private static void GenerateSequence(UnrolledLinkedList<int> list, int size)
        {
            int ceil_size = size + 1;
            for (int i = 1; i < ceil_size; ++i)
            {
                list.Add(i);
            }
        }
    }
}
