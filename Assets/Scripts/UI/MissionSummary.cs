using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Mission Summary UI and Camera transition procedures.
    Temporary file for proof of concept until SO's are setup.
 */
public class MissionSummary : MonoBehaviour
{
    // Temporary referencing structure until SO's are setup.
    public GameObject player;
    public GameObject mainCamera;
    public GameObject missionSummaryCamera;
    public GameObject hudCanvas;
    public GameObject missionSummaryCanvas;
    public GameObject selectableObject;

    // Perform mission complete procedure
    public void missionComplete()
    {
        if (player != null && mainCamera != null && missionSummaryCamera != null )
        {
            player.GetComponent<PlayerControl>().isPlayerControlEnabled = false;
            missionSummaryCamera.SetActive(true);
            mainCamera.SetActive(false);
            hudCanvas.SetActive(false);
            missionSummaryCanvas.SetActive(true);
            selectableObject.GetComponent<Button>().Select();
        }
    }
}

