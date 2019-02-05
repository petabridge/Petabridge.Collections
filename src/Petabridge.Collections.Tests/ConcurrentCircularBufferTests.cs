namespace Petabridge.Collections.Tests
{
    public class ConcurrentCircularBufferTests : CircularBufferTests
    {
        protected override ICircularBuffer<T> GetBuffer<T>(int capacity)
        {
            return new ConcurrentCircularBuffer<T>(capacity);
        }
    }
}