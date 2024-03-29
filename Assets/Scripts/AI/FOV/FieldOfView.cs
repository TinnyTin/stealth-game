using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        Tom
 * Description: AI "eyes" includes view Radius, view Angle, view Raycasting.
 *              Also includes red cone FOV using a mesh renderer and viewCasting to objects in scene
 * External
 * Source Credit:       
 *                      
 */

public class FieldOfView : MonoBehaviour
{
    [Header("FOV settings")]
    public GameObject viewCastStartingPoint;
    public float refreshInterval = 0.2f;
    public float viewRadius;
    public float fadeInMeshRadiusMultiplier = 1.1f;
    [Range(0, 360)]
    public float viewAngle;
    public float fadeInMeshviewAngleMultiplier = 1.1f;

    public bool updateTransform = true;
    [Header("Visual Field of View")]
    public bool forceVisible = false;
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

    [Header("Other components")]
    public AIStateMachine ai;
    public ThreatMeter threatmeter;
    public GameObject anchor;


    // private
    private Mesh viewMesh;
    private float meshAlpha = 0.0f;

    [SerializeField] private bool _fovIsGrowing = false; 
    [SerializeField] private bool _fovIsShrinking = false;


    // TODO remove this secion and add in fixedupdate...?
    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        meshAlpha = 0.0f;

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
        if (updateTransform)
        {
            transform.position = anchor.transform.position;
            transform.rotation = anchor.transform.rotation;
        }
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
        foreach (Transform t in lastVisibleTargets)
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
    public Vector3? FindTargetWithinRadius(float radius)
    {
        Vector3? retpos = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (targetsInViewRadius.Length > 0)
        {
            retpos = targetsInViewRadius[0].transform.position;
        }
        return retpos;
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
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void DrawFieldOfView()
    {
        bool closeToView = forceVisible;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius * fadeInMeshRadiusMultiplier, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            // check within viewing ANGLE
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - viewCastStartingPoint.transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < (viewAngle * fadeInMeshviewAngleMultiplier) / 2)
            {
                closeToView = true;
                break;
            }
        }

        if (closeToView)
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
        viewMeshRenderer.material.color = new Color(viewMeshRenderer.material.color.r, viewMeshRenderer.material.color.g, viewMeshRenderer.material.color.b, meshAlpha);

    }

    // check cast using viewRadius and angle hits any obstacle. return info
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
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

    public void SetImmediateFOVAngle(float angle)
    {
        _fovIsShrinking = false;
        _fovIsGrowing = false;
        viewAngle = angle; 
    }

    public void GrowFOVAngle(float maxAngle, float growTimeSeconds)
    {
        _fovIsShrinking = false; 
        _fovIsGrowing = true;
        StartCoroutine(CoroutineGrowFOVAngle(maxAngle, growTimeSeconds));
    }

    private IEnumerator CoroutineGrowFOVAngle(float maxAngle, float growTimeSeconds)
    {
        float startAngle = viewAngle;
        float timeStep = 0.1f; 
        float growAnglePerTimeStep = (maxAngle-startAngle) / (growTimeSeconds / timeStep); 

        while (viewAngle < maxAngle)
        {
            if (_fovIsGrowing == false)
                break;

            viewAngle += growAnglePerTimeStep;
            if (viewAngle > maxAngle)
                viewAngle = maxAngle;
            yield return new WaitForSeconds(timeStep); 
        }

        _fovIsGrowing = false; 
    }

    public void ShrinkFOVAngle(float minAngle, float shrinkTimeSeconds)
    {
        _fovIsShrinking = true;
        _fovIsGrowing = false;
        StartCoroutine(CoroutineShrinkFOVAngle(minAngle, shrinkTimeSeconds)); 
    }

    public IEnumerator CoroutineShrinkFOVAngle(float minAngle, float shrinkTimeSeconds)
    {
        float startAngle = viewAngle;
        float timeStep = 0.1f;
        float shrinkAnglePerTimeStep = (startAngle - minAngle) / (shrinkTimeSeconds / timeStep);

        while (viewAngle > minAngle)
        {
            if (_fovIsShrinking == false)
                break; 

            viewAngle -= shrinkAnglePerTimeStep;
            if (viewAngle < minAngle)
                viewAngle = minAngle;
            yield return new WaitForSeconds(timeStep);
        }

        _fovIsShrinking = false;
    }
}
