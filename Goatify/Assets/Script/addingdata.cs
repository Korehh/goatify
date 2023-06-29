using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class addingdata : MonoBehaviour
{
    public InputField nameInputField;
    public InputField ageInputField;

    public InputField datebirthInputField; // change date picker 
    public InputField dateentryageInputField;// change date picker
    public InputField weightInputField;
    public InputField notesInputField;


    public Dropdown breedDropdown;
    public Dropdown genderDropdown;
    public Dropdown obtainDropdown;

    public GameObject dataTextPrefab; // Reference to the Text prefab
    public Transform dataTextContainer; // Reference to the parent Transform for the created Text objects
    private string dataFilePath;
    private List<Data> dataList;

    private void Awake()
    {
        //dataFilePath = Path.Combine(Application.persistentDataPath, "GoatInfo.json");
        dataFilePath = Path.Combine(Application.persistentDataPath, "GoatInfo.json");
    }

    private void Start()
    {
        dataList = new List<Data>();
        LoadData();
        ExampleUsage();
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(new DataListWrapper(dataList));
        File.WriteAllText(dataFilePath, json);
    }

    private void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            DataListWrapper dataListWrapper = JsonUtility.FromJson<DataListWrapper>(json);
            dataList = dataListWrapper.dataList;
        }
        else
        {
            dataList = new List<Data>();
        }
    }

    public void AddData()
    {   
        int selectedBreed = breedDropdown.value;
        int selectedGender = genderDropdown.value;
        int selectedObtain = obtainDropdown.value;

        string breedselectedText = breedDropdown.options[selectedBreed].text;
        string genderselectedText = genderDropdown.options[selectedGender].text;
        string obtainselectedText =  obtainDropdown.options[selectedObtain].text;

        string name = nameInputField.text;
        int age = int.Parse(ageInputField.text);

        string birth= datebirthInputField.text;
        string entry = dateentryageInputField.text;
        string weight = weightInputField.text;
        string notes = notesInputField.text;

        string breed = breedselectedText;
        string gender = genderselectedText;
        string obtain =  obtainselectedText;


        Data newData = new Data(name, age, birth,entry,weight,notes, breed, gender, obtain );
        dataList.Add(newData);

        SaveData();
    }

    // Example usage
    private void ExampleUsage()
    {

    foreach (Transform child in dataTextContainer)
        {
            Destroy(child.gameObject);
        }

    float yOffset = 0f; // Vertical offset between Text objects

    // Create Text objects for each data entry
    foreach (Data data in dataList)
        {
            // Instantiate the dataTextPrefab
            GameObject dataTextObject = Instantiate(dataTextPrefab, dataTextContainer);
            Text dataText = dataTextObject.GetComponent<Text>();
            dataText.text = "Name: " + data.name +" "+ "No.: " + data.age;

            // Position the Text object
            RectTransform textTransform = dataTextObject.GetComponent<RectTransform>();
            textTransform.anchoredPosition = new Vector2(0f, yOffset);

            // Create a button as a child of the Text object
            GameObject buttonObject = new GameObject("Button");
            buttonObject.transform.SetParent(dataTextObject.transform, false);

            // Add the button component and set its properties
            Button button = buttonObject.AddComponent<Button>();
            button.onClick.AddListener(() => OnButtonClick(data.name, data.age));

            // Add a Text component to the button and set its properties
            Text buttonText = buttonObject.AddComponent<Text>();
            buttonText.text = "Click";
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.fontSize = 40; // Set the text size here
            

             // Position and size the button within the Text object
                 RectTransform buttonTransform = buttonObject.GetComponent<RectTransform>();
                buttonTransform.anchorMin = new Vector2(0f, 0f); // Align top-left
                buttonTransform.anchorMax = new Vector2(0f, 0f); // Align top-left
                buttonTransform.pivot = new Vector2(0f, 0f); // Set pivot to top-left
                buttonTransform.anchoredPosition = new Vector2(700f, 75f); // Set the position of the button

                buttonTransform.sizeDelta = new Vector2(150f, 80f); // Set the button size here

            // Increase the vertical offset
            yOffset -= 100f; // Adjust this value to control the spacing between Text objects
        }
    
    }

    
    private void OnButtonClick(string name, int age)
    {
        PlayerPrefs.SetString("NamePass", name);
        PlayerPrefs.SetInt("AgePass", age);
        //SceneManager.LoadScene("UpdateGoat");
        SceneManager.LoadScene("DisplayGoat");
        // inputField.text = PlayerPrefs.GetString("INamePassnput");
        
    }
    //udpdating data 
    //public int GetIndexByName(string name)
    public int GetIndexByAge(int age)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].age == age)
            {
                return i; // Return the index of the matching entry
            }
        }
        
        return -1; // Return -1 if the entry with the specified name is not found
    }
    public Button updateButton;
    public void SearchAndDisplayData()
    {   
        
        //string name = nameInputField.text;
        int age = int.Parse(ageInputField.text);
        int index = GetIndexByAge(age);// tag number will replace this 
        
        if (index >= 0)
        {
            Data data = dataList[index];
            //nameText.text = "Name: " + data.name;
            //ageText.text = "Age: " + data.age.ToString();
            
            // Set the input field values to the current data
            nameInputField.text = data.name;
            ageInputField.text = data.age.ToString();
            
            // Set the update button to update the data at the current index
            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() => UpdateData(index));
        }
        else
        {
            //nameText.text = "Name: Not Found";
            //ageText.text = "Age: Not Found";
            
            // Clear the input field values
            nameInputField.text = "";
            ageInputField.text = "";
            
            // Set the update button to do nothing (no data to update)
            updateButton.onClick.RemoveAllListeners();
        }
    }

    public void UpdateData(int index)
    {
        string name = nameInputField.text;
        int age = int.Parse(ageInputField.text);

        // Update the data at the specified index
        dataList[index].name = name;
        dataList[index].age = age;

        SaveData();
    }
}

[System.Serializable]
public class Data
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

    public Data(string name, int age,string birth,string entry,string weight,string notes,string breed,string gender,string obtain)
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
public class DataListWrapper
{
    public List<Data> dataList;

    public DataListWrapper(List<Data> dataList)
    {
        this.dataList = dataList;
    }
}
