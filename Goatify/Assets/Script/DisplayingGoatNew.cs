using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class DisplayingGoatNew : MonoBehaviour
{
    public GameObject buttonsContainerPrefab;
    public Transform containerParent;
    private float steadyHeight = 1570f;
    private float extraItemHeight = 260f; // Set to a positive value here
    private Vector3 initialPosition;

    private List<CustomData> customDataList = new List<CustomData>(); // Declare customDataList at the class level

    private void Start()
    {
        
        // Store the initial position of the Container
        initialPosition = new Vector3(containerParent.position.x, 0f, containerParent.position.z);

        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GoatInfo.json");

        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            ContainerDataList dataList = JsonUtility.FromJson<ContainerDataList>(jsonString);

            int totalItems = dataList.dataList.Length;
            float totalHeight = steadyHeight;

            if (totalItems > 7)
            {
                totalHeight += (totalItems - 7) * extraItemHeight;
                Debug.Log("Total Items: " + totalItems);
                Debug.Log("Total Height: " + totalHeight);
            }
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

                // Store the age and button reference in the custom data structure
                CustomData customData = new CustomData();
                customData.age = data.age;
                customData.button = ageButton;
                customDataList.Add(customData);

                // Add onClick event to the button to handle the click event
                ageButton.onClick.AddListener(() => OnAgeButtonClick(ageText.text));

            }

            // Set the height of the Container to match the total height required
            RectTransform containerRect = containerParent.GetComponent<RectTransform>();
            Vector2 containerSize = containerRect.sizeDelta;
            containerSize.y = totalHeight;
            containerRect.sizeDelta = containerSize;

            // Reset the position of the Container to the initial static position
            containerParent.position = initialPosition;
        }
        else
        {
            Debug.LogError("GoatInfo.json not found in the persistent data path.");
        }
    }
    public void OnAgeButtonClick(string ageText)
    {
        // Extract the age value from the ageText string
        int age;
        if (int.TryParse(ageText.Replace("Age: ", ""), out age))
        {
            PlayerPrefs.SetInt("AgePass", age);
            SceneManager.LoadScene("DisplayGoat");

            Debug.Log("Age saved to PlayerPrefs: " + age);
        }
        else
        {
            Debug.LogError("Failed to parse age from the button text: " + ageText);
        }
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
}
[System.Serializable]
public class CustomData
{
    public int age;
    public Button button;
}