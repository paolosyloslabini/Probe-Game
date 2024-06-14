using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEvent", menuName = "GlobalEvents/NewEvent")]
public class GlobalEvent : ScriptableObject
{
    private List<GlobalEventListener> listeners = new List<GlobalEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GlobalEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GlobalEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
