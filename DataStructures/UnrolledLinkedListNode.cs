using System;
using System.Collections.Generic;

namespace DataStructures
{
    class UnrolledLinkedListNode<T>
    {
        internal UnrolledLinkedListNode<T> Previous { get; private set; }
        internal UnrolledLinkedListNode<T> Next { get; private set; }
        internal int Count { get; private set; }
        internal readonly T[] Data;

        private int HalfCapacity { get { return (Data.Length + 1) / 2; } }

        #region Constructors
        internal UnrolledLinkedListNode(int capacity)
        {
            Data = new T[capacity];
            Count = 0;
        }

        protected UnrolledLinkedListNode(UnrolledLinkedListNode<T> node)
        {
            Data = new T[node.Data.Length];
            Array.Copy(node.Data, Data, node.Data.Length);
            Count = node.Count;
        }
        #endregion Constructors

        #region Methods
        internal bool IsEmpty()
        {
            return Count == 0;
        }

        internal UnrolledLinkedListNode<T> Clone()
        {
            return new UnrolledLinkedListNode<T>(this);
        }

        internal void Add(T item)
        {
            if (Count < Data.Length)
            {
                Data[Count] = item;
                Count++;
            }
            else
            {
                CreateNext();
                Next.Add(item);
            }
        }

        internal void RemoveAt(int index)
        {
            ShiftItemsLeft(index + 1, index);
            PullItemsFromNext();
        }

        internal int CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(Data, 0, array, arrayIndex, Count);
            return Count;
        }

        internal int IndexOf(T item)
        {
            return Array.IndexOf(Data, item, 0, Count);
        }

        internal bool Contains(T item)
        {
            return (IndexOf(item) > -1);
        }

        internal void Insert(int index, T item)
        {
            var targetNode = this;
            if (Count == Data.Length)
            {
                int midIndex = Data.Length / 2;
                var condition = index > midIndex;
                if (condition)
                {
                    midIndex += 1;
                    index -= midIndex;
                }

                SplitAt(midIndex);
                if (condition) targetNode = Next;
            }

            targetNode.ShiftItemsRight(index, index + 1);
            targetNode.Data[index] = item;
        }

        internal int InsertRange(int index, IEnumerable<T> collection)
        {
            int collectionLength = 0;
            ICollection<T> c = collection as ICollection<T>;
            var node = this;
            if (c != null)
            {
                collectionLength = c.Count;
                int remainingNodeItemsCount = node.Count - index;

                T[] newData = new T[c.Count + remainingNodeItemsCount];
                c.CopyTo(newData, 0);

                if (remainingNodeItemsCount > 0)
                {
                    Array.Copy(node.Data, index, newData, c.Count, remainingNodeItemsCount);
                    node.Count -= remainingNodeItemsCount;
                }

                int startNewDataCopyIndex = 0;

                while (startNewDataCopyIndex < newData.Length)
                {
                    var copyLength = Math.Min(
                        node.Data.Length - node.Count,
                        newData.Length - startNewDataCopyIndex);
                    Array.Copy(newData, startNewDataCopyIndex, node.Data, node.Count, copyLength);
                    node.Count += copyLength;
                    startNewDataCopyIndex += copyLength;
                    node = node.CreateNext();
                }                
            }
            else
            {
                var enumerator = collection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    node.Insert(index, enumerator.Current);
                    collectionLength++;
                    index = ++index % Data.Length;
                    if (index > node.Count)
                    {
                        index -= node.Count;
                        node = node.Next;
                        continue;
                    }
                    if (index == 0)
                    {
                        if (node.Next == null)
                            node.CreateNext();
                        node = node.Next;
                    }
                }
            }
            if (node.IsEmpty())
            {
                node = node.Previous;
                node.ByPassNext();
            }
            node.PullItemsFromNext();
            return collectionLength;
        }

        internal void Append(UnrolledLinkedListNode<T> node)
        {
            if (node != null)
                node.Previous = this;
            Next = node;
        }

        internal void Prepend(UnrolledLinkedListNode<T> node)
        {
            if (node != null)
                node.Next = this;
            Previous = node;
        }

        internal void ByPassNext()
        {
            if (Next == null)
                return;
            var newNext = Next.Next;
            if (newNext != null)
                newNext.Previous = this;
            Next = newNext;
        }
        #endregion Public Methods


        #region Private Methods
        private UnrolledLinkedListNode<T> CreateNext()
        {
            var newNode = new UnrolledLinkedListNode<T>(Data.Length);
            newNode.Append(Next);
            Append(newNode);
            return newNode;
        }

        private void SplitAt(int index)
        {
            CreateNext();
            PushItemsToNext(index);
        }

        private void ShiftItemsRight(int sourceIndex, int destinationIndex)
        {
            if (destinationIndex < sourceIndex)
                throw new InvalidOperationException("This shifts items right");

            int length = Count - sourceIndex;
            Array.Copy(Data, sourceIndex, Data, destinationIndex, length);
            Count += destinationIndex - sourceIndex;
        }

        private void ShiftItemsLeft(int sourceIndex, int destinationIndex)
        {
            if (destinationIndex > sourceIndex)
                throw new InvalidOperationException("This shifts items left");

            int idxDifference = sourceIndex - destinationIndex;
            int length = Count - sourceIndex;

            // TODO: think about the special case of length == 0,
            // when the last element in the node is removed

            Array.Copy(Data, sourceIndex, Data, destinationIndex, length);
            Array.Clear(Data, destinationIndex + length, idxDifference);
            Count -= idxDifference;
        }

        private void PullItemsFromNext()
        {
            if (Next == null)
                return;

            if (Count > HalfCapacity)
                return;

            PullItemsFromNextUntilHalfFull();

            var nextCount = Next.Count;
            int remainingCapacity = Data.Length - Count;

            if (nextCount <= remainingCapacity)
            {
                Array.Copy(Next.Data, 0, Data, Count, nextCount);
                Count += nextCount;
                Next.Count = 0;
                ByPassNext();
            }
        }

        private void PullItemsFromNextUntilHalfFull()
        {
            int itemsShortToHalfFull = HalfCapacity - Count;
            int numberOfItemsToTransfer = Math.Min(itemsShortToHalfFull, Next.Count);
            if (numberOfItemsToTransfer == 0)
                return;
            Array.Copy(Next.Data, 0, Data, Count, numberOfItemsToTransfer);
            Count += numberOfItemsToTransfer;

            Next.ShiftItemsLeft(numberOfItemsToTransfer, 0);
        }

        private void PushItemsToNext(int startIndex)
        {
            var length = Count - startIndex;
            Array.Copy(Data, startIndex, Next.Data, 0, length);
            Next.Count += length;

            Array.Clear(Data, startIndex, length);
            Count -= length;
        }
        #endregion Private Methods
    }
}
