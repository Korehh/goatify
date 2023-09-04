using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalPanelController : MonoBehaviour
{
    public void DeactivatePanel(GameObject panelToDeactivate)
    {
        panelToDeactivate.SetActive(false);
        Debug.Log("Panel Deactivated: " + panelToDeactivate.name);
    }
}
