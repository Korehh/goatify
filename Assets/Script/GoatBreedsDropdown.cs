using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GoatBreedsDropdown : MonoBehaviour
{
    public GameObject buttonsContainerPrefab;
    public Transform containerParent;
    public InputField breedInputField;
    public InputField breedPassInputField;
    public InputField AddBreedInputField;
    public GameObject panelToDeactivate;
    public GameObject panelToActivate;
    private string breedDataFilePath;
    private List<string> breedList;
    private List<BreedButtonData> breedButtonList = new List<BreedButtonData>();

    private List<string> filteredBreedList;  // Add this variable

    private void Awake()
    {
        breedDataFilePath = Path.Combine(Application.dataPath, "JsonFiles/GoatBreeds.json");
    }

    private void Start()
    {
        LoadBreeds();

         // Attach the search input listener
        breedInputField.onValueChanged.AddListener(UpdateFilteredBreedList);
    }

    private void LoadBreeds()
    {
         breedList = new List<string>();
        filteredBreedList = new List<string>();  // Initialize the filtered list

        Debug.Log("Checking if file exists at: " + breedDataFilePath);
    if (File.Exists(breedDataFilePath))
    {
        Debug.Log("File exists.");
        string json = File.ReadAllText(breedDataFilePath);
        Debug.Log("Loaded JSON: " + json);
         breedList = JsonUtility.FromJson<BreedListWrapper>(json).breedList;
        // ... rest of the code
    }
    else
    {
        Debug.LogError("File not found at: " + breedDataFilePath);
    }

        // Populate the filtered breed list
        filteredBreedList.AddRange(breedList);
        UpdateBreedButtons();
    }

 private void UpdateBreedButtons()
{
    Debug.Log("Updating breed buttons...");
      // Clear existing cloned breed buttons
    foreach (Transform child in containerParent)
    {
        if (child != buttonsContainerPrefab.transform) // Skip the original button container
        {
            Destroy(child.gameObject);
        }
    }
    // Instantiate and set up new breed buttons based on breedList
    foreach (string breed in filteredBreedList)
    {
        GameObject buttonsContainer = Instantiate(buttonsContainerPrefab, containerParent);

        // Set the properties based on the JSON data
        Text breedButtonText = buttonsContainer.transform.Find("Breed Text").GetComponent<Text>();
        Button breedButton = buttonsContainer.GetComponentInChildren<Button>();

        if (breedButtonText != null)
        {
            breedButtonText.text = breed;
        }
        else
        {
            Debug.LogWarning("Breed Text component not found in " + buttonsContainerPrefab.name);
        }
        
        if (breedButton != null)
        {
            // Add an onClick listener to the button
            breedButton.onClick.AddListener(() => HandleBreedButtonClick(breed));
             breedButton.gameObject.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("Button component not found in " + buttonsContainerPrefab.name);
        }
    }

    // Hide the original button before the new buttons
    Button originalButton = buttonsContainerPrefab.GetComponentInChildren<Button>();
    originalButton.gameObject.SetActive(false);

    // Instantiate the original button after the new buttons
    GameObject originalButtonsContainer = Instantiate(buttonsContainerPrefab, containerParent);
    Button clonedOriginalButton = originalButtonsContainer.GetComponentInChildren<Button>();
    clonedOriginalButton.gameObject.SetActive(true);
    clonedOriginalButton.onClick.AddListener(() => HandleAddBreedButtonClick());

    // Adjust pivot and anchor settings of the container parent
    RectTransform containerRect = containerParent.GetComponent<RectTransform>();
    containerRect.pivot = new Vector2(0.5f, 1f);
    containerRect.anchorMin = new Vector2(0.5f, 1f);

    Debug.Log("Number of breed buttons: " + breedList.Count);
}

    private void HandleBreedButtonClick(string breed)
    {
        breedPassInputField.text = breed;
        DeactivatePanel();
        Debug.Log("Breed button clicked: " + breed);

        // Perform actions based on the clicked breed
    }
    private void HandleAddBreedButtonClick()
    {
        DeactivatePanel();
        ActivatePanel();
    }

    public void AddBreed()
    {
        string newBreed = AddBreedInputField.text;
        breedList.Add(newBreed);
        SaveBreeds();
        AddBreedInputField.text = "";
        if (!string.IsNullOrEmpty(breedInputField.text))
    {
        UpdateFilteredBreedList(breedInputField.text);
    }
    else
    {
        filteredBreedList.AddRange(breedList);
    }
        UpdateBreedButtons();

    }

    private void UpdateFilteredBreedList(string searchString)
    {
        // Clear the filtered breed list
        filteredBreedList.Clear();

        // Populate the filtered breed list based on search input
        foreach (string breed in breedList)
        {
            if (breed.ToLower().Contains(searchString.ToLower()))
            {
                filteredBreedList.Add(breed);
            }
        }

        // Update the displayed buttons based on the filtered breed list
        UpdateBreedButtons();
    }
    public void DeactivatePanel()
    {
        panelToDeactivate.SetActive(false);
    }
    public void ActivatePanel()
    {
        panelToActivate.SetActive(true);
    }
    private void SaveBreeds()
    {
        string json = JsonUtility.ToJson(new BreedListWrapper(breedList));
        File.WriteAllText(breedDataFilePath, json);
    }
    [System.Serializable]
    private class BreedListWrapper
    {
        public List<string> breedList;

        public BreedListWrapper(List<string> breedList)
        {
            this.breedList = breedList;
        }
    }

    [System.Serializable]
    public class BreedButtonData
    {
        public string breed;
        public Button button;
    }
}
