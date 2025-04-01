using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;

//https://medium.com/@defuncart/json-serialization-in-unity-9420abbce30b Json
//https://code-maze.com/csharp-read-and-process-json-file/
//https://videlais.com/2021/02/25/using-jsonutility-in-unity-to-save-and-load-game-data/
//https://stackoverflow.com/questions/3144492/how-do-i-get-nets-path-combine-to-convert-forward-slashes-to-backslashes
//TODO: add overide existing data function (should take a bool with true or false)
//TODO: add Create New file, change save function to not save if file doenst exist
/// <summary>
/// JSON Serialzer using Newtonsoft.Json 
/// Creates,loads and Saves to Json
/// 
/// Current issues quirks:
/// - asking for a filepath is unclear when loading
/// - should have to check for persistent datapath
/// - should class be static or only methods?
/// 
/// TODO:
/// Version number
/// Encryption voor gevoelige data
/// Application.persistentDataPath
/// 
/// </summary>

public static class JSONSerializer
{
    /// <summary>
    /// Loads and deserializes a JSON file into a class object.
    /// </summary>
    /// <typeparam name="T">The class type to deserialize into</typeparam>
    /// <param name="path">The full path to the JSON file (including filename)</param>
    /// <returns>Deserialized object of type T, or null if failed</returns>
    static string path = Application.persistentDataPath;
    //we need this to directly send vector3 and quaternions unity type objects https://www.newtonsoft.com/json/help/html/ReferenceLoopHandlingIgnore.htm
    static JsonSerializerSettings ignorerule = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    public static T Load<T>(string filename) where T : class
    {
        try
        {
            //make sure we add the .json extention
            Debug.Log(GetFullpathjson(path, filename));
            string json = File.ReadAllText(GetFullpathjson(path, filename));
            T data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Load] Failed to load JSON from : {GetFullpathjson(path, filename)}\nException: {ex.Message}");
            throw;
        }
    }
    /// <summary>
    /// Save serializable objects to Json file in persistent data path.
    /// </summary>
    /// <typeparam name="T">AnySaveable type</typeparam>
    /// <param name="filename">When not empty save to separate file</param>
    /// <param name="data">Actual data for serialization</param>
    public static void Save<T>(T data, string filename) where T : class
    {
        //TODO: make sure we dont overide data
        if (!string.IsNullOrEmpty(filename))
        {
            File.WriteAllText(GetFullpathjson(path, filename), JsonConvert.SerializeObject(data, Formatting.Indented));
            Debug.Log("Saving Data to: " + GetFullpathjson(path, filename));
        }
    }
    //Using the Wrapper to save the data
    public static void Save<T>(T data, string filename, int version = 1) 
    {
        //TODO: make sure we dont overide data
        if (!string.IsNullOrEmpty(filename))
        {
            var wrapper = new JsonWrapper<T>(data, version);
            File.WriteAllText(GetFullpathjson(path, filename), JsonConvert.SerializeObject(wrapper, Formatting.Indented, ignorerule));
            Debug.Log("Saving Data to: " + GetFullpathjson(path, filename));
        }
    }

    public static bool PathExists(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// checks if file exists at path
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static bool FileExists(string filename)
    {
        if (File.Exists(GetFullpathjson(path, filename)))
        {
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Returns combined path and filename with .json extention
    /// </summary>
    /// <param name="path">persistentdata by default</param>
    /// <param name="filename">the file which might be missing .json</param>
    /// <returns></returns>
    public static string GetFullpathjson(string path,string filename)
    {
        if (!filename.EndsWith(".json")) filename += ".json";
        string fullpath = Path.Combine(path, filename);
        return fullpath;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data">text data you would like to add to an existing file</param>
    public static void AppendDataToFile<T>(string Identifier, T data, string filename)
    {
        var JsonEntry = new Dictionary<string, T>
        {
            {  Identifier,data }
        };

        if (FileExists(filename))
        {
            File.AppendAllText(GetFullpathjson(path, filename), JsonConvert.SerializeObject(JsonEntry, Formatting.Indented));
            Debug.Log($"Appended Data to {filename}");
        }
        else Debug.LogWarning("We don't have a valid path! " + path);
    }

    private static string GetDate()
    {
        return DateTime.Today.ToString();
    }

    private static string GetTime()
    {
        return DateTime.Now.ToString();
    }
}
public class JsonWrapper<T>
{
    public int Version;
    public string Lastsaved;
    public T Data; //blackboard data
    //Dictionary<string, object> Data
    public JsonWrapper(T data, int version = 1)
    {
        Version = version;
        Lastsaved = System.DateTime.UtcNow.ToString("o"); // ISO 8601
        Data = data;// as Dictionary<string, object>;
    }

}