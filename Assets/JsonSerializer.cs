using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
    public static T Load<T>(string filename) where T : class
    {
        string fullpath = path;
        try
        {
            //make sure we add the .json extention
            if (!filename.EndsWith(".json")) filename += ".json";
            fullpath = Path.Combine(path, filename);
            Debug.Log(fullpath);
            //if (!FileExists(fullpath))
            //{
            //    Debug.LogWarning($"[Load] File not found at path: {fullpath}");
            //    return default;
            //}
        
           string json = File.ReadAllText(fullpath);

            //if (string.IsNullOrWhiteSpace(json))
            //{
            //    Debug.LogWarning($"[Load] File is empty at path: {fullpath}");
            //    return default;
            //}

            T data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Load] Failed to load JSON from : {fullpath}\nException: {ex.Message}");
            throw;
        }
    }
    /// <summary>
    /// returns class object From File.Read <br></br>
    /// path includes file name
    /// </summary>
    /// <typeparam name="T">Class type that gets returned</typeparam>
    /// <param name="Path">Location of file to read</param>
    /// 
    //public static T Load<T>(string Path) where T : class
    //{
    //    if (PathExists(Path))
    //    {
    //        return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path));
    //       // return JsonUtility.FromJson<T>(File.ReadAllText(Path));
    //    }
    //    //lets tell we didnt find the filepath
    //    Debug.LogWarning("Json couldn't find the path: " + Path);
    //    return default; //default(T)
    //}

    /// <summary>
    /// returns class object From persistentDataPath <br></br>
    /// path excludes file name
    /// </summary>
    /// <typeparam name="T">Class type that gets returned</typeparam>
    /// <param name="Path">Location of file to read</param>
    //public static T Load<T>(string Path, string filename) where T : class
    //{
    //    if (PathExists(Path))
    //    {
    //        return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path + filename));

    //        //  return JsonUtility.FromJson<T>(Application.persistentDataPath + filename);
    //    }
    //    return default;
    //}

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
            if (!filename.EndsWith(".json")) filename += ".json";
            string fullpath = Path.Combine(path, filename);
           // string path = Application.persistentDataPath+ "/" + filename + ".json"; //should create a folder persistentdatapath +/Folder/ + Filenmae + "Json"
            File.WriteAllText(fullpath, JsonConvert.SerializeObject(data, Formatting.Indented));
            Debug.Log("Saving Data to: " + fullpath);
        }
    }

    /// <summary>
    /// Save serializable objects to Json file at given path.
    /// </summary>
    /// <typeparam name="T">AnySaveable type</typeparam>
    /// <param name="filename">When not empty save to separate file</param>
    /// <param name="data">Actual data for serialization</param>
    //public static void Save<T>(string Path, T data) where T : class
    //{
    //    if (!string.IsNullOrEmpty(Path))
    //    {
    //        //if given path is not empty or null, Write data to location
    //        File.WriteAllText(Path, JsonConvert.SerializeObject(data, Formatting.Indented));
    //        Debug.Log("Saving Data to: " + Path);
    //    }
    //    else
    //    {
    //        //create a pop up that lets the user know that the object could not be saved 
    //    }
    //}

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
        //not allowed because linux android etc uses \ instead of / use path.combine
        //  if (!filename.StartsWith('/')) filename = '/' + filename;
        string fullpath = Path.Combine(path, filename);
        if (File.Exists(fullpath))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data">text data you would like to add to an existing file</param>
    public static void AppendDataToFile(string data,string filename)
    {
        if (PathExists(path))
        File.AppendAllText(path+ filename, JsonConvert.SerializeObject(data));
       
        else Debug.LogWarning("We don't have a valid path! " + path);
    }
}
public class JsonHeader
{
    public int Version;
    public string Lastsaved;
    public Dictionary<string, object> Data; //blackboard data

    public JsonHeader(Dictionary<string, object> data, int version = 1)
    {
        Version = version;
        Lastsaved = System.DateTime.UtcNow.ToString("o"); // ISO 8601
        Data = data;
    }

}