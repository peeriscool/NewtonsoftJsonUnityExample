using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//A peertjuh / pappelflap script

//TODO: add option to make filling fields neccecary/mandatory with a checkbox in the inspector(Like giving a file or player name)
/// <summary>
/// About this script:
/// demo script for reading writing Json data.
/// 
/// Communicates data from unity to the blackboard
/// uses JsonSerializer to serialize data from the blackboard (So blackboard does not depend on Json class and reverse)
///   
/// regions are used to divide the dependicies
/// summaries are used for explenation of the methods
/// 
///         Here are the snippets
///         <Summary> </Summary>
///         #region
///         #endregion
///         
/// </summary>
public class DemoScript : MonoBehaviour
{
    [SerializeField]
    private bool Ismandatory; //if true disable sumbit button until data has been given

    private void Start()
    {
        //explain in the console what we can do
        Debug.Log("Use form to Save and load data from/To Json");
        Data_Available("DefaultFileName");
    }
    private String GetDate()
    {
        return DateTime.Today.ToString();
    }
    private String GetTime()
    {
        return DateTime.Now.ToString();
    }
    private bool Data_Available(string filename)
    {
       // if (Blackboard.Instance.GetData() != null) //wil always exist therfore we aren't able to write
       if(JSONSerializer.FileExists(filename))
        {
            //we are not aloud to create a new Json file use the existing!
            Debug.Log("Earlier data was found!");
            Debug.Log(Application.persistentDataPath.ToString());
            //Searh for the date of the data and display in console
            return true;
        }
        return false;
    }
    #region  //---------------------------------------------------------------------------Unity to blackboard methods-----------------------------------------------------------\\
   
    //<Summary> sets Slider value to blackboard (Key = slider.name)</Summary>
    public void OnValueChanged_Number(Slider slider)
    {
        var value = 0;
        if (Math.Abs(slider.value % 1) < Double.Epsilon) //value is an integer
        { 
         value = (int) slider.value;
        Blackboard.Instance.SetValue(slider.name, value);
        }
        else
        {
          decimal number = Math.Round((decimal)slider.value, 4); //rounding to 4 decimals, design choice not yet substantiated
          Blackboard.Instance.SetValue<decimal>(slider.name, number);
        }
       
    }
    ///<Summary> sets TMP_Text String to blackboard (Key = TMP_Text.name)</Summary>
    public void OnValueChanged_String(TMP_Text text)
    {
        Blackboard.Instance.SetValue<string>(text.name, text.text);
    }
    #endregion
    #region //---------------------------------------------------------------------------blackboard to JsonSerializer methods-----------------------------------------------------------\\
    //<Summary>confirms we want to send data from blackboard to json </Summary>
    public void SumbitData()
    {
        if (Data_Available("DefaultFileName"))
        {
            //if we gots data in the blackboard save it to json
            if(true)
            {
                SaveToJson("DefaultFileName");
               // JSONSerializer.Save(Blackboard.Instance.GetData(), "DefaultFileName");
            }
            //open file and update values

        }
        else
        {
            //make new savefile and put it in there.
            SaveToJson("DefaultFileName");
            JSONSerializer.Save(Blackboard.Instance.GetData(), "DefaultFileName");
        }
      
      
    }
    ///<Summary> saves all data in the blackboard </Summary>
    private void SaveToJson(string filename)
    {
      JSONSerializer.Save(Blackboard.Instance.GetData(), filename);
    }
    public void Loadsaveddata(TMP_Text textloc)
    {
        Dictionary<string, object> dic = JSONSerializer.Load<Dictionary<string, object>>("DefaultFileName");
        Debug.Log(dic.Values.Count);
        // Blackboard.Instance.SetValue();

        //  Dictionary<string,object> dic = JSONSerializer.Load<Dictionary<string, object>>("DefaultFileName");
         string dictext = dic["Text"] as string;
         textloc.text = dictext;
    
        double ValueSlider = (double)dic["ValueSlider"];
        textloc.text = dictext+ ValueSlider;
    }
    #endregion
}
