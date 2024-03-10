using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deactivateMenuContents()
    {
        foreach(Transform c in transform)
        {
            c.gameObject.SetActive(false);
        }
    }

    void activateMenuContents()
    {
        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(true);
        }
    }
}
