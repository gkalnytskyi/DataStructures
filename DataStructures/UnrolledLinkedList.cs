﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public partial class UnrolledLinkedList<T> : ICollection<T>, IEnumerable<T>
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

        #region ICollection<T> properties
        public int Count { get; private set; }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region IEnumerable<T> methods
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new UnroledLinkedListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

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
    }
}
