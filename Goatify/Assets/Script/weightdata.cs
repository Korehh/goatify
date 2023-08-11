using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
public class weightdata : MonoBehaviour
{
    public Dropdown nameDropdown;
    public Text dateText;
    public Text notesText;
    public Text weightText;

    public InputField dateInput;
    public InputField weightInput;
    public InputField notesInput;

    public GameObject dataTB;
    public Transform dataContainer;
    private Dictionary<string, GameObject> dataObjects = new Dictionary<string, GameObject>();
    private MyWeight myWeight;

    private string jsonFilePath;

    void Start()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "Weightdata.json");

        // Read the JSON file
        string jsonString = File.ReadAllText(jsonFilePath);

        // Parse the JSON data
        myWeight = JsonUtility.FromJson<MyWeight>(jsonString);
        nameDropdown.onValueChanged.AddListener(OnNameDropdownValueChanged);
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

    public void OnNameDropdownValueChanged(int index)
    {
        // Get the selected name from the dropdown
        string selectedName = nameDropdown.options[index].text;

        // Display the data based on the selected name
        DisplayDataByName(selectedName);

        // Update the data objects
        CreateDataObjects();
    }

    private void DisplayDataByName(string targetName)
    {
        if (myWeight != null && myWeight.dataList != null)
        {
            // Find the data item with the target name
            WeightData targetItem = myWeight.dataList.Find(item => item.name == targetName);

            if (targetItem != null && targetItem.details != null && targetItem.details.Count > 0)
            {
                // Display the first detail entry
                DisplayDetailData(targetItem.details[0]);
            }
            else
            {
                // If no detail data is available, display a message
                ClearTextFields();
            }
        }
        else
        {
            // If no data is available, display a message
            ClearTextFields();
        }
    }

    private void DisplayDetailData(DetailData detailData)
    {
        // Assign the data fields to text objects
        dateText.text = detailData.date;
        notesText.text = detailData.notes;
        weightText.text = detailData.weight.ToString();
    }

    private void ClearTextFields()
    {
        dateText.text = "";
        notesText.text = "";
        weightText.text = "";
    }

    public void SaveData()
    {
        // Retrieve the entered data from the UI fields
        string name = nameDropdown.options[nameDropdown.value].text;
        string date = dateInput.text;
        string notes = notesInput.text;
        float weight = float.Parse(weightInput.text);

        // Create a new DetailData object
        DetailData newData = new DetailData()
        {
            date = date,
            notes = notes,
            weight = weight
        };

        // Find the existing data item with the same name
        WeightData existingData = myWeight.dataList.Find(item => item.name == name);

        if (existingData != null)
        {
            // Add the new detail data to the existing data item
            existingData.details.Add(newData);
        }
        else
        {
            // Create a new WeightData object
            WeightData newWeightData = new WeightData()
            {
                name = name,
                details = new List<DetailData> { newData }
            };

            // Add the new WeightData to the dataList
            myWeight.dataList.Add(newWeightData);
        }

        // Convert the dataList to JSON format
        string jsonData = JsonUtility.ToJson(myWeight);

        // Write the JSON data to the file
        File.WriteAllText(jsonFilePath, jsonData);

        // Update the dropdown options
        PopulateNameDropdown();

        // Update the data objects
        CreateDataObjects();
    }

 private void CreateDataObjects()
    {
        // Clear any existing data objects
        ClearDataObjects();

        // Get the selected name from the dropdown
        string selectedName = nameDropdown.options[nameDropdown.value].text;

        // Find the data item with the selected name
        WeightData selectedData = myWeight.dataList.Find(item => item.name == selectedName);

        if (selectedData != null)
        {
            for (int i = 0; i < selectedData.details.Count; i++)
            {
                // Instantiate a new data object from the template
                GameObject newDataObject;

                if (i == 0)
                {
                    newDataObject = Instantiate(dataTB, dataContainer);
                }
                else
                {
                    newDataObject = Instantiate(dataTB, dataContainer.transform);
                    newDataObject.name = dataTB.name + " clone " + i;
                }

                // Assign the corresponding data to the Text components of the new data object
                AssignDataToTextComponents(newDataObject, selectedData.details[i]);

                // Add the new data object to the dictionary with a unique key
                string dataKey = selectedName + "_" + i;
                dataObjects.Add(dataKey, newDataObject);
            }
        }
    }
    private void AssignDataToTextComponents(GameObject dataObject, DetailData detailData)
    {
        // Get the Text components of the data object
        Text dateText = dataObject.transform.Find("DateText").GetComponent<Text>();
        Text notesText = dataObject.transform.Find("NotesText").GetComponent<Text>();
        Text weightText = dataObject.transform.Find("WeightText").GetComponent<Text>();

        // Assign the data to the Text components
        dateText.text = detailData.date;
        notesText.text = detailData.notes;
        weightText.text = detailData.weight.ToString();
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

    public void dateChoose(){
        Debug.Log("date method called");
        dateInput.text = DatePickerControl.dateStringFormato;
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
    public List<DetailData> details;
}

[System.Serializable]
public class DetailData
{
    public string date;
    public string notes;
    public float weight;
}