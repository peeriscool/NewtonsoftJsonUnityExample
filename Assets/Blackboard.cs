using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blackboard keeps track of local player data that has to be serialized to json and comunnicated with the network.
/// </summary>
public class Blackboard : MonoBehaviour
{
    // Singleton instance
    public static Blackboard Instance { get; private set; }

    // Dictionary to store data
    private readonly Dictionary<string, object> data = new();

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate blackboards
        }
    }

    // Method to set a value in the blackboard
    public void SetValue<T>(string key, T value)
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
    public T GetValue<T>(string key)
    {
        if (data.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        Debug.LogWarning($"Key '{key}' not found in Blackboard.");
        return default;
    }

    // Check if a key exists in the blackboard
    public bool ContainsKey(string key)
    {
        return data.ContainsKey(key);
    }

    // Remove a key-value pair
    public void RemoveValue(string key)
    {
        if (data.ContainsKey(key))
        {
            data.Remove(key);
        }
    }
    public Dictionary<string,object>GetData()
    {
        return data;
    }
}
