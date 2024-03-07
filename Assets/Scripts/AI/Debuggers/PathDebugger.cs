using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PathDebugType
{ 
    PathCornersLength,
        NextPosition
};

[RequireComponent(typeof(LineRenderer))]
public class PathDebugger : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agentToDebug;

    private LineRenderer linerenderer;

    public PathDebugType debugType;

    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agentToDebug != null)
        {
            if (debugType == PathDebugType.PathCornersLength)
            {
                if (agentToDebug.hasPath)
                {
                    linerenderer.positionCount = agentToDebug.path.corners.Length;
                    linerenderer.SetPositions(agentToDebug.path.corners);
                    linerenderer.enabled = true;
                }
                else
                {
                    linerenderer.enabled = false;
                }
            }
            else if (debugType == PathDebugType.NextPosition)
            {
                if (agentToDebug.hasPath)
                {
                    Vector3[] debugpositions = {new Vector3(), new Vector3()};
                    debugpositions[0] = agentToDebug.transform.position;
                    debugpositions[1] = agentToDebug.nextPosition;
                    linerenderer.SetPositions(debugpositions);
                }
                else
                {
                    linerenderer.enabled = false;
                }
            }
           
        }
    }
}

