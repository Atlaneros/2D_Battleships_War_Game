using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text healthText;
    Player player;

    void Start()
    {
        healthText = GetComponent<Text>();  //za manipulaciju tekstom iz unitya potrebno je uzeti taj tekst funkcijom 
        player = FindObjectOfType<Player>(); //ugradili ugradili smo igracu helt pa onda je potrebno i njega da uzmemo
    }

    void Update()
    {
        healthText.text = player.GetHealth().ToString(); //pretvori to u string da se ne zajebavamo
    }
}
