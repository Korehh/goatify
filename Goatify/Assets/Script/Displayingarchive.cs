using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
public class Displayingarchive : MonoBehaviour
{
    public Text nameText;
    public Text ageText;
    public Text birthText;
    public Text entryText;
    public Text weightText;
    public Text notesText;
    public Text breedText;
    public Text genderText;
    public Text obtainText;

    private MyData myArchive;

    void Start()
    {
       // string jsonFilePath = "Assets/Archivedata.json";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "Archivedata.json");

        // Read the JSON file
        string jsonString = File.ReadAllText(jsonFilePath);

        // Parse the JSON data
        myArchive = JsonUtility.FromJson<MyData>(jsonString);


        if (myArchive != null && myArchive.dataList != null)
        {
            int targetAge = PlayerPrefs.GetInt("AgePass");

            // Display the data based on the target age
            DisplayDataByAge(targetAge);
        }
        else
        {
            // If no data is available, display a message
            nameText.text = "No Data";
            ageText.text = "";
            birthText.text = "";
            entryText.text = "";
            weightText.text = "";
            notesText.text = "";
            breedText.text = "";
            genderText.text = "";
            obtainText.text = "";
        }
    }

    private void DisplayDataByAge(int targetAge)
{

    if (myArchive != null && myArchive.dataList != null)
    {
        // Find the data item with the target age
        DataArhive targetItem = myArchive.dataList.Find(item => item.age == targetAge);

        if (targetItem != null)
        {
            // Assign the data fields to text objects
            nameText.text = targetItem.name;
            ageText.text = targetItem.age.ToString();
            birthText.text = targetItem.birth;
            entryText.text = targetItem.entry;
            weightText.text = targetItem.weight;
            notesText.text = targetItem.notes;
            breedText.text = targetItem.breed;
            genderText.text = targetItem.gender;
            obtainText.text = targetItem.obtain;
            
        }
        else
        {
            // If no data item with the target age is found, display a message
        
            nameText.text = "No Data";
            ageText.text = "";
            birthText.text = "";
            entryText.text = "";
            weightText.text = "";
            notesText.text = "";
            breedText.text = "";
            genderText.text = "";
            obtainText.text = "";
        }
    }
    else
    {
    
        // If no data is available, display a message
        nameText.text = "No Data";
        ageText.text = "";
        birthText.text = "";
        entryText.text = "";
        weightText.text = "";
        notesText.text = "";
        breedText.text = "";
        genderText.text = "";
        obtainText.text = "";
    }
}
}

[System.Serializable]
public class myArchive
{
    public List<DataItem> dataList;
}

[System.Serializable]
public class DataItem
{
    public string name;
    public int age;
    public string birth;
    public string entry;
    public string weight;
    public string notes;
    public string breed;
    public string gender;
    public string obtain;
    // Add other fields to match your JSON structure
}
