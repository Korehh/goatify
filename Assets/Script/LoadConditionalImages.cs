using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadConditionalImages : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage rawImage;
    public string pathA = "Assets/Images/StageImg/malegoat.png"; // Path for condition A
    public string pathB = "Assets/Images/StageImg/femalegoat.png"; // Path for condition B
    public string pathC = "Assets/Images/StageImg/kidgoat.png"; // Path for condition C
    public Text stageText;
    void Start()
    {
        LoadAndDisplayConditionalImage();
    }

    void LoadAndDisplayConditionalImage()
    {
        string selectedPath = "";

        // Determine which path to use based on some conditions
        if (stageText.text == "Wether" || stageText.text == "Buckling" || stageText.text == "Buck")
        {
            selectedPath = pathA;
        }
        else if (stageText.text == "Doeling" || stageText.text == "Doe")
        {
            selectedPath = pathB;
        }
        else if (stageText.text == "Kid")
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
        Texture2D rawImage = new Texture2D(2, 2);
        rawImage.LoadImage(imageBytes);

    }
}
