using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class JSwriter : MonoBehaviour
{
    public InputField idInputField;
    public InputField nameInputField;
    //public InputField genderInputField;
    public InputField WeightInputField;
 
    public void SaveToJson()
    {
        GoatInfoData data = new GoatInfoData();
        data.Id = idInputField.text;
        data.Name = nameInputField.text;
        //data.Gender = genderInputField.text;
        data.Weight = WeightInputField.text;
 
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/GoatInfoFile.json", json);
    }
 
    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/GoatInfoFile.json");
        GoatInfoData data = JsonUtility.FromJson<GoatInfoData>(json);
 
        idInputField.text = data.Id;
        nameInputField.text = data.Name;
        //genderInputField.text = data.Gender;
        WeightInputField.text = data.Weight;
    }

}
