using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamouflage : MonoBehaviour
{
    public PlayerData playerData;

    [Range(0f, 1f)]
    public float CamoAmount = 1f;
    public bool requireCrouch = false;

    private void Start()
    {
        CamoAmount = Mathf.Clamp(CamoAmount, 0f, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        UpdateCamoLevel();
    }

    private void OnTriggerStay(Collider other)
    {
        UpdateCamoLevel();
    }

    private void OnTriggerExit(Collider other)
    {
        playerData.ResetPlayerCamo();
    }

    private void UpdateCamoLevel()
    {
        if (!requireCrouch)
        {
            playerData.SetPlayerCamo(CamoAmount);
        }
        else
        {
            if (playerData.IsCrouched)
            {
                playerData.SetPlayerCamo(CamoAmount);
            }
            else
            {
                playerData.ResetPlayerCamo();
            }
        }
    }
}
