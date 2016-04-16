using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public partial class UnrolledLinkedList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        private readonly int _NodeCapasity;
        private UnrolledLinkedListNode<T> _FirstNode;
        private UnrolledLinkedListNode<T> _LastNode;

        public UnrolledLinkedList() : this(16) { }

        public UnrolledLinkedList(int nodeCapasity)
        {
            if (nodeCapasity < 1)
            {
                throw new ArgumentOutOfRangeException("NodeCapasity cannot be less than 1");
            }
            _NodeCapasity = nodeCapasity;
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(this._NodeCapasity);
            Count = 0;
        }

        #region ICollection<T> Properties
        public int Count { get; private set; }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region IList<T> Properties
        public T this[int index]
        {
            get
            {
                CheckIndex(index);
                var node = FindNodeAndIndex(ref index);
                return node.Data[index];
            }

            set
            {
                CheckIndex(index);
                var node = FindNodeAndIndex(ref index);
                node.Data[index] = value;
            }
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index > (Count - 1))
                throw new IndexOutOfRangeException();
        }

        private UnrolledLinkedListNode<T> FindNodeAndIndex(ref int index)
        {
            var currentNode = _FirstNode;
            do
            {
                if (index < currentNode.Count)
                {
                    break;
                }
                index -= currentNode.Count;
                currentNode = currentNode.Next;
            }
            while (currentNode != null);
            return currentNode;
        }
        #endregion

        #region IEnumerable<T> Methods
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion IEnumerable<T> Methods

        #region ICollection<T> Methods
        /// <summary>
        /// Adds an item to the end of the collection
        /// <param name="item"></param>
        public void Add(T item)
        {
            AddLast(item);
        }

        private void AddLast(T item)
        {
            if (!_LastNode.TryAddItem(item))
            {
                var newNode = new UnrolledLinkedListNode<T>(_NodeCapasity, _LastNode, item);
                _LastNode.Next = newNode;
                _LastNode = newNode;
            }
            Count += 1;
        }

        /// <summary>
        /// Removes the first occurrence of the item in the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return RemoveFirst(item);
        }

        private bool RemoveFirst(T item)
        {
            var found = false;
            var currentNode = _FirstNode;

            do
            {
                var index_found = currentNode.IndexOf(item);
                if (index_found > -1)
                {
                    found = true;
                    currentNode.RemoveItem(index_found);
                    Count -= 1;
                    if (currentNode.IsEmpty())
                    {
                        var prevNode = currentNode.Previous;
                        var nextNode = currentNode.Next;
                        if (nextNode != null)
                        {
                            nextNode.Previous = prevNode;
                        }
                        if (prevNode != null)
                        {
                            prevNode.Next = nextNode;
                        }
                    }
                    break;
                }
                currentNode = currentNode.Next;
            }
            while ((currentNode != null));

            return found;
        }

        /// <summary>
        /// Removes all items from the collection
        /// </summary>
        public void Clear()
        {
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(_NodeCapasity);
            Count = 0;
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            bool found = false;
            var currentNode = _FirstNode;
            do
            {
                found = currentNode.Contains(item);
                currentNode = currentNode.Next;
            }
            while (!found && currentNode != null);
            return found;
        }

        /// <summary>
        /// Copies the elements of the collection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Parameter 'array' cannot be null");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("Parameter 'arrayIndex' cannot be negative");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException(
                    "Data will not fit into destination 'array' " +
                    "starting from specified index. " + 
                    "{\"array.Length\" : {{{0}}}, \"arrayIndex\" : {{{1}}}}");
            }

            var currentNode = _FirstNode;
            do
            {
                int count = currentNode.CopyTo(array, arrayIndex);
                arrayIndex += count;
                currentNode = currentNode.Next;
            }
            while (currentNode != null);
        }
        #endregion ICollection<T> Methods

        #region IList<T> Methods
        public int IndexOf(T item)
        {
            var currentNode = _FirstNode;
            int index = 0;
            bool found = false;
            do
            {
                var i = currentNode.IndexOf(item);
                if (i < 0)
                {
                    index += currentNode.Count;
                }
                else
                {
                    found = true;
                    index += i;
                }
                currentNode = currentNode.Next;
            }
            while (currentNode != null && !found);
            return (found) ? (index) : (-1);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
        #endregion IList<T> Methods

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }
    }
}
