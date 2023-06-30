using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearplayerpref : MonoBehaviour
{
   public void clearplayerprefs(){
      PlayerPrefs.DeleteAll();
   }
}
