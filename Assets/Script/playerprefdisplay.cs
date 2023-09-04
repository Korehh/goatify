using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerprefdisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public Text nameText;
    public Text ageText;
    void Start()
    {
     nameText.text = PlayerPrefs.GetString("NamePass");
     ageText.text = PlayerPrefs.GetInt("AgePass").ToString();
    }

     public void clickSave()
     {
      PlayerPrefs.SetInt("AgePass", int.Parse(ageText.text));
      SceneManager.LoadScene("UpdateGoat");

        
     }
}
