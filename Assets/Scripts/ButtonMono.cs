using UnityEngine;
using UnityEngine.Events;

public class ButtonMono : MonoBehaviour
{
    // Create a public UnityEvent
    public UnityEvent onTriggerAction;

    // Method to invoke the event
    public void TriggerAction()
    {
        // Invoke all functions added to this event
        onTriggerAction?.Invoke();
    }
}