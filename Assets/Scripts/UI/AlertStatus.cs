using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class AlertStatus : MonoBehaviour
{
    private Image image;
    private TMPro.TextMeshProUGUI tmp;
    public GameObject textObject;
 
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (textObject != null)
        {
            tmp = textObject.GetComponent<TMPro.TextMeshProUGUI>();
        }
    }
    
    public void setAlertStatus(Component component, AIThreatPriority threatPriority)
    {
        switch(threatPriority)
        {
            case AIThreatPriority.Idle:
                setSafe();
                break;
            case AIThreatPriority.Investigate:
                setSearching();
                break;
            case AIThreatPriority.Pursuit:
                setPursuit();
                break;
            default:
                break;
        }
    }

    private void setSafe()
    {
        if (image != null && tmp != null) 
        {
            image.color = new Color(0, 0, 0, 0.5f);
            tmp.text = "Safe";
        }
    }
    private void setSearching()
    {
        if (image != null && tmp != null)
        {
            image.color = new Color(1f, 0.92f, 0.016f, 0.5f);
            tmp.text = "Searching";
        }
    }
    private void setPursuit()
    {
        if (image != null && tmp != null)
        {
            image.color = new Color(1f, 0, 0, 0.5f);
            tmp.text = "Pursuit";
            // In the future, we can add different music/ambience when stage is in alert mode.
        }
    }
}
