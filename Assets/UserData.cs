using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Script that saves userdata in to Json so it is actualy saved!
/// </summary>
[System.Serializable]
public class UserData 
{
    public string PlayerName { get { return PlayerName; } set { PlayerName = value; } }
    public int GamesPlayed { get { return GamesPlayed; } set {GamesPlayed=value;} } //use set with gamesplayed = gamesplayed+1
    public List<int> Scores;
    public string iconLocation;
    [System.NonSerialized]
    Image icon;

    public void loadExistingData(string path)
    {
        JSONSerializer.Load<UserData>(path);
    }

    public void saveData(string Filename)
    {
        JSONSerializer.Save<UserData>(this, Filename);
    }

    public void SaveUserImage(Image _icon)
    {
        icon = _icon;
        iconLocation = Path.Combine( Application.persistentDataPath ,_icon.name);
        Debug.Log(iconLocation);
    }
}
