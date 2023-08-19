using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    public InputField inputField;
    public GameObject targetObject;

    private void Start()
    {
        // Subscribe to the OnValueChanged event of the InputField
        inputField.onValueChanged.AddListener(OnInputValueChanged);

        // Disable the targetObject initially
        targetObject.SetActive(false);
    }

    private void OnInputValueChanged(string newValue)
    {
        // Check if the input value is not empty
        if (!string.IsNullOrEmpty(newValue))
        {
            // Enable the targetObject when the input value changes and is not empty
            targetObject.SetActive(true);
        }
        else
        {
            // Disable the targetObject when the input value is empty
            targetObject.SetActive(false);
        }
    }
}
