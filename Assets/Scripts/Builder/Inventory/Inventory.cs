using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Inventory<T> : ScriptableObject where T : IInventorable
{
    public List<T> availablePieces;
    public T currentSelection;
}

public interface IInventorable
{
    public Sprite Sprite{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
}
