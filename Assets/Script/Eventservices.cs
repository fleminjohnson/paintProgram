using System;

public class EventServices : SingletonGeneric<EventServices>
{
    public event Action OnGameOver;
    public event Action OnWin;
    public event Action<int> OnFlowUpdate;
    public event Action<int> OnMoveUpdate;

    public void InvokeOnGameOver()
    {
        OnGameOver?.Invoke();
    }

    public void InvokeOnWin()
    {
        OnWin?.Invoke();
    }

    public void InvokeOnFlowUpdate(int count)
    {
        OnFlowUpdate?.Invoke(count);
    }

    public void InvokeOnMoveUpdate(int count)
    {
        OnMoveUpdate?.Invoke(count);
    }
}
