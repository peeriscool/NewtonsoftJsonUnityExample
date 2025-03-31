using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://gamedevbeginner.com/how-to-use-player-prefs-in-unity/
public class Playerprefs : MonoBehaviour
{
    [SerializeField] 
    [Range(1,100)]
    private int value;

    //saves data between play sessions
    public void SaveNumber(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public void SaveNumber(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public int LoadNumber(string key, int value)
    {
       return value = PlayerPrefs.GetInt(key);
    }
    public float LoadNumber(string key, float value)
    {
        return value = PlayerPrefs.GetFloat(key);
    }

    public void SaveText(string key, string text)
    {
        PlayerPrefs.SetString(key, text);
    }
    public void GetText(string key, string text)
    {
        PlayerPrefs.GetString(key, text);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }


    //example code
    /*
public int slot;

public float sfxVolume;
public float musicVolume;
public float dialogueVolume;

public void LoadSettings()
{
    sfxVolume = PlayerPrefs.GetFloat("sfxVolume" + slot);
    musicVolume = PlayerPrefs.GetFloat("musicVolume" + slot);
    dialogueVolume = PlayerPrefs.GetFloat("dialogueVolume" + slot);
}

public void SaveSettings()
{
    PlayerPrefs.SetFloat("sfxVolume" + slot, sfxVolume);
    PlayerPrefs.SetFloat("musicVolume" + slot, musicVolume);
    PlayerPrefs.SetFloat("dialogueVolume" + slot, dialogueVolume);
}
*/
}
