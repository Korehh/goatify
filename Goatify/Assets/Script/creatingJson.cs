using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class creatingJson : MonoBehaviour
{
  void Start()
    {

            string filename = Path.Combine(Application.persistentDataPath, "GoatInfo.json");
            string filenamear = Path.Combine(Application.persistentDataPath, "Archivedata.json");
            string filenameWeight = Path.Combine(Application.persistentDataPath, "weightdata.json");

            if (!File.Exists(filename) || !File.Exists(filenamear) || !File.Exists(filenameWeight))
        {
            // Create empty JSON files for each file that doesn't exist
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, "{}");
                Debug.Log("Created " + filename);
            }

            if (!File.Exists(filenamear))
            {
                File.WriteAllText(filenamear, "{}");
                Debug.Log("Created " + filenamear);
            }

            if (!File.Exists(filenameWeight))
            {
                File.WriteAllText(filenameWeight, "{}");
                Debug.Log("Created " + filenameWeight);
            }
        }
        else
        {
            Debug.Log("All files already exist");
        }
        
    }
    void Update()
    {
        
    }
}
