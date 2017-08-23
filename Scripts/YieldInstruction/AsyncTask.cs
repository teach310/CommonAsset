using UnityEngine;

public class AsyncTask : CustomYieldInstruction
{
    bool isDone = false;
    public bool IsDone { get { return isDone; } }

    public virtual void Done()
    {
        isDone = true;
    }

    public override bool keepWaiting
    {
        get
        {
            return !isDone;
        }
    }
}

public class AsyncRequest<T> : CustomYieldInstruction
{
	bool isDone = false;
	public bool IsDone { get { return isDone; } }
    protected T data;

	public virtual void Done(T data)
	{
        this.data = data;
		isDone = true;
	}

	public override bool keepWaiting
	{
		get
		{
			return !isDone;
		}
	}
}