using System;

namespace DataStructures
{
    class UnrolledLinkedListNode<T>
    {
        public UnrolledLinkedListNode<T> Previous;
        public UnrolledLinkedListNode<T> Next;
        public int Count { get; private set; }
        public readonly T[] Data;

        public UnrolledLinkedListNode(int capacity)
        {
            Data = new T[capacity];
        }

        public UnrolledLinkedListNode(
            int capacity,
            UnrolledLinkedListNode<T> previous) : this(capacity)
        {
            Previous = previous;
        }

        public UnrolledLinkedListNode(
            int capacity,
            UnrolledLinkedListNode<T> previous,
            UnrolledLinkedListNode<T> next) : this(capacity, previous)
        {
            Next = next;
        }

        public UnrolledLinkedListNode(
            int capacity,
            UnrolledLinkedListNode<T> previous,
            T firstElement) : this(capacity, previous)
        {
            Data[0] = firstElement;
            Count += 1;
        }

        private int HalfCapacity { get { return (Data.Length + 1) / 2; } }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public void ByPassNext()
        {
            var newNext = Next.Next;
            newNext.Previous = this;
            Next = newNext;
        }

        public bool TryAddItem(T item)
        {
            if (Count < Data.Length)
            {
                Data[Count] = item;
                Count += 1;
                return true;
            }
            return false;
        }

        public void RemoveItem(int index)
        {
            ShiftItemsLeft(index + 1, index);
            PullItemsFromNext();
        }

        public int CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(Data, 0, array, arrayIndex, Count);
            return Count;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(Data, item, 0, Count);
        }

        public bool Contains(T item)
        {
            return (IndexOf(item) > -1);
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

            TransferItemsFromNextUntilHalfFull();

            var nextCount = Next.Count;
            int remainingCapacity = Data.Length - Count;

            if (nextCount <= remainingCapacity)
            {
                Array.Copy(Next.Data, 0, Data, Count, nextCount);
                ByPassNext();
            }
        }

        private void TransferItemsFromNextUntilHalfFull()
        {
            int itemsShortToHalfFull = HalfCapacity - Count;
            int numberOfItemsToTransfer = Math.Min(itemsShortToHalfFull, Next.Count);
            Array.Copy(Next.Data, 0, Data, Count, numberOfItemsToTransfer);
            Count += numberOfItemsToTransfer;

            Next.ShiftItemsLeft(numberOfItemsToTransfer, 0);
        }

        public void Insert(int index, T item)
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

        private void ShiftItemsRight(int sourceIndex, int destinationIndex)
        {
            if (destinationIndex < sourceIndex)
                throw new InvalidOperationException("This shifts items left");

            int length = Count - sourceIndex;
            Array.Copy(Data, sourceIndex, Data, destinationIndex, length);
            Count += destinationIndex - sourceIndex;
        }

        private void CreateNext()
        {
            var newNode = new UnrolledLinkedListNode<T>(Data.Length);
            newNode.Next = Next;
            newNode.Previous = this;
            Next = newNode;
        }

        private void PushItemsToNext(int startIndex)
        {
            var length = Count - startIndex;
            Array.Copy(Data, startIndex, Next.Data, 0, length);
            Next.Count += length;

            Array.Clear(Data, startIndex, length);
            Count -= length;
        }
    }
}
