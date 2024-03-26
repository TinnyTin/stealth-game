using UnityEngine;

public class triggerDisableCollider : MonoBehaviour
{
    public GameObject GO;

    bool isDisabled = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isDisabled)
        {
            GO.SetActive(false);
            isDisabled = true;
        }
        
        
    }
}
