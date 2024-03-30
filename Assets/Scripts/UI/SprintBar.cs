using UnityEngine;
using UnityEngine.UI;


/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Sprint Stamina Bar
 */


public class SprintBar : MonoBehaviour
{
    public PlayerData PlayerData;

    private Slider slider;
    private Animator anim;
    private float currTime;
    private float triggerTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        anim = GetComponent<Animator>();
        currTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Change the slider value when the bar isn't full;
        if (PlayerData.PlayerSprintStamina < 1f)
        {
            slider.value = PlayerData.PlayerSprintStamina;
            currTime = 0f;
        }

        if (currTime < triggerTime && PlayerData.PlayerSprintStamina >= 0.99f)
        {
            currTime += Time.deltaTime;
            
        }

        if (currTime >= triggerTime)
        {
            anim.SetBool("SprintBarFull", true);
        }
    }

    public void onValueChanged(float value)
    {
        // If the bar isn't full, reveal the bar to player.
        if (value < 0.99f)
        {
            anim.SetBool("SprintBarFull", false);
        }
    }
}
