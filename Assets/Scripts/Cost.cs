using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ResourceDict
{
    [SerializeField] float energy;
    [SerializeField] float iron;
    [SerializeField] float copper;
    [SerializeField] float diamond;

    public Dictionary<ResourceType, float> GetDict()
    {
        var dict = new Dictionary<ResourceType, float>();
        foreach (ResourceType resType in Enum.GetValues(typeof(ResourceType)))
        {
            if (this[resType] > 0) dict[resType] = this[resType];
        }
        return dict;

    }

    public float this[ResourceType key]
    {
        get
        {
            switch (key)
            {
                case ResourceType.energy:
                    return energy;
                case ResourceType.iron:
                    return iron;
                case ResourceType.copper:
                    return copper;
                case ResourceType.diamond:
                    return diamond;
                default:
                    throw new InvalidOperationException("Invalid key");
            }
        }
    }

}