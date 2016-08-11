using System;
using System.Collections.Generic;

namespace DataStructures
{
    internal sealed class UnrolledLinkedListNode<T>
    {
        internal UnrolledLinkedListNode<T> previous;
        internal UnrolledLinkedListNode<T> next;
        private int _size;
        internal int Count { get { return _size; } }

        internal readonly T[] data;
        private readonly int _halfCapacity;

        #region Constructors
        internal UnrolledLinkedListNode(int capacity)
        {
            data = new T[capacity];
            _halfCapacity = (capacity + 1) / 2;
            _size = 0;
        }

        /// <summary>
        /// Performs a shallow copy of array elements
        /// </summary>
        /// <param name="node"></param>
        internal UnrolledLinkedListNode(UnrolledLinkedListNode<T> node)
        {
            data = new T[node.data.Length];
            Array.Copy(node.data, data, node.data.Length);
            _size = node._size;
        }
        #endregion Constructors

        #region Methods
        internal bool IsEmpty()
        {
            return _size == 0;
        }

        internal bool IsNotFull()
        {
            return _size < data.Length;
        }

        internal UnrolledLinkedListNode<T> Clone()
        {
            return new UnrolledLinkedListNode<T>(this);
        }

        internal void Add(T item)
        {
            data[_size] = item;
            _size++;
        }

        internal void Insert(int index, T item)
        {
            ShiftItemsRight(index, index + 1);
            data[index] = item;
        }

        internal void RemoveAt(int index)
        {
            ShiftItemsLeft(index + 1, index);
            PullItemsFromTail();
        }

        internal void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(data, 0, array, arrayIndex, _size);
        }

        internal void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            Array.Copy(data, index, array, arrayIndex, count);
        }

        internal int IndexOf(T item)
        {
            return Array.IndexOf(data, item, 0, _size);
        }

        internal bool Contains(T item)
        {
            return (IndexOf(item) > -1);
        }

        internal void AddRange(T[] array, int arrayIndex, int count)
        {
            InserRange(_size, array, arrayIndex, count);
        }

        internal void InserRange(int index, T[] array, int arrayIndex, int count)
        {
            ShiftItemsRight(index, index + count);
            Array.Copy(array, arrayIndex, data, index, count);
        }

        internal void RemoveRange(int index, int count)
        {
            ShiftItemsLeft(index + count, index);
        }

        internal void AppendNode(UnrolledLinkedListNode<T> node)
        {
            if (node != null)
                node.previous = this;
            next = node;
        }

        internal void PrependNode(UnrolledLinkedListNode<T> node)
        {
            if (node != null)
                node.next = this;
            previous = node;
        }

        internal void InsertNodeNext(UnrolledLinkedListNode<T> node)
        {
            node.AppendNode(next);
            node.PrependNode(this);
        }

        internal void ByPassNextNode()
        {
            var newNext = next.next;
            if (newNext != null)
                newNext.previous = this;
            next = newNext;
        }
        #endregion Public Methods


        #region Private Methods
        private UnrolledLinkedListNode<T> CreateNextNode()
        {
            var newNode = new UnrolledLinkedListNode<T>(data.Length);
            newNode.AppendNode(next);
            AppendNode(newNode);
            return newNode;
        }

        private void SplitAt(int index)
        {
            CreateNextNode();
            PushItemsToNextNode(index);
        }

        private void ShiftItemsRight(int sourceIndex, int destinationIndex)
        {
            if (destinationIndex < sourceIndex)
                throw new InvalidOperationException("This shifts items right");

            int length = _size - sourceIndex;
            Array.Copy(data, sourceIndex, data, destinationIndex, length);
            _size += destinationIndex - sourceIndex;
        }

        private void ShiftItemsLeft(int sourceIndex, int destinationIndex)
        {
            if (destinationIndex > sourceIndex)
                throw new InvalidOperationException("This shifts items left");

            int idxDifference = sourceIndex - destinationIndex;
            int length = _size - sourceIndex;

            // TODO: think about the special case of length == 0,
            // when the last element in the node is removed

            Array.Copy(data, sourceIndex, data, destinationIndex, length);
            Array.Clear(data, destinationIndex + length, idxDifference);
            _size -= idxDifference;
        }

        internal void PullItemsFromTail()
        {
            if (next == null)
                return;

            if (_size >= _halfCapacity)
                return;

            PullItemsFromNextUntilHalfFull();

            if (next == null)
                return;

            var next_size = next._size;
            int remainingCapacity = data.Length - _size;

            if (next_size < next._halfCapacity && remainingCapacity >= next_size)
            {
                Array.Copy(next.data, 0, data, _size, next_size);
                _size += next_size;
                ByPassNextNode();
            }
        }

        private void PullItemsFromNextUntilHalfFull()
        {
            int itemsShortToHalfFull = _halfCapacity - _size;
            if (itemsShortToHalfFull == 0)
                return;
            int numberOfItemsToTransfer = 0;
            var node = next;
            while (itemsShortToHalfFull > 0 && next != null)
            {
                numberOfItemsToTransfer = Math.Min(itemsShortToHalfFull, next._size);
                if (numberOfItemsToTransfer == 0)
                    return;
                Array.Copy(next.data, 0, data, _size, numberOfItemsToTransfer);
                _size += numberOfItemsToTransfer;
                itemsShortToHalfFull -= numberOfItemsToTransfer;
                if (next._size == numberOfItemsToTransfer)
                    ByPassNextNode();
            }
            if (next != null)
                next.ShiftItemsLeft(numberOfItemsToTransfer, 0);
        }

        internal void PushItemsToNextNode(int startIndex)
        {
            var length = _size - startIndex;
            Array.Copy(data, startIndex, next.data, 0, length);
            next._size += length;

            Array.Clear(data, startIndex, length);
            _size -= length;
        }
        #endregion Private Methods
    }
}
