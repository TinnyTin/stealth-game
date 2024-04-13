using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private TextMeshProUGUI _pauseMenuRestartLevelUITextElem;

    [SerializeField]
    private TextMeshProUGUI _missionSummaryRestartLevelUITextElem;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.name != "Player Model")
            return; 

        if (_checkpointReached)
            return;
        
        ActivateCheckpoint(true, true);
    }

    private void ActivateCheckpoint(bool showNotification, bool updatePlayerPosition)
    {
        _checkpointReached = true;

        if (PlayerData != null && updatePlayerPosition)
        {
            PlayerData.LastCheckpoint = CheckpointNumber;
            PlayerData.LastCheckpointPos = PlayerData.PlayerPosition;
            PlayerData.LastCheckpointRot = PlayerData.PlayerRotation; 
        }

        foreach (GameObject trigger in Triggers)
        {
            // Check for a null trigger, this will happen when there
            // is an empty array element in the inspector view. 
            if (trigger == null)
                continue; 

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

        // Update UI element text to reflect that a checkpoint was reached
        _pauseMenuRestartLevelUITextElem.text = "Restart Level From Checkpoint";
        _missionSummaryRestartLevelUITextElem.text = "Restart Level From Checkpoint";
    }

private IEnumerator HideNotification(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotificationUIText.gameObject.SetActive(false);
    }

    public void ManuallyTrigger()
    {
        ActivateCheckpoint(false, false);
    }
}
