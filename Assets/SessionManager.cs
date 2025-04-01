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
public class SessionManager : MonoBehaviour
{
    //[SerializeField]
    //private bool Ismandatory; //if true disable sumbit button until data has been given

    [SerializeField] private string saveFileName = "FormData.json";

    private void Start()
    {
        //explain in the console what we can do
        Debug.Log($"Files for writing = {saveFileName}");
        Data_Available(saveFileName);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //enter data
            Quaternion Rotation= GameObject.Find("DataObject").gameObject.transform.rotation;
            Vector3 position = GameObject.Find("DataObject").gameObject.transform.position;
            Blackboard.SetValue<Vector3>("Myposition",position);
            Blackboard.SetValue<Quaternion>("Myrotation", Rotation);

            JSONSerializer.Save<Dictionary<string,object>>(Blackboard.GetData(), saveFileName, 0);
            Debug.Log("saved");
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            JSONSerializer.AppendDataToFile("Identifiername","Ik wil er ook bij!",saveFileName);
        }
    }
    private bool Data_Available(string filename)
    {
       // if (Blackboard.Instance.GetData() != null) //wil always exist therfore we aren't able to write
       if(JSONSerializer.FileExists(filename))
        {
            //we are not allowed to create a new Json file use the existing!
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
        Blackboard.SetValue(slider.name, value);
        }
        else
        {
          decimal number = Math.Round((decimal)slider.value, 4); //rounding to 4 decimals, design choice not yet substantiated
          Blackboard.SetValue<decimal>(slider.name, number);
        }
    }
    ///<Summary> sets TMP_Text String to blackboard (Key = TMP_Text.name)</Summary>
    public void OnValueChanged_String(TMP_Text text)
    {
        Blackboard.SetValue<string>(text.name, text.text);
    }
    #endregion
    #region //---------------------------------------------------------------------------blackboard to JsonSerializer methods-----------------------------------------------------------\\
    //<Summary>confirms we want to send data from blackboard to json </Summary>
    public void SumbitData()
    {
        if (Data_Available(saveFileName))
        {
            //if we gots data in the blackboard save it to json
            if(true)
            {
                SaveToJson(saveFileName);
               // JSONSerializer.Save(Blackboard.Instance.GetData(), "DefaultFileName");
            }
            //open file and update values
        }
        else
        {
            //make new savefile and put it in there.
            SaveToJson(saveFileName);
            JSONSerializer.Save(Blackboard.GetData(), saveFileName);
        }
    }
    ///<Summary> saves all data in the blackboard </Summary>
    private void SaveToJson(string filename)
    {
      JSONSerializer.Save(Blackboard.GetData(), filename);
    }
    public void Loadsaveddata(TMP_Text textloc)
    {
        Dictionary<string, object> dic = JSONSerializer.Load<Dictionary<string, object>>(saveFileName);
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
