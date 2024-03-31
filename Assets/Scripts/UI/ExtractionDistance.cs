using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
