using TMPro;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Erik
 * Contributors:        Martin, Tom
 */

[RequireComponent(typeof(BoxCollider))]
public class ExtractionPoint : MonoBehaviour
{
    public AudioClip AudioClipExtract;
    public GameEvent eventToRaise;

    private GameObject childDefault;
    private GameObject childHilighted;

    public GameEvent PlayerReachedExit;

    public PlayerData playerData;

    [SerializeField]
    private TextMeshProUGUI _pauseMenuRestartLevelUITextElem;

    [SerializeField]
    private TextMeshProUGUI _missionSummaryRestartLevelUITextElem;

    private bool hastExtracted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (AudioClipExtract == null)
        {
            Debug.LogError("ExtractionPoint: No AudioClipExtract");
        }

        if (playerData == null)
        {
            Debug.LogError("ExtractionPoint: No PlayerData SO");
        }

        hastExtracted = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (playerData.PlayerHasStolenObject)
        {
            Debug.Log("ExtractionPoint: collide.");
            if (!hastExtracted)
            {
                Extract();
                hastExtracted = true;

                // Reset UI text elements to no longer mention checkpoints
                _pauseMenuRestartLevelUITextElem.text = "Restart Level";
                _missionSummaryRestartLevelUITextElem.text = "Restart Level";

                // Update the player to have no longer reached a checkpoint, 
                // this will cause the level to reload at the start instead
                // of the last reached checkpoint
                playerData.LastCheckpoint = -1; 
            }

        }
    }

    public void Extract()
    {
        if (AudioClipExtract != null)
            eventToRaise.Raise(AudioClipExtract, AudioSourceParams.Default);

        PlayerReachedExit.Raise();
    }
}
