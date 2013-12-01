using System;
using System.Collections;

namespace Game
{
	public class LimitedQueue<T> : Queue
	{
	    private int limit = -1;
	
	    public int Limit
	    {
	        get { return limit; }
	        set { limit = value; }
	    }
	
	    public LimitedQueue(int limit)
	        : base(limit)
	    {
	        this.Limit = limit;
	    }
	
	    public void Enqueue(T item)
	    {
	        if (this.Count >= this.Limit)
	        {
	            this.Dequeue();
	        }
	        base.Enqueue(item);
	    }
	}
}

