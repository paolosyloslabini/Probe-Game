using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalVariable<T> : ScriptableObject
{
    [SerializeField] private T value;
    [SerializeField] private T lastValue = default;

    public event Action<T,T> OnValueChangedWithHistory;
    public event Action<T> OnValueChanged;

    public void NotifyChange()
    {
        OnValueChangedWithHistory?.Invoke(lastValue, Value);
        OnValueChanged?.Invoke(Value);
    }
    public void NotifyChange(T value) => NotifyChange();


    public T Value
    {
        get => value;
        set
        {
            lastValue = this.value;
            this.value = value;
            NotifyChange();            
        }
    }

    public T LastValue
    {
        get => value;
        private set => lastValue = value;
    }

    void OnValidate()
    {
        NotifyChange();
    }
}
