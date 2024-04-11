using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamouflage : MonoBehaviour
{
    public PlayerData playerData;

    [Range(0f, 1f)]
    public float CamoAmount = 1f;

    private void Start()
    {
        CamoAmount = Mathf.Clamp(CamoAmount, 0f, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        playerData.SetPlayerCamo(CamoAmount);
    }

    private void OnTriggerExit(Collider other)
    {
        playerData.ResetPlayerCamo();
    }
}
