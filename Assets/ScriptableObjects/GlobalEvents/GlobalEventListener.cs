using UnityEngine;
using UnityEngine.Events;

public class GlobalEventListener : MonoBehaviour
{
    [SerializeField]
    private GlobalEvent globalEvent;

    [SerializeField]
    private UnityEvent response;

    private void OnEnable()
    {
        globalEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        globalEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
