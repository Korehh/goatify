using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EasyUI.Popup ;
using System.Linq;

public class savingdata : MonoBehaviour
{
    public InputField nameInputField;
    public InputField ageInputField;
    public InputField agedisplayInputField;
    public InputField datebirthInputField;
    public InputField dateentryageInputField;
    public InputField weightInputField;
    public InputField notesInputField;
    public Dropdown breedDropdown;
    public Dropdown genderDropdown;
    public Dropdown obtainDropdown;

    public InputField otherinputField;

    public Text nameText;
    public Text ageText;
    public Text dateText;
    public Text dateentryageText;
    public Text weightText;
    public Text notesText;
    public Text breedText;
    public Text genderText;
    public Text obtainText;

    private string dataFilePath;
    private string dataFilePathArchive;
    private List<GoatData> dataList;

    
    private MyDataGoat myData;
    //private DatePicker datePicker;

    private void Awake()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "GoatInfo.json");
        dataFilePathArchive = Path.Combine(Application.persistentDataPath, "Archivedata.json");
    }

    private void Start()
    {

        dataList = new List<GoatData>();
        LoadData();
        int targetAge = PlayerPrefs.GetInt("AgePass");
        DisplayDataByAge(targetAge);
         obtainDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        // datePicker.SelectedDate = DateTime.Today;
        int randomNum = Random.Range(1000, 10000);
        ageInputField.text = randomNum.ToString();
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(new GoatDataListWrapper(dataList));
        File.WriteAllText(dataFilePath, json);
    }

    private void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            GoatDataListWrapper dataListWrapper = JsonUtility.FromJson<GoatDataListWrapper>(json);
            dataList = dataListWrapper.dataList;
        }
        else
        {
            dataList = new List<GoatData>();
        }
    }
    public void AddData()
    {
        string name = nameInputField.text;
        int age = int.Parse(ageInputField.text);
        string birth = datebirthInputField.text;
        string entry = dateentryageInputField.text;
        string weight = weightInputField.text;
        string notes = notesInputField.text;
        string breed = breedDropdown.options[breedDropdown.value].text;
        string gender = genderDropdown.options[genderDropdown.value].text;
        //string obtain = obtainDropdown.options[obtainDropdown.value].text;
        string obtain;
        if (obtainDropdown.options[obtainDropdown.value].text == "Other")
            {
                 obtain = otherinputField.text;
            }
            else
            {
                
                 obtain = obtainDropdown.options[obtainDropdown.value].text;
            }


        GoatData newData = new GoatData(name, age, birth, entry, weight, notes, breed, gender, obtain);
        dataList.Add(newData);

        SaveData();

        nameInputField.text = "No Data";
            ageInputField.text = "";
            datebirthInputField.text = "";
            dateentryageInputField.text = "";
            weightInputField.text = "";
            notesInputField.text = "";

            SetDropdownValue(breedDropdown, "");
            SetDropdownValue(genderDropdown, "");
            SetDropdownValue(obtainDropdown, "");
        
    }
   
    public void AddArchives()
    {
        int targetAge = PlayerPrefs.GetInt("AgePass");

        // Load the existing data from the archive file, if it exists
        List<GoatData> archiveList = new List<GoatData>();
        if (File.Exists(dataFilePathArchive))
        {
            string json = File.ReadAllText(dataFilePathArchive);
            GoatDataListWrapper dataListWrapper = JsonUtility.FromJson<GoatDataListWrapper>(json);
            archiveList = dataListWrapper.dataList;
        }

        // Find the data item with the target age
        GoatData targetData = dataList.Find(item => item.age == targetAge);

        if (targetData != null)
        {
            // Add the target data item to the archive list
            archiveList.Add(targetData);

            // Serialize the archive list to JSON
            string json = JsonUtility.ToJson(new GoatDataListWrapper(archiveList));

            // Write the JSON data to the archive file
            File.WriteAllText(dataFilePathArchive, json);

            Debug.Log("Data item added to archive successfully!");
        }
        else
        {
            Debug.Log("No data item found with the specified age: " + targetAge);
        }

        DeleteDataByAge(targetAge);
    }
    
   public void UpdateDataByAgeFromInputField()
    {
        int targetAge = PlayerPrefs.GetInt("AgePass");
        UpdateDataByAge(targetAge);
    }

   public void UpdateDataByAge(int targetAge)
    {
        // Find the data item with the target age
        GoatData targetData = dataList.Find(item => item.age == targetAge);

        if (targetData != null)
        {
            // Perform the update based on your requirements
            // For example, let's update the name and weight fields
            targetData.name = nameInputField.text;
            targetData.weight = weightInputField.text;
            targetData.birth = datebirthInputField.text;
            targetData.entry = dateentryageInputField.text;
            targetData.notes = notesInputField.text;
            targetData.breed = breedDropdown.options[breedDropdown.value].text;
            targetData.gender = genderDropdown.options[genderDropdown.value].text;
            //targetData.obtain = obtainDropdown.options[obtainDropdown.value].text;
            if (obtainDropdown.options[obtainDropdown.value].text == "Other")
            {
                targetData.obtain = otherinputField.text;
            }
            else
            {
                
                targetData.obtain = obtainDropdown.options[obtainDropdown.value].text;
            }

            // Save the updated data
            SaveData();

            Debug.Log("Data item updated successfully!");
        }
        else
        {
            Debug.Log("No data item found with the specified age: " + targetAge);
        }
    }

   private void DisplayDataByAge(int targetAge)
    {
        // Find the data item with the target age
        GoatData targetItem = dataList.Find(item => item.age == targetAge);

        if (targetItem != null)
        {
            // Assign the data fields to text objects
            nameInputField.text = targetItem.name;
            agedisplayInputField.text = targetItem.age.ToString();
            datebirthInputField.text = targetItem.birth;
            dateentryageInputField.text = targetItem.entry;
            weightInputField.text = targetItem.weight;
            notesInputField.text = targetItem.notes;

            SetDropdownValue(breedDropdown, targetItem.breed);
            SetDropdownValue(genderDropdown, targetItem.gender);
           
            if (targetItem.obtain == "Born of farm" || targetItem.obtain == "Purchased")
            {
                // Show the input field
                 otherinputField.gameObject.SetActive(false);
                SetDropdownValue(obtainDropdown, targetItem.obtain);

            }
            else
            {
                // Hide the input field
                
                SetDropdownValue(obtainDropdown, "Other");
                otherinputField.gameObject.SetActive(true);
                otherinputField.text = targetItem.obtain;
            }
        }
        else
        {
            // If no data item with the target age is found, display a message
            nameInputField.text = "No Data";
            agedisplayInputField.text = "";
            datebirthInputField.text = "";
            dateentryageInputField.text = "";
            weightInputField.text = "";
            notesInputField.text = "";

            SetDropdownValue(breedDropdown, "");
            SetDropdownValue(genderDropdown, "");
            SetDropdownValue(obtainDropdown, "");
        }
    }

    private void SetDropdownValue(Dropdown dropdown, string value)
    {
        int index = dropdown.options.FindIndex(option => option.text == value);
        dropdown.value = index;
    }
    public void DeleteDataByAge(int targetAge)
    {
        // Find the index of the data item with the specified age
        int index = dataList.FindIndex(item => item.age == targetAge);

        if (index != -1)
        {
            // Remove the data item from the list
            dataList.RemoveAt(index);

            // Save the updated data
            SaveData();

            Debug.Log("Data item deleted successfully!");
        }
        else
        {
            Debug.Log("No data item found with the specified age: " + targetAge);
        }
    }
     private void OnDropdownValueChanged(int value)
        {
            // Check if the selected option is "Other"
            if (obtainDropdown.options[value].text == "Other")
            {
                // Show the input field
                otherinputField.gameObject.SetActive(true);
            }
            else
            {
                // Hide the input field
                otherinputField.gameObject.SetActive(false);
            }
        }
    public void birthChoose(){
        Debug.Log("birthChoose method called");
        datebirthInputField.text = DatePickerControl.dateStringFormato;
    }
    public void EntryChoose(){
        Debug.Log("EntryChoose method called");
        dateentryageInputField.text = DatePickerControl.dateStringFormato;
    }
}


[System.Serializable]
public class GoatData
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

    public GoatData(string name, int age, string birth, string entry, string weight, string notes, string breed, string gender, string obtain)
    {
        this.name = name;
        this.age = age;
        this.birth = birth;
        this.entry = entry;
        this.weight = weight;
        this.notes = notes;
        this.breed = breed;
        this.gender = gender;
        this.obtain = obtain;
    }
}

[System.Serializable]
public class GoatDataListWrapper
{
    public List<GoatData> dataList;

    public GoatDataListWrapper(List<GoatData> dataList)
    {
        this.dataList = dataList;
    }
}

[System.Serializable]
    public class MyDataGoat
    {
        public List<GoatData> dataList;
    }

