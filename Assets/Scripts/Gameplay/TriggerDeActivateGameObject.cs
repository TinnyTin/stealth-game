
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeActivateGameObject: MonoBehaviour
{
    public GameObject[] GOs;

    bool isDeactivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isDeactivated)
        {
            foreach (GameObject GO in GOs)
            {
                GO.SetActive(false);
            }
            isDeactivated = true;
        }


    }
}
