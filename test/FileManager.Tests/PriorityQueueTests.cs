namespace FileManager.Tests;

public class PriorityQueueTests
{
    [Fact]
    public void SortRows_CorrectOrder()
    {
        var queue = new PriorityQueue<int, string>(new LineComparer());

        var text = "415. Apple";
        queue.Enqueue(1, text);

        text = "30432. Something something something";
        queue.Enqueue(2, text);

        text = "1. Apple";
        queue.Enqueue(3, text);

        text = "32. Cherry is the best";
        queue.Enqueue(4, text);

        text = "2. Banana is yellow";
        queue.Enqueue(5, text);
        
        queue.TryDequeue(out var item, out var key);
        Assert.Equal("1. Apple", key);
        Assert.Equal(3, item);
        
        queue.TryDequeue(out item, out key);
        Assert.Equal( "415. Apple", key);
        Assert.Equal(1, item);
        
        queue.TryDequeue(out item, out key);
        Assert.Equal("2. Banana is yellow", key);
        Assert.Equal(5, item);
        
        queue.TryDequeue(out item, out key);
        Assert.Equal("32. Cherry is the best", key);
        Assert.Equal(4, item);
        
        queue.TryDequeue(out item, out key);
        Assert.Equal("30432. Something something something", key);
        Assert.Equal(2, item);
    }
}