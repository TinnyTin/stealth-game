using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AIStateMachine))]
[RequireComponent(typeof(ThreatMeter))]
public class FieldOfView : MonoBehaviour
{
    [Header("FOV settings")]
    public GameObject viewCastStartingPoint;
    public float refreshInterval = 0.2f;
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    [Header("Visual Field of View")]
    public float meshResolution = 1f;
    [Tooltip("Based on how close viewRadius is to Player, fade in the FOV as a mesh alpha")]
    public float meshFadeInMultiplier = 1f;
    public MeshFilter viewMeshFilter;
    public MeshRenderer viewMeshRenderer;
    [Header("Layer Masks")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    private List<Transform> lastVisibleTargets = new List<Transform>();
    private int countInView = 0;
    
    // private
    private Mesh viewMesh;
    private ThreatMeter threatmeter;
    private float meshAlpha = 0.0f;
    private AIStateMachine ai;

    // TODO remove this secion and add in fixedupdate...?
    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        threatmeter = GetComponent<ThreatMeter>();
        meshAlpha = 0.0f;
        ai = GetComponent<AIStateMachine>();

        StartCoroutine("FindTargetsWithDelay", refreshInterval);
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void Update()
    {
        DrawFieldOfView();
    }
    
    //  Find targets
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        // get objects within view RADIUS
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            // check within viewing ANGLE
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - viewCastStartingPoint.transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(viewCastStartingPoint.transform.position, target.position);

                // RAYCAST against blocking obstacles
                if (!Physics.Raycast(viewCastStartingPoint.transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
        countInView = 0;
        foreach(Transform t in lastVisibleTargets)
        {
            if (visibleTargets.Contains(t))
            {
                threatmeter.onSightThreat(t, refreshInterval);
                countInView++;
            }
        }
        ai.countInView = countInView;
        lastVisibleTargets.Clear();
        lastVisibleTargets.AddRange(visibleTargets);
    }

    bool checkTargetVisible(Transform t)
    {
        return visibleTargets.Contains(t) ? true : false;
    }

    // convert Angle into vector3 direction
    // angleIsGlobal indicates whether we use global or local coordinates
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees*Mathf.Deg2Rad));
    }

    void DrawFieldOfView()
    {
        if (countInView > 0) 
        {
            meshAlpha = Mathf.Lerp(meshAlpha, 0.3f, Time.deltaTime);
        }
        else
        {
            meshAlpha = Mathf.Lerp(meshAlpha, 0.0f, Time.deltaTime);
        }
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1; // origin vertex + viewCast points
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3]; // triangles formed = total vertices - 2

        vertices[0] = Vector3.zero; // origin of local space
        // triangle vertex indices
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]); // save in vertices
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
        viewMeshRenderer.material.color = new Color(viewMeshRenderer.material.color.r,viewMeshRenderer.material.color.g,viewMeshRenderer.material.color.b, meshAlpha);
        
    }

    // check cast using viewRadius and angle hits any obstacle. return info
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true,hit.point,hit.distance,globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }
    
    // 
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }
}
