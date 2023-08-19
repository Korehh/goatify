using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class test : MonoBehaviour

{
    public RawImage rawImage;
    public string filename = "Assets/Images/StageImg/malegoat.png"; // Corrected path without the file extension

    void Start()
    {
        byte[] imagePath = System.IO.File.ReadAllBytes(filename); // Explicitly declare and initialize imagePath as byte[]

        Texture2D imageTexture = new Texture2D(2, 2); // You need to provide dimensions
        imageTexture.LoadImage(imagePath); // Load the image data

        if (imageTexture != null)
        {
            rawImage.texture = imageTexture;
        }
        else
        {
            Debug.LogError("Image not found at path: " + filename);
        }
    }

    void Update()
    {

    }
}