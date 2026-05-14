using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    // collection of Observers of the subject:
    private List<IObserver> m_Observers = new List<IObserver>();

    // add the observer to the subject's collection
    void AddObserver(IObserver observer)
    {
        m_Observers.Add(observer);
    }

    // remove the observer from the subject's collection
    void RemoveObserver(IObserver observer)
    {
        m_Observers.Remove(observer);
    }

    // notify each observer that an event has occured
    protected void NotifyObservers()
    {
        m_Observers.ForEach((m_Observer) =>
        {
            m_Observer.OnNotify();
        });
    }
}
