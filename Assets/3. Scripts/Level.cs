 using System.Collections; 
 using System.Collections.Generic; 
 using UnityEngine; 
 using UnityEngine.SceneManagement; 
 
 
 public class Level : MonoBehaviour
{ 

    [SerializeField] float delayInSeconds = 2f; //koristicemo za ucitavanje sledeceg levela pa malo nekog dileja napraviti cisto radi estetike

 
    public void LoadStartMenu()
     { 
        SceneManager.LoadScene(0); //ucitaj pocetak iliti start
     } 

     public void LoadGame()
     { 
        SceneManager.LoadScene("Game"); //ucitaj igru 
        FindObjectOfType<GameSession>().ResetGame(); //u sessionu cuvamo skorove i stosta pa valja ih ocistiti po pocetku igrice da nas ne bi jebali kasnije
        //a reset game sluzi da unisti trazeni objekat u ovom slucaju to nam je score
     } 
 
 
    public void LoadGameOver()
    { 
        StartCoroutine(WaitAndLoad()); //korutina za ucitavanje kraja igrice jer cemo imati onaj gore dilej
     } 

 
    IEnumerator WaitAndLoad()
   { 
         yield return new WaitForSeconds(delayInSeconds); //evo ga dilej pozivom WaitForSeconds(pa koliki nam je dilej), funkcija tako reci zaledi odradjivanje funkcije koju izvrsavamo na odredjeno vrijeme
         SceneManager.LoadScene("Game Over"); //i evo ga sad ucitam kraj igrice
    } 

 
    public void QuitGame()
    { 
        Application.Quit(); //ajd ovo je valjda poznato da se mora izaci iz igrice kad se zavrsi da ne bi komp morao gasiti igrac jer to prao nema smisla 
    } 

 
} 

