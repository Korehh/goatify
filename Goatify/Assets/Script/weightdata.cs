using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
public class weightdata : MonoBehaviour
{
    public Dropdown nameDropdown;
    public Text ageText;
    public Text birthText;
    public Text entryText;
    public Text weightText;
    public Text notesText;
    public Text breedText;
    public Text genderText;
    public Text obtainText;

    public GameObject dataTB;
    public Transform dataContainer;
    private Dictionary<string, GameObject> dataObjects = new Dictionary<string, GameObject>();
    private MyWeight myWeight;


    void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "Archivedata.json");

        // Read the JSON file
        string jsonString = File.ReadAllText(jsonFilePath);

        // Parse the JSON data
        myWeight = JsonUtility.FromJson<MyWeight>(jsonString);
        nameDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        if (myWeight != null && myWeight.dataList != null)
        {
            // Populate the dropdown with names
            PopulateNameDropdown();

            // Set the initial selection of the dropdown
            nameDropdown.value = 0;

            // Display the data based on the selected name
            DisplayDataByName(nameDropdown.options[0].text);

            CreateDataObjects();
        }
        else
        {
            // If no data is available, display a message
            ClearTextFields();
        }
    }

    private void PopulateNameDropdown()
    {
        // Clear existing options
        nameDropdown.ClearOptions();

        // Create a list to store dropdown options
        List<string> names = new List<string>();

        // Add names from the JSON data to the list
        foreach (WeightData dataItem in myWeight.dataList)
        {
            names.Add(dataItem.name);
        }

        // Set the options of the dropdown
        nameDropdown.AddOptions(names);
    }

    public void OnNameDropdownValueChanged()
    {
        // Get the selected name from the dropdown
        string selectedName = nameDropdown.options[nameDropdown.value].text;

        // Display the data based on the selected name
        DisplayDataByName(selectedName);
    }
    private void OnDropdownValueChanged(int index)
    {
        // Retrieve the selected option from the dropdown
        string selectedOption = nameDropdown.options[index].text;

        // Call your desired method with the selected option
        CreateDataObjects();
    }

    private void DisplayDataByName(string targetName)
    {
        if (myWeight != null && myWeight.dataList != null)
        {
            // Find the data item with the target name
            WeightData targetItem = myWeight.dataList.Find(item => item.name == targetName);

            if (targetItem != null)
            {
                // Assign the data fields to text objects
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
                // If no data item with the target name is found, display a message
                ClearTextFields();
            }
        }
        else
        {
            // If no data is available, display a message
            ClearTextFields();
        }
    }

    private void ClearTextFields()
    {
        ageText.text = "";
        birthText.text = "";
        entryText.text = "";
        weightText.text = "";
        notesText.text = "";
        breedText.text = "";
        genderText.text = "";
        obtainText.text = "";
    }

    private void CreateDataObjects()
    {
        // Clear any existing data objects
        ClearDataObjects();

        // Iterate through the data entries in the JSON
        for (int i = 0; i < myWeight.dataList.Count; i++)
        {
            WeightData data = myWeight.dataList[i];
            string name = data.name;

            // Check if a data object already exists for the name
            if (!dataObjects.ContainsKey(name))
            {
                // Instantiate a new data object from the template
                GameObject newDataObject = Instantiate(dataTB, dataContainer);

                // Assign the corresponding data to the Text components of the new data object
                AssignDataToTextComponents(newDataObject, data);

                // Add the new data object to the dictionary with the name as the key
                dataObjects.Add(name, newDataObject);
            }
        }
    }

    private void AssignDataToTextComponents(GameObject dataObject, WeightData data)
    {
        // Get the Text components of the data object
        Text ageText = dataObject.transform.Find("AgeText").GetComponent<Text>();
        Text birthText = dataObject.transform.Find("BirthText").GetComponent<Text>();
        Text weightText = dataObject.transform.Find("WeightText").GetComponent<Text>();

        // Assign the data to the Text components
        ageText.text = data.age.ToString();
        birthText.text = data.birth;
        weightText.text = data.weight;
    }

    private void ClearDataObjects()
    {
        // Destroy existing data objects
        foreach (KeyValuePair<string, GameObject> kvp in dataObjects)
        {
            Destroy(kvp.Value);
        }

        // Clear the dictionary
        dataObjects.Clear();
    }
}

[System.Serializable]
public class MyWeight
{
    public List<WeightData> dataList;
}

[System.Serializable]
public class WeightData
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