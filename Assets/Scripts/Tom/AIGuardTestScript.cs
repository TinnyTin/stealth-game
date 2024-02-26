using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIGuardTestScript : MonoBehaviour
{
    [Header("Events")]
    public GameEvent onPlayerSpotted;
    public GameEvent onTest2ParamEvent; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            onPlayerSpotted.Raise(this, "raised by AI Guard Test");
        }

        if (Input.GetKeyDown("x"))
        {
            onTest2ParamEvent.Raise(this, "param1", "param2");
        }
    }
}
