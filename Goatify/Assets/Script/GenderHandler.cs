using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenderHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetMale;
    public GameObject targetFemale;
    public Text textgender;
    void Start()
    {
        if (textgender.text == "Male")
            {
            targetFemale.SetActive(false);
            targetMale.SetActive(true);
            }
            else
            {
                
            targetMale.SetActive(false);
            targetFemale.SetActive(true);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
