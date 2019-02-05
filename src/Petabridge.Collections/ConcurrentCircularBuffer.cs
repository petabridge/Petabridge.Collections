﻿// -----------------------------------------------------------------------
// <copyright file="ConcurrentCircularBuffer.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2019 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Petabridge.Collections
{
    /// <summary>
    ///     Concurrent circular buffer implementation, synchronized using a monitor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentCircularBuffer<T> : ICircularBuffer<T>
    {
        #region Internal members

        /// <summary>
        ///     The buffer itself
        /// </summary>
        private readonly T[] _buffer;

        #endregion

        private bool _full;

        public ConcurrentCircularBuffer(int capacity)
        {
            Head = 0;
            Tail = 0;
            _buffer = new T[capacity];
        }

        private int SizeUnsafe => _full ? Capacity : (Tail - Head + Capacity) % Capacity;

        /// <summary>
        ///     FOR TESTING PURPOSES ONLY
        /// </summary>
        internal int Head { get; private set; }

        /// <summary>
        ///     FOR TESTING PURPOSES ONLY
        /// </summary>
        internal int Tail { get; private set; }

        public bool IsReadOnly => false;

        public int Capacity => _buffer.Length;


        public int Size
        {
            get
            {
                lock (SyncRoot)
                {
                    return SizeUnsafe;
                }
            }
        }

        public T Peek()
        {
            lock (SyncRoot)
            {
                return _buffer[Head];
            }
        }

        public void Enqueue(T obj)
        {
            lock (SyncRoot)
            {
                UnsafeEnqueue(obj);
            }
        }

        public void Enqueue(T[] objs)
        {
            //Expand
            lock (SyncRoot)
            {
                foreach (var item in objs) UnsafeEnqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (SyncRoot)
            {
                return UnsafeDequeue();
            }
        }

        public IEnumerable<T> Dequeue(int count)
        {
            IList<T> returnItems;

            lock (SyncRoot)
            {
                var availabileItems = Math.Min(count, SizeUnsafe);
                returnItems = new List<T>(availabileItems);

                if (availabileItems == 0)
                    return returnItems;

                for (var i = 0; i < availabileItems; i++) returnItems.Add(UnsafeDequeue());
            }

            return returnItems;
        }

        public IEnumerable<T> DequeueAll()
        {
            return Dequeue(Size);
        }

        public void Clear()
        {
            lock (SyncRoot)
            {
                Head = 0;
                Tail = 0;
                _full = false;
            }
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int index)
        {
            CopyTo(array, index, Size);
        }

        public bool TryAdd(T item)
        {
            Enqueue(item);
            return true;
        }

        public bool TryTake(out T item)
        {
            item = default(T);
            if (Size == 0) return false;
            item = Dequeue();
            return true;
        }

        public T[] ToArray()
        {
            lock (SyncRoot)
            {
                var bufferCopy = new T[SizeUnsafe];
                CopyToUnsafe(bufferCopy, 0, bufferCopy.Length);
                return bufferCopy;
            }
        }

        public void CopyTo(T[] array, int index, int count)
        {
            lock (SyncRoot)
            {
                CopyToUnsafe(array, index, count);
            }
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo((T[]) array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) ToArray()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Size;

        public object SyncRoot { get; } = new object();

        public bool IsSynchronized => true;

        private void UnsafeEnqueue(T obj)
        {
            _full = _full || Tail + 1 == Capacity; // leave FULL flag on
            _buffer[Tail] = obj;
            Tail = (Tail + 1) % Capacity;
        }

        private T UnsafeDequeue()
        {
            _full = false; // full is always false as soon as we dequeue
            var item = _buffer[Head];
            Head = (Head + 1) % Capacity;
            return item;
        }

        public void Add(T item)
        {
            Enqueue(item);
        }

        public bool Contains(T item)
        {
            lock (SyncRoot)
            {
                return _buffer.Any(x => x.GetHashCode() == item.GetHashCode());
            }
        }

        public bool Remove(T item)
        {
            return false;
        }

        private void CopyToUnsafe(T[] array, int index, int count)
        {
            if (count > SizeUnsafe) //The maximum value of count is Size
                count = SizeUnsafe;

            var bufferBegin = Head;
            for (var i = 0; i < count; i++, bufferBegin = (bufferBegin + 1) % Capacity, index++)
                array[index] = _buffer[bufferBegin];
        }
    }
}