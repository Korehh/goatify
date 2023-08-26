using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;


public class displayingdata : MonoBehaviour
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
    public Text stageText;
    public Text tagmotherText;
    public Text tagfatherText;
     public Text StatusGText;

    public Text femaleIdentify;         // Reference to your Text UI element
    public GameObject textStatus;     // Reference to the first object to enable/disable
    public GameObject buttonStatus;     // Reference to the second object to enable/disable

    public RawImage rawImage;
    public string pathA = "Assets/Images/StageImg/malegoat.png"; // Path for condition A
    public string pathB = "Assets/Images/StageImg/femalegoat.png"; // Path for condition B
    public string pathC = "Assets/Images/StageImg/kidgoat.png"; // Path for condition C
    private MyDataArchived MyDataArchived;

    void Start()
    {
        //string jsonFilePath = "Assets/GoatInfo.json";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GoatInfo.json");

        // Read the JSON file
        string jsonString = File.ReadAllText(jsonFilePath);

        // Parse the JSON data
        MyDataArchived = JsonUtility.FromJson<MyDataArchived>(jsonString);


        if (MyDataArchived != null && MyDataArchived.dataList != null)
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
            stageText.text = "";
            tagmotherText.text = "";
            tagfatherText.text = "";
            StatusGText.text = "";
        }
       //LoadAndDisplayConditionalImage();
    }
    private void Update()
    {
        if (femaleIdentify.text == "Female")
        {
            // Enable the two objects
            textStatus.SetActive(true);
            buttonStatus.SetActive(true);
        }
        else
        {
            // Disable the two objects
            textStatus.SetActive(false);
            buttonStatus.SetActive(false);
        }
    }

    private void DisplayDataByAge(int targetAge)
    {

        if (MyDataArchived != null && MyDataArchived.dataList != null)
        {
            // Find the data item with the target age
            DataArchived targetItem = MyDataArchived.dataList.Find(item => item.age == targetAge);

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
                stageText.text =targetItem.stageG;
                tagmotherText.text =targetItem.tagmother;
                tagfatherText.text = targetItem.tagfather;
                StatusGText.text = targetItem.statusG;
                LoadAndDisplayConditionalImage(targetItem);
                
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
                stageText.text = "";
                tagmotherText.text = "";
                tagfatherText.text = "";
                StatusGText.text = "";
                LoadAndDisplayConditionalImage(null);
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
                stageText.text = "";
                tagmotherText.text = "";
                tagfatherText.text = "";
                StatusGText.text = "";
                LoadAndDisplayConditionalImage(null);
        }
    }
public void EditScene(){
    PlayerPrefs.SetInt("AgePass", int.Parse(ageText.text));
    SceneManager.LoadScene("UpdateGoatF");
    }

void LoadAndDisplayConditionalImage(DataArchived targetItem)
    {
        string selectedPath = "";

        if (targetItem != null)
        {
            // Determine which path to use based on some conditions
            if (targetItem.stageG == "Wether" || targetItem.stageG == "Buckling" || targetItem.stageG == "Buck")
            {
                selectedPath = pathA;
            }
            else if (targetItem.stageG == "Doeling" || targetItem.stageG == "Doe")
            {
                selectedPath = pathB;
            }
            else if (targetItem.stageG == "Kid")
            {
                selectedPath = pathC;
            }
            else
            {
                Debug.LogError("No valid condition met.");
                return;
            }

            // Load the image from the selected path
            byte[] imageBytes = System.IO.File.ReadAllBytes(selectedPath);
            Texture2D rawTexture = new Texture2D(2, 2);
            rawTexture.LoadImage(imageBytes);

            // Display the loaded texture in your RawImage component
            rawImage.texture = rawTexture;
        }
        else
        {
            // Handle case where targetItem is null
            Debug.LogError("targetItem is null.");
        }
    }   
}

[System.Serializable]
public class MyDataArchived
{
    public List<DataArchived> dataList;
}

[System.Serializable]
public class DataArchived
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
    public string stageG;
    public string tagfather;
    public string tagmother;
    public string statusG;
    // Add other fields to match your JSON structure
}