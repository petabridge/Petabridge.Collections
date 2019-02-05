// -----------------------------------------------------------------------
// <copyright file="CircularBufferTests.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2019 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using System.Text;
using Xunit;

namespace Petabridge.Collections.Tests
{
    public class CircularBufferTests
    {
        protected virtual ICircularBuffer<T> GetBuffer<T>(int capacity)
        {
            return new CircularBuffer<T>(capacity);
        }


        /// <summary>
        ///     If a circular buffer is defined with a fixed maximum capacity, it should
        ///     simply overwrite the old elements even if they haven't been dequed
        /// </summary>
        [Fact]
        public void CircularBuffer_should_not_expand()
        {
            var byteArrayOne = Encoding.Unicode.GetBytes("ONE STRING");
            var byteArrayTwo = Encoding.Unicode.GetBytes("TWO STRING");
            var buffer = GetBuffer<byte>(byteArrayOne.Length);
            buffer.Enqueue(byteArrayOne);
            Assert.Equal(buffer.Size, buffer.Capacity);
            buffer.Enqueue(byteArrayTwo);
            Assert.Equal(buffer.Size, buffer.Capacity);
            var availableBytes = buffer.DequeueAll().ToArray();
            Assert.False(byteArrayOne.SequenceEqual(availableBytes));
            Assert.True(byteArrayTwo.SequenceEqual(availableBytes));
        }
    }
}