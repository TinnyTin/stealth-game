using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Martin
 * Contributors:        
 * 
 * Calculates and displays the distance to extraction zone.
 */

public class ExtractionDistance : MonoBehaviour
{
    public GameObject extractionPoint;
    public TMPro.TextMeshProUGUI distanceText;
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (extractionPoint != null)
        {
            distanceText.text = Vector3.Distance(playerData.PlayerPosition, extractionPoint.transform.position).ToString("F1") +'m' ;
        }
    }
}
