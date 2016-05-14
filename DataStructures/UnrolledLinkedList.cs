using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public partial class UnrolledLinkedList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        public const int NODE_CAPACITY = 16;
        private readonly int _NodeCapacity;
        private int _Count = 0;
        private UnrolledLinkedListNode<T> _FirstNode;
        private UnrolledLinkedListNode<T> _LastNode;


        #region Constructors
        public UnrolledLinkedList() : this(NODE_CAPACITY) { }

        public UnrolledLinkedList(int nodeCapacity)
        {
            if (nodeCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("NodeCapacity cannot be less than 1");
            }
            _NodeCapacity = nodeCapacity;
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
            _Count = 0;
        }

        public UnrolledLinkedList(
            int nodeCapacity,
            IEnumerable<T> collection) : this(nodeCapacity)
        {
            AddRange(collection);
        }
        #endregion Constructors


        #region ICollection<T> Properties
        public int Count { get { return _Count; } }

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
                CheckIndex(index, _Count - 1);
                var node = FindNodeAndIndex(ref index);
                return node.Data[index];
            }

            set
            {
                CheckIndex(index, _Count - 1);
                var node = FindNodeAndIndex(ref index);
                node.Data[index] = value;
            }
        }

        private void CheckIndex(int index, int maxValue)
        {
            if (index < 0 || index > maxValue)
                throw new IndexOutOfRangeException(
                      string.Format("Index cannot be less than 0, " +
                                    "or greater than Count: {0}", maxValue));
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


        #region Public Non-Interface Methods
        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }

        public void AddFirst(T item)
        {
            _FirstNode.Insert(0, item);
            _Count++;
        }

        public void AddLast(T item)
        {
            _LastNode.Add(item);
            _Count++;
            FindNewLastNode();
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection cannot be null");
            }

            CheckIndex(index, _Count);

            int nodeIndex = index;
            UnrolledLinkedListNode<T> startNode = FindNodeAndIndex(ref nodeIndex);
            _Count += startNode.InsertRange(nodeIndex, collection);
            FindNewLastNode();
        }
        #endregion Public Non-Interface Methods


        #region ICollection<T> Methods
        /// <summary>
        /// Adds an item to the end of the collection
        /// <param name="item"></param>
        public void Add(T item)
        {
            AddLast(item);
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
                    currentNode.RemoveAt(index_found);
                    _Count--;
                    if (currentNode.IsEmpty() && currentNode.Previous != null)
                    {
                        var prevNode = currentNode.Previous;
                        prevNode.ByPassNext();
                    }
                    break;
                }
                currentNode = currentNode.Next;
            }
            while ((currentNode != null));
            if (found)
            {
                FindNewLastNode();
            }
            return found;
        }

        /// <summary>
        /// Removes all items from the collection
        /// </summary>
        public void Clear()
        {
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
            _Count = 0;
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
            CheckIndex(index, _Count);
            if (index == _Count)
            {
                AddLast(item);
                return;
            }
            if (index == 0)
            {
                AddFirst(item);
                return;
            }
            var node = FindNodeAndIndex(ref index);
            node.Insert(index, item);
            _Count++;
            FindNewLastNode();
        }

        public void RemoveAt(int index)
        {
            CheckIndex(index, _Count - 1);
            var node = FindNodeAndIndex(ref index);
            node.RemoveAt(index);
            _Count--;
            FindNewLastNode();
        }
        #endregion IList<T> Methods


        #region Private Methods
        private UnrolledLinkedListNode<T> FindNodeAndIndex(ref int index)
        {
            var currentNode = _FirstNode;
            do
            {
                if (index < currentNode.Count || index == 0)
                {
                    break;
                }
                index -= currentNode.Count;
                currentNode = currentNode.Next;
            }
            while (currentNode != null);
            if (currentNode == null)
            {
                currentNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                _LastNode.Append(currentNode);
                _LastNode = currentNode;
            }
            return currentNode;
        }

        private void FindNewLastNode()
        {
            while (_LastNode.IsEmpty() && _LastNode.Previous != null)
            {
                _LastNode = _LastNode.Previous;
            }
            while (_LastNode.Next != null)
            {
                _LastNode = _LastNode.Next;
            }
        }
        #endregion Private Methods
    }
}
