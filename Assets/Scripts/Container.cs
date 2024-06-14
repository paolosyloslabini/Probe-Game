using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum ResourceType {iron, copper, diamond,energy}

public class Container
{
    private Dictionary<ResourceType,float> resourceDict;
    private ResourceDict maxResourcesDict;

    public Container(ResourceDict maxResourcesDict, ResourceDict startResourceDict)
    {
        this.maxResourcesDict = maxResourcesDict;
        resourceDict = startResourceDict.GetDict();
    }

    public Container(ResourceDict maxResourcesDict)
    {
        this.maxResourcesDict = maxResourcesDict;
        resourceDict = new Dictionary<ResourceType, float>();
    }

    public Container()
    {
        maxResourcesDict = new ResourceDict();
        resourceDict = new Dictionary<ResourceType, float>();
    }

    public float GetAmount(ResourceType resourceType)
    {
        if (!resourceDict.ContainsKey(resourceType)) return 0;
        else return resourceDict[resourceType];
    }

    public string GetContentString()
    {
        string info = "";
        foreach (var res in resourceDict.Keys)
        {
            info += $"{res} : {resourceDict[res]} \n";            
        }
        return info;
    }

    public float AddResource(float amount, ResourceType resourceType, bool fractional = false)
    {
        if (fractional && amount > 1) throw new InvalidOperationException("AddResource: amount must be <= 1 when fractional is true");
        if (fractional) amount *= GetCapacity(resourceType);
        if (resourceDict.ContainsKey(resourceType)) resourceDict[resourceType] += amount;
        else resourceDict[resourceType] = amount;
        return amount;
    }

    public Dictionary<ResourceType, float> AddResources(Dictionary<ResourceDict, float> resources, bool fractional = false)
    {
        Dictionary<ResourceType, float> added = new();
        foreach(var kvp in resourceDict)
        {
            added[kvp.Key] = AddResource(kvp.Value, kvp.Key, fractional);
        }
        return added;
    }

    public float RemoveResource(float amount, ResourceType resourceType, bool fractional = false)
    {
        if (fractional && amount <= 1) amount *= GetAmount(resourceType);
        if (resourceDict.ContainsKey(resourceType)) 
        {
            amount = Math.Min(resourceDict[resourceType], amount);
            resourceDict[resourceType] -= amount;
        }
        else amount = 0;
        return amount;
    }

    public Dictionary<ResourceType, float> RemoveResources(Dictionary<ResourceType, float> resourceDict, bool fractional = false)
    {
        Dictionary<ResourceType, float> removed = new();

        foreach(var kvp in resourceDict)
        {
            removed[kvp.Key] = RemoveResource(kvp.Value, kvp.Key, fractional);
        }
        return removed;
    }

    public void SetResource(float amount, ResourceType resourceType)
    {
        resourceDict[resourceType] = Math.Min(amount, GetCapacity(resourceType));
    }

    public float GetResourceFrom(Container giver, float amount, ResourceType resourceType, bool fractional = false)
    {
        float capacity = GetCapacity(resourceType);
        amount = Math.Min(amount, capacity);
        float extracted = giver.RemoveResource(amount, resourceType, fractional);
        AddResource(extracted, resourceType);
        return extracted;
    }

    public float GetCapacity(ResourceType resource)
    {
        return maxResourcesDict[resource] - GetAmount(resource);
    }

    internal Container Copy()
    {
        return new Container(maxResourcesDict);
    }
}
