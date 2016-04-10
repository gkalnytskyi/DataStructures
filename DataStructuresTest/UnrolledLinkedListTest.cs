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
    }
}
