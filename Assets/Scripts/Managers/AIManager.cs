using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: AI Manager 
 *              receives request to enable/disable all AI
 *              pushes data into a ScriptableObjects.
 * External Source Credit: 
 *                      
 */

[CreateAssetMenu(menuName = "SO's/AIGlobalData")]
public class AIManager : ScriptableObjectWithInit
{
    public AIThreatPriority highestState;
    //public float highestThreat;

    // Events
    public GameEvent eventToRaise;

    private List<GameObject> AIs = new List<GameObject>();

    private GameObject AIhighestState;
    private GameObject AIWithHighestThreatPriority;

    // Init is called before the first frame update
    public override void Init()
    {
        highestState = AIThreatPriority.Idle;
    }

    public void UpdateThreatPriority()
    {
        if (AIs.Count > 0)
        {
            // ThreatPriority
            var sortedAIs = AIs.OrderByDescending(AIs => AIs.GetComponent<AIStateMachine>().aiThreatPriority).ToArray();
            AIWithHighestThreatPriority = sortedAIs[0];
            highestState = AIWithHighestThreatPriority.GetComponent<AIStateMachine>().aiThreatPriority;

            // raise Event with 
            eventToRaise.Raise(highestState);
        }

    }

    public void UnRegisterAI(GameObject ai)
    {
        if (AIs.Contains(ai))
        {
            AIs.Remove(ai);
            AIs.RemoveAll(ai => ai == null);
        }
        else
        {
            Debug.Log("Error: tried to remove ai obj that is not part of AI manager list");
        }
    }

    public void RegisterAI(GameObject ai)
    {
        if (AIs.Contains(ai))
        {
            Debug.Log("Error: tried to add AI twice to AI list!");
        }
        else
        {
            AIs.Add(ai);
        }

    }

    public void setAIActive(bool isActive)
    {
        foreach (GameObject AI in AIs)
        {
            AI.SetActive(isActive);
        }
    }
}
