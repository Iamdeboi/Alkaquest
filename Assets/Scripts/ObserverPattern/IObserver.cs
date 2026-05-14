using UnityEngine;

public interface IObserver
{
    // class that inherit from IObserver interface
    // must implement the OnNotify method
    public void OnNotify()
    {
        // do something when the event happens
    }
}
