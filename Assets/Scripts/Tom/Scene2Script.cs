using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Script : MonoBehaviour
{
    public PlayerData PlayerData; 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"The player health is {PlayerData.PlayerHealth}.");
        PlayerData.PlayerHealth += 15;
        Debug.Log($"<HEV Suit Recharge Sound>, now it is {PlayerData.PlayerHealth}.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
