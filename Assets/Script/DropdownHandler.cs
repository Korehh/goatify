using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DropdownHandler : MonoBehaviour
{
    public GameObject targetMale;
    public GameObject targetFemale;
    public Dropdown dropdown;

    private void Start()
    {
        // Add an event listener to the dropdown to call the OnDropdownValueChanged method when the value changes
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        

        // By default, the targetObject should be disabled. You can change this behavior by setting it in the Unity Inspector.
        //targetObject.SetActive(false);
    }

    private void OnDropdownValueChanged(int value)
    {

        //targetObject.SetActive(true);
        // value 0 corresponds to the first option, value 1 corresponds to the second option, and so on.
       if (value == 1)
        {
            targetFemale.SetActive(false);
            targetMale.SetActive(true);
            
        }
        else if (value == 2)
        {   
            targetMale.SetActive(false);
            targetFemale.SetActive(true);
        }
        else
        {   
            targetMale.SetActive(false);
            targetFemale.SetActive(false);
        }
    }

}
