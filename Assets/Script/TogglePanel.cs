using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
     public void TogglePanelVisibility(GameObject panelToToggle)
    {
        panelToToggle.SetActive(!panelToToggle.activeSelf);
    }
}
