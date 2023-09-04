using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerprefname : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField ageInputField;
    void Start()
    {
       ageInputField.text = PlayerPrefs.GetInt("AgePass").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
