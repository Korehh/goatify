using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class ArchiveDisplayingdata : MonoBehaviour
{
    public GameObject buttonsContainerPrefab;
    public Transform containerParent;
    //private float steadyHeight = 1570f;
    //private float extraItemHeight = 260f; // Set to a positive value here
    private Vector3 initialPosition;

    private List<CustomDataArchive> customDataList = new List<CustomDataArchive>(); // Declare customDataList at the class level

     public RawImage rawImage; // Reference to the RawImage component

    public Toggle allToggle;
    public Toggle kidToggle;
    public Toggle buckToggle;
    public Toggle bucklingToggle;
    public Toggle doeToggle;
    public Toggle doelingToggle;

    public Toggle NonLactatingToggle;
    public Toggle LactatingToggle;
    public Toggle PregnantToggle;
    public InputField searchInputField;
    private void Start()
    {
        

        UpdateGoatDisplay(); // Update the goat display initially
        // Store the initial position of the Container
        initialPosition = new Vector3(containerParent.position.x, 0f, containerParent.position.z);

        string jsonFilePath = Path.Combine(Application.persistentDataPath, "Archivedata.json");

        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            ContainerDataListArchive dataList = JsonUtility.FromJson<ContainerDataListArchive>(jsonString);

            //int totalItems = dataList.dataList.Length;
            //float totalHeight = steadyHeight;

           // if (totalItems > 7)
            //{
            //    totalHeight += (totalItems - 7) * extraItemHeight;
            //    Debug.Log("Total Items: " + totalItems);
            //    Debug.Log("Total Height: " + totalHeight);
            //}
            // Subtract 20 from the total height
            //totalHeight -= 90f;
            foreach (ContainerDataArchive data in dataList.dataList)
            {
                GameObject buttonsContainer = Instantiate(buttonsContainerPrefab, containerParent);

                // Set the properties based on the JSON data
                Text nameText = buttonsContainer.transform.Find("Name Text").GetComponent<Text>();
                Text ageText = buttonsContainer.transform.Find("Age Text").GetComponent<Text>();
                Text genderText = buttonsContainer.transform.Find("Gender Text").GetComponent<Text>();
                nameText.text = data.name;
                ageText.text = data.age.ToString();
                genderText.text = data.gender;
            
                 // Get the Button component from the button container
                Button ageButton = buttonsContainer.GetComponentInChildren<Button>();

                  // Add a RawImage component
                RawImage rawImagePrefab = buttonsContainer.transform.Find("RawImagePrefab").GetComponent<RawImage>();
                RawImage rawImage = Instantiate(rawImagePrefab, buttonsContainer.transform);

                // Store the age and button reference in the custom data structure
                CustomDataArchive CustomDataArchive = new CustomDataArchive();
                CustomDataArchive.age = data.age;
                CustomDataArchive.button = ageButton;
                CustomDataArchive.gender = data.gender;
                CustomDataArchive.rawImage = rawImage;
                CustomDataArchive.stageG = data.stageG;
                CustomDataArchive.statusG = data.statusG;
                CustomDataArchive.name = data.name;
                customDataList.Add(CustomDataArchive);

                // Add onClick event to the button to handle the click event
                ageButton.onClick.AddListener(() => OnAgeButtonClick(ageText.text));
                //innerButton.onClick.AddListener(() => OnInnerButtonClick(ageText.text));

              Debug.Log("Gender: " + data.stageG); // Debug statement to check the gender value

            if (data.stageG == "Wether" || data.stageG == "Buckling" || data.stageG == "Buck")
            {
                rawImage.texture = Resources.Load<Texture2D>("Images/StageImg/malegoat");
            }
            else if (data.stageG == "Doeling" || data.stageG == "Doe")
            {
                rawImage.texture = Resources.Load<Texture2D>("Images/StageImg/femalegoat");
            }
            else
            {
                rawImage.texture = Resources.Load<Texture2D>("Images/StageImg/kidgoat");
            }

            // Debug statement to check the loaded image path
            Debug.Log("Loaded Image Path: " + rawImage.texture);
            }

            // Set the height of the Container to match the total height required
            //RectTransform containerRect = containerParent.GetComponent<RectTransform>();
            //Vector2 containerSize = containerRect.sizeDelta;
            //containerSize.y = totalHeight;
            //containerRect.sizeDelta = containerSize;
            RectTransform containerRect = containerParent.GetComponent<RectTransform>();
            containerRect.pivot = new Vector2(0.5f, 1f);
            containerRect.anchorMin = new Vector2(0.5f, 1f);
            Vector2 containerSize = containerRect.sizeDelta;
            containerSize.y = 1400; // Set the height to 1400
            containerRect.sizeDelta = containerSize;

            // Reset the position of the Container to the initial static position
            containerParent.position = initialPosition;
        }
        else
        {
            Debug.LogError("GoatInfo.json not found in the persistent data path.");
        }

       allToggle.onValueChanged.AddListener(OnToggleValueChanged);
        kidToggle.onValueChanged.AddListener(OnToggleValueChanged);
        buckToggle.onValueChanged.AddListener(OnToggleValueChanged);
        doelingToggle.onValueChanged.AddListener(OnToggleValueChanged);
        
        bucklingToggle.onValueChanged.AddListener(OnToggleValueChanged);
        doeToggle.onValueChanged.AddListener(OnToggleValueChanged);
        PregnantToggle.onValueChanged.AddListener(OnToggleValueChanged);
        NonLactatingToggle.onValueChanged.AddListener(OnToggleValueChanged);
        LactatingToggle.onValueChanged.AddListener(OnToggleValueChanged);

        // Initially, update the display to show all goats
        searchInputField.onValueChanged.AddListener(OnSearchBarValueChanged);
        UpdateGoatDisplay();
    }
    public void OnAgeButtonClick(string ageText)
    {
        // Extract the age value from the ageText string
        int age;
        if (int.TryParse(ageText.Replace("Age: ", ""), out age))
        {
            PlayerPrefs.SetInt("AgePass", age);
            SceneManager.LoadScene("DisplayArchive");

            Debug.Log("Age saved to PlayerPrefs: " + age);
        }
        else
        {
            Debug.LogError("Failed to parse age from the button text: " + ageText);
        }
    }


     private void OnToggleValueChanged(bool isOn)
    {
        UpdateGoatDisplay();

         foreach (CustomDataArchive CustomDataArchive in customDataList)
    {
        Debug.Log("Goat: " + CustomDataArchive.stageG + ", Should Display: " + ShouldDisplayGoat(CustomDataArchive.stageG));
    }
    }

     private void UpdateGoatDisplay()
{
    string searchText = searchInputField.text.ToLower(); // Convert to lowercase for case-insensitive search
    foreach (CustomDataArchive CustomDataArchive in customDataList)
    {
        bool shouldDisplay = ShouldDisplayGoat(CustomDataArchive.stageG);
        bool shouldDisplaySearch = SearchMatches(CustomDataArchive, searchText);
        bool shouldDisplayStatus = ShouldDisplayGoatStatus(CustomDataArchive.statusG, CustomDataArchive.gender);

        // Debug log statements to check conditions
        Debug.Log("Name: " + CustomDataArchive.name);
        Debug.Log("Should Display: " + shouldDisplay);
        Debug.Log("Should Display Status: " + shouldDisplayStatus);
        Debug.Log("Should Display Search: " + shouldDisplaySearch);

        bool finalDisplayCondition = (shouldDisplay || shouldDisplayStatus) && shouldDisplaySearch;

        CustomDataArchive.rawImage.gameObject.SetActive(finalDisplayCondition);
        CustomDataArchive.button.gameObject.SetActive(finalDisplayCondition);
    
    }
}

   private bool ShouldDisplayGoat(string stageG)
    {
        if (allToggle.isOn)
            return true;
        else if (kidToggle.isOn && (stageG == "Kid"))
            return true;
        else if (buckToggle.isOn && (stageG == "Buck"))
            return true;
        else if (doelingToggle.isOn && (stageG == "Doeling"))
            return true;
        else if (doeToggle.isOn && (stageG == "Doe"))
            return true;
        else if (bucklingToggle.isOn && (stageG == "Buckling"))
            return true;
        
        return false;
    }

    private bool ShouldDisplayGoatStatus(string statusG, string gender)
    {
        Debug.Log("Checking ShouldDisplayGoatStatus: " + statusG);
        
        if (allToggle.isOn)
            return true;
        else if (NonLactatingToggle.isOn && gender == "Female" && statusG == "Non-Lactating")
            return true;
        else if (LactatingToggle.isOn && statusG == "Lactating")
            return true;
        else if (PregnantToggle.isOn && statusG == "Pregnant")
            return true;
            
        return false;
    }

    public void OnSearchBarValueChanged(string searchText)
    {
        UpdateGoatDisplay();
    }
    private bool SearchMatches(CustomDataArchive CustomDataArchive, string searchText)
    {
        return (CustomDataArchive?.name?.ToLower().Contains(searchText) ?? false) ||
            (CustomDataArchive?.age.ToString()?.Contains(searchText) ?? false);
    }

    public void ClearSearchInputField()
    {
        searchInputField.text = ""; // Clear the text of the input field
    }

}

[System.Serializable]
public class ContainerDataListArchive
{
    public ContainerDataArchive[] dataList;
}

[System.Serializable]
public class ContainerDataArchive
{
    public string name;
    public int age;
    public string gender;
    public string stageG;
    public string statusG;
}
[System.Serializable]
public class CustomDataArchive
{
    public int age;
    public string name;
    public string stageG;
    public string statusG;
    public string gender;
    public Button button;
    public Button innerButton;
    public RawImage rawImage;
    
}