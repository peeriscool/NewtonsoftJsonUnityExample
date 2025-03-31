using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blackboard keeps track of local player data that has to be serialized to json and comunnicated with the network.
/// </summary>
public static class Blackboard 
{
  
    // Dictionary to store data
    private static readonly Dictionary<string, object> data = new();
    // Method to set a value in the blackboard
    public static void SetValue<T>(string key, T value)
    {
        if (data.ContainsKey(key))
        {
            data[key] = value; // Update existing value
        }
        else
        {
            data.Add(key, value); // Add new value
        }
    }

    // Method to get a value from the blackboard
    public static T GetValue<T>(string key)
    {
        if (data.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        Debug.LogWarning($"Key '{key}' not found in Blackboard.");
        return default;
    }

    // Check if a key exists in the blackboard
    public static bool ContainsKey(string key)
    {
        return data.ContainsKey(key);
    }

    // Remove a key-value pair
    public static void RemoveValue(string key)
    {
        if (data.ContainsKey(key))
        {
            data.Remove(key);
        }
    }
    public static Dictionary<string,object>GetData()
    {
        return data;
    }
}
