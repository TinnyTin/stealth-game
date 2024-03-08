using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "SO's/AIGlobalData")]
public class AIData : ScriptableObjectWithInit
{
    public AIThreatPriority highestState;
    //public float highestThreat;

    // Events
    public GameEvent eventToRaise;

    private List<GameObject> AIs;

    private GameObject AIhighestState;
    private GameObject AIHighestThreatPriority;

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
            AIs.OrderByDescending(AIs => AIs.GetComponent<AIStateMachine>().aiThreatPriority).ToArray();
            AIHighestThreatPriority = AIs[0];
            highestState = AIHighestThreatPriority.GetComponent<AIStateMachine>().aiThreatPriority;

            // raise Event with 
            eventToRaise.Raise(highestState);
        }

    }

    public void UnRegisterAI(GameObject ai)
    {
        if (AIs.Contains(ai))
        {
            AIs.Remove(ai);
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
        AIs.Add(ai);
    }
}
