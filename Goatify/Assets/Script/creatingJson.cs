using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class creatingJson : MonoBehaviour
{
    // Start is called before the first frame update
  void Start()
    {

        string filename = Path.Combine(Application.persistentDataPath, "GoatInfo.json");
        string filenamear = Path.Combine(Application.persistentDataPath, "Archivedata.json");

        if (!File.Exists(filename))
        {
            // Create an empty JSON file
            File.WriteAllText(filename, "{}");
            File.WriteAllText(filenamear, "{}");

            Debug.Log("Created goatinfo.json");
        }
        else
        {
            Debug.Log("goatinfo.json already exists");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
