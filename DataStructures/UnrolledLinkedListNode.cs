using System;

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
        #endregion Constructors

        #region Methods
        internal bool IsEmpty()
        {
            return Count == 0;
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
            if (Count < Data.Length)
            {
                ShiftItemsRight(index, index + 1);
                Data[index] = item;
            }
            else
            {
                CreateNext();
                int midIndex = Data.Length / 2;

                UnrolledLinkedListNode<T> nodeInsertTo = null;

                if (index > midIndex)
                {
                    nodeInsertTo = Next;
                    midIndex += 1;
                    index -= midIndex;
                }
                else
                {
                    nodeInsertTo = this;
                }

                PushItemsToNext(midIndex);

                nodeInsertTo.Insert(index, item);
            }
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
        private void CreateNext()
        {
            var newNode = new UnrolledLinkedListNode<T>(Data.Length);
            newNode.Next = Next;
            newNode.Previous = this;
            Next = newNode;
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
                ByPassNext();
            }
        }

        private void PullItemsFromNextUntilHalfFull()
        {
            int itemsShortToHalfFull = HalfCapacity - Count;
            int numberOfItemsToTransfer = Math.Min(itemsShortToHalfFull, Next.Count);
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
