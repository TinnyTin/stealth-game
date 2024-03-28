
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateGameObject : MonoBehaviour
{
    public GameObject[] GOs;

    bool isEnabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled)
        {
            foreach (GameObject GO in GOs)
            {
                GO.SetActive(true);
            }
            isEnabled = true;
        }


    }
}
