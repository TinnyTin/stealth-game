using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSummary : MonoBehaviour
{
    // Temporary referencing structure until SO's are setup.
    public GameObject player;
    public GameObject mainCamera;
    public GameObject missionSummaryCamera;
    public GameObject hudCanvas;
    public GameObject missionSummaryCanvas;

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
        }
    }
}
