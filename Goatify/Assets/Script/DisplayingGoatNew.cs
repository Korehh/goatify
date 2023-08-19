using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class DisplayingGoatNew : MonoBehaviour
{
    public GameObject buttonsContainerPrefab;
    public GameObject editAndViewButtonPrefab;
    public Transform containerParent;
    //private float steadyHeight = 1570f;
    //private float extraItemHeight = 260f; // Set to a positive value here
    private Vector3 initialPosition;

    private List<CustomData> customDataList = new List<CustomData>(); // Declare customDataList at the class level

    //public RawImage rawImage; // Reference to the RawImage component
     public RawImage rawImage; // Reference to the RawImage component

    public Toggle allToggle;
    public Toggle kidToggle;
    public Toggle buckToggle;
    public Toggle doelingToggle;

    public InputField searchInputField;
    private void Start()
    {
        

        UpdateGoatDisplay(); // Update the goat display initially
        // Store the initial position of the Container
        initialPosition = new Vector3(containerParent.position.x, 0f, containerParent.position.z);

        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GoatInfo.json");

        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            ContainerDataList dataList = JsonUtility.FromJson<ContainerDataList>(jsonString);

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
            foreach (ContainerData data in dataList.dataList)
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

                 // Duplicate the inner button
                Button innerButtonPrefab = buttonsContainer.transform.Find("InnerButtonPrefab").GetComponent<Button>();
                Button innerButton = Instantiate(innerButtonPrefab, buttonsContainer.transform);

                  // Add a RawImage component
                RawImage rawImagePrefab = buttonsContainer.transform.Find("RawImagePrefab").GetComponent<RawImage>();
                RawImage rawImage = Instantiate(rawImagePrefab, buttonsContainer.transform);

                // Store the age and button reference in the custom data structure
                CustomData customData = new CustomData();
                customData.age = data.age;
                customData.button = ageButton;
                customData.innerButton = innerButton;
                customData.rawImage = rawImage;
                customData.stageG = data.stageG;
                customData.name = data.name;
                customDataList.Add(customData);

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

            GameObject editAndViewButton = Instantiate(editAndViewButtonPrefab, containerParent);

             Button editButton = editAndViewButton.transform.Find("EditButton").GetComponent<Button>();
            Button viewButton = editAndViewButton.transform.Find("ViewButton").GetComponent<Button>();

            Text ageTextComponent = ageText; // Assuming ageText is the reference to the Text component
            editButton.onClick.AddListener(() => OnEditButtonClick(ageTextComponent));
            viewButton.onClick.AddListener(() => OnViewButtonClick(ageTextComponent));
            }

            // Set the height of the Container to match the total height required
            //RectTransform containerRect = containerParent.GetComponent<RectTransform>();
            //Vector2 containerSize = containerRect.sizeDelta;
            //containerSize.y = totalHeight;
            //containerRect.sizeDelta = containerSize;
            RectTransform containerRect = containerParent.GetComponent<RectTransform>();
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
            SceneManager.LoadScene("DisplayGoatF");

            Debug.Log("Age saved to PlayerPrefs: " + age);
        }
        else
        {
            Debug.LogError("Failed to parse age from the button text: " + ageText);
        }
    }

    public void OnEditButtonClick(Text ageTextComponent)
    {
        if (int.TryParse(ageTextComponent.text.Replace("Age: ", ""), out int age))
        {
            PlayerPrefs.SetInt("AgePass", age);
            SceneManager.LoadScene("UpdateGoatF");

            Debug.Log("Age saved to PlayerPrefs: " + age);
        }
        else
        {
            Debug.LogError("Failed to parse age from the UI text: " + ageTextComponent.text);
        }
    }


   public void OnViewButtonClick(Text ageTextComponent)
    {
        if (int.TryParse(ageTextComponent.text.Replace("Age: ", ""), out int age))
        {
            PlayerPrefs.SetInt("AgePass", age);
            SceneManager.LoadScene("DisplayGoatF");

            Debug.Log("Age saved to PlayerPrefs: " + age);
        }
        else
        {
            Debug.LogError("Failed to parse age from the UI text: " + ageTextComponent.text);
        }
    }

     private void OnToggleValueChanged(bool isOn)
    {
        UpdateGoatDisplay();

         foreach (CustomData customData in customDataList)
    {
        Debug.Log("Goat: " + customData.stageG + ", Should Display: " + ShouldDisplayGoat(customData.stageG));
    }
    }

    private void UpdateGoatDisplay()
    {
        string searchText = searchInputField.text.ToLower(); // Convert to lowercase for case-insensitive search
        foreach (CustomData customData in customDataList)
        {
            // Check if the goat should be displayed based on the selected toggle
            bool shouldDisplay = ShouldDisplayGoat(customData.stageG) && (SearchMatches(customData, searchText));

            // Set the game objects to active or inactive based on the shouldDisplay value
            customData.rawImage.gameObject.SetActive(shouldDisplay);
            customData.button.gameObject.SetActive(shouldDisplay);
            customData.innerButton.gameObject.SetActive(shouldDisplay);
        }
    }

    private bool ShouldDisplayGoat(string stageG)
    {
        if (allToggle.isOn)
            return true;
        else if (kidToggle.isOn && (stageG == "Kid" || stageG == "Goat"))
            return true;
        else if (buckToggle.isOn && (stageG == "Buck" || stageG == "Buckling"))
            return true;
        else if (doelingToggle.isOn && (stageG == "Doeling" || stageG == "Doe"))
            return true;
        else
            return false;
    }

    public void OnSearchBarValueChanged(string searchText)
    {
        UpdateGoatDisplay();
    }
    private bool SearchMatches(CustomData customData, string searchText)
    {
        return (customData?.name?.ToLower().Contains(searchText) ?? false) ||
            (customData?.age.ToString()?.Contains(searchText) ?? false);
    }

    public void ClearSearchInputField()
    {
        searchInputField.text = ""; // Clear the text of the input field
    }

}

[System.Serializable]
public class ContainerDataList
{
    public ContainerData[] dataList;
}

[System.Serializable]
public class ContainerData
{
    public string name;
    public int age;
    public string gender;
    public string stageG;
}
[System.Serializable]
public class CustomData
{
    public int age;
    public string name;
    public String stageG;
    public Button button;
    public Button innerButton;
    public RawImage rawImage;
    
}