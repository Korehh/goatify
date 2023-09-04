using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoatFormValidation : MonoBehaviour
{
    public InputField breedInput;
    public InputField nameInput;
    public InputField dateOfBirthInput;
    public InputField dateOfEntryInput;
    public InputField weightInput;
    public Dropdown genderDropdown;
    public Dropdown obtainDropdown;

    public GameObject breedErrorObject;
    public GameObject nameErrorObject;
    public GameObject dateOfBirthErrorObject;
    public GameObject dateOfEntryErrorObject;
    public GameObject weightErrorObject;
    public GameObject genderErrorObject;
    public GameObject obtainErrorObject;

    public GameObject confirmPanel;

    public Button validateButton;
    //private bool validationButtonClicked = false;
    private void Start()
    { 
        HideAllErrorObjects();
        HideConfirmPanel();

        breedInput.onValueChanged.AddListener((newValue) => ValidateFieldAndHideError(breedInput, breedErrorObject));
        nameInput.onValueChanged.AddListener((newValue) => ValidateFieldAndHideError(nameInput, nameErrorObject));
        dateOfBirthInput.onValueChanged.AddListener((newValue) => ValidateFieldAndHideError(dateOfBirthInput, dateOfBirthErrorObject));
        dateOfEntryInput.onValueChanged.AddListener((newValue) => ValidateFieldAndHideError(dateOfEntryInput, dateOfEntryErrorObject));
        weightInput.onValueChanged.AddListener((newValue) => ValidateFieldAndHideError(weightInput, weightErrorObject));

            // Set up listeners for dropdowns (remove the initialization value)
        genderDropdown.onValueChanged.AddListener((newValue) => ValidateDropdownAndHideError(genderDropdown, genderErrorObject));
        obtainDropdown.onValueChanged.AddListener((newValue) => ValidateDropdownAndHideError(obtainDropdown, obtainErrorObject));


        if (validateButton != null)
        {
            validateButton.onClick.AddListener(ValidateAndShowConfirm);
        }
    }
   private void ValidateFieldAndHideError(InputField inputField, GameObject errorObject)
    {
      
        
            ValidateSingleField(inputField, errorObject);
        
    }
    private void ValidateSingleField(InputField inputField, GameObject errorObject)
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            errorObject.SetActive(true);
        }
        else
        {
            errorObject.SetActive(false);
        }
    }

        private void ValidateDropdownAndHideError(Dropdown dropdown, GameObject errorObject)
    {

        {
            ValidateSingleDropdown(dropdown, errorObject);
        }
    }

    private void ValidateSingleDropdown(Dropdown dropdown, GameObject errorObject)
 {
    if (dropdown.value != 0)
    {
        errorObject.SetActive(false);
    }
}
    private void ValidateAndShowConfirm()
    {
        ValidateForm("");
        UpdateConfirmPanelState();
    }

    private void HideAllErrorObjects()
    {
        breedErrorObject.SetActive(false);
        nameErrorObject.SetActive(false);
        dateOfBirthErrorObject.SetActive(false);
        dateOfEntryErrorObject.SetActive(false);
        weightErrorObject.SetActive(false);
        genderErrorObject.SetActive(false);
        obtainErrorObject.SetActive(false);
    }

    private void HideConfirmPanel()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }

    private void UpdateConfirmPanelState()
    {
        bool allFieldsValid = ValidateAllFields();

        if (confirmPanel != null)
        {
            confirmPanel.SetActive(allFieldsValid);
        }
    }

    private bool ValidateAllFields()
    {
        bool isBreedValid = !string.IsNullOrEmpty(breedInput.text);
        bool isNameValid = !string.IsNullOrEmpty(nameInput.text);
        bool isDateOfBirthValid = !string.IsNullOrEmpty(dateOfBirthInput.text);
        bool isDateOfEntryValid = !string.IsNullOrEmpty(dateOfEntryInput.text);
        bool isWeightValid = !string.IsNullOrEmpty(weightInput.text) && float.TryParse(weightInput.text, out _);
        bool isGenderValid = genderDropdown.value != 0;
        bool isObtainValid = obtainDropdown.value != 0;

        return isBreedValid && isNameValid && isDateOfBirthValid && isDateOfEntryValid && isWeightValid && isGenderValid && isObtainValid;
    }

     private void ValidateForm(string newValue)
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(breedInput.text))
        {
            isValid = false;
            breedErrorObject.SetActive(true);
        }
        else
        {
            breedErrorObject.SetActive(false);
        }

        if (string.IsNullOrEmpty(nameInput.text))
        {
            isValid = false;
            nameErrorObject.SetActive(true);
        }
        else
        {
            nameErrorObject.SetActive(false);
        }

        if (string.IsNullOrEmpty(dateOfBirthInput.text))
        {
            isValid = false;
            dateOfBirthErrorObject.SetActive(true);
        }
        else
        {
            dateOfBirthErrorObject.SetActive(false);
        }

        if (string.IsNullOrEmpty(dateOfEntryInput.text))
        {
            isValid = false;
            dateOfEntryErrorObject.SetActive(true);
        }
        else
        {
            dateOfEntryErrorObject.SetActive(false);
        }

        if (string.IsNullOrEmpty(weightInput.text) || !float.TryParse(weightInput.text, out _))
        {
            isValid = false;
            weightErrorObject.SetActive(true);
        }
        else
        {
            weightErrorObject.SetActive(false);
        }

        if (genderDropdown.value == 0)
        {
            isValid = false;
            genderErrorObject.SetActive(true);
        }
        else
        {
            genderErrorObject.SetActive(false);
        }

        if (obtainDropdown.value == 0)
        {
            isValid = false;
            obtainErrorObject.SetActive(true);
        }
        else
        {
            obtainErrorObject.SetActive(false);
        }

        if (isValid)
        {
            Debug.Log("Form is valid. Proceed with saving.");
            // You can proceed with saving the data here
        }
        else
        {
            Debug.Log("Form has validation errors.");
            // You can show an error message or perform other actions here
        }
    }
}
