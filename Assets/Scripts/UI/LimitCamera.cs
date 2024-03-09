using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        // We don't want the camera to rotate with the player
        transform.position = new Vector3(player.transform.position.x, 40, player.transform.position.z); 
    }
}
