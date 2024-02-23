using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookatCamera : MonoBehaviour
{
    public Camera tcam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(this.transform.position - (tcam.transform.position - this.transform.position));
    }
}
