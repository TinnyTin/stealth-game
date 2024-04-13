using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 *
 * External
 * Source Credit:
 */

[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(Canvas))]
public class Checkpoint : MonoBehaviour
{
    public int CheckpointNumber;
    public PlayerData PlayerData;
    public List<GameObject> Triggers = new();

    public GameObject NotificationUIText;
    public float NotificationVisibleTime;

    private bool _checkpointReached = false;


    //private Canvas test; 

    // Start is called before the first frame update
    void Start()
    {
        // test = GetComponent<Canvas>(); 
        //test.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checkpoint trigger enter");
        Debug.Log($"{PlayerData.PlayerPosition}");

        if (_checkpointReached)
            return;
        
        ActivateCheckpoint(true);
    }

    private void ActivateCheckpoint(bool showNotification)
    {
        _checkpointReached = true;

        if (PlayerData != null)
        {
            PlayerData.LastCheckpoint = CheckpointNumber;
            PlayerData.LastCheckpointPos = PlayerData.PlayerPosition;
        }

        foreach (GameObject trigger in Triggers)
        {
            // Try to get a TriggerActiveGameObject and TriggerDeactiveateGameObject script
            TriggerActivateGameObject tago = trigger.GetComponent<TriggerActivateGameObject>();
            if (tago != null)
                tago.ManuallyTrigger();

            TriggerDeActivateGameObject tdgo = trigger.GetComponent<TriggerDeActivateGameObject>();
            if (tdgo != null)
                tdgo.ManuallyTrigger();
        }

        if (showNotification && NotificationUIText != null)
        {
            NotificationUIText.SetActive(true);
            StartCoroutine(HideNotification(NotificationVisibleTime));
        }
    }

    private IEnumerator HideNotification(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotificationUIText.gameObject.SetActive(false);
    }

    public void ManuallyTrigger()
    {
        ActivateCheckpoint(false);
    }
}
