using System.Collections;
using System.Collections.Generic;
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
        tmp = textObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void setSafe()
    {
        image.color = new Color(0, 0, 0, 0.5f);
        tmp.text = "Safe";
    }
    public void setSearching()
    {
        image.color = new Color(1f, 0.92f, 0.016f, 0.5f);
        tmp.text = "Searching";
    }
    public void setPursuit()
    {
        image.color = new Color(1f,0,0, 0.5f);
        tmp.text = "Pursuit";
        // In the future, we can add different music/ambience when stage is in alert mode.
    }
}
