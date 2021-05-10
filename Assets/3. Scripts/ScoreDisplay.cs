using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //ako bi koristili TMP onda je neophodno upaliti i ovu biblioteku jer nece jer tostring ne moze raditi ako tmp iz unitiya pokusamo povezati sa 
//obicnim tekstom iz skripte

public class ScoreDisplay : MonoBehaviour
{
    //[SerializeField] Text scoreText;
    Text scoreText;
    GameSession gameSession;

    void Start()
    {
        scoreText = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        scoreText.text = gameSession.GetScore().ToString(); //score koji je tipa int potrebno je pretvoriti u tekst da  bi ga mogli ispisati
    }
}
