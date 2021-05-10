using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
      
    [Header ("Player Movement")]
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int Health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.8f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.2f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed=1f;
    [SerializeField] float projectileFiringPeriod = 0.1f; //koristim je za brzinu automatskog pucanja
    Coroutine firingCoroutine;
    // Start is called before the first frame update

    float xMin;
    float xMax;
    float yMin;
    float yMax; //ovo nam je granicnik dokle se krecemo jer nes ti nidje gdje ne smijes
    void Start()
    {
        SetUpMoveBoundaries();
        //StartCoroutine(PrintAndWait());

    }



    // Update is called once per frame
    void Update()
    {
        Move();
        /*   MoveX();  ///ova soucija koja je aktivna ima manje poziva i zato je bolja od ove maskirane
           MoveY();*/
        Fire();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } //sad nezz sto ako ne postoji da li je mozda zato da ne zove funkciju bzv nego da izadje 
        ProcessHit(damageDealer); //ako postoji buce damage i ide dalje 
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        Health -= damageDealer.GetDamage(); //ako si pogodjen onda ti je helt vjerovatno manji nije veci 
        //mada ima ona narodna "sto te ne ubije to te ojaca"  e to ovdje nije slucaj jer ovo je moja igrica i ja ovdje postavljam pravila ako ti se ne svidja nemoj igrati poyy
        damageDealer.Hit(); //pri koliziji metak nestaje
        if (Health <= 0) //ako ti je helt nula onda si krepo jbg 
        {
            Die();
        }
    }
    
    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver(); //ako izgubim ucitacu zadnju scenu 
        Destroy(gameObject); //i unistiti objekat tj igraca
        //FindObjectOfType<Level>().LoadGameOver();
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume); //ovo je efekat odsviravanja mrtvacke pjesme jer jbg krepo si u igrici mora malo efekta da ima 
    }



    public int GetHealth()
    {
        return Health; //uzeli helt, uglavnom je za koristenje ispisa na ekranu da vidimo status igraca 
    }

    /*   IEnumerator PrintAndWait()
       {
           Debug.Log("Firs message sent, boss");
           yield return new WaitForSeconds(3);
           Debug.Log("The Second message, boss");
       }*/
    private void Fire()
    {
        if(Input.GetButtonDown("Fire1")){
           firingCoroutine = StartCoroutine(FireContiniously()); //posto je pozivam po svakom updateu onda ce ona provjeriti drzim li "fire1" dugme koje mi je namjesteno u unityu opciono
        }
        if (Input.GetButtonUp("Fire1")) //ova funkcija GetButtonUp je za provjeru ako gumb nije pritisnut, znaci: nije pritisnut = true
        {
            StopCoroutine(firingCoroutine); //napravio sam da prestane pucati ako sam vec poceo jer ako ovo ne napravim pucace konstantno bez prestanka
        }
    }

    IEnumerator FireContiniously() //enmeracija za konstantno pucanje 
    {
        while (true)//true is always true
        {
            GameObject laser = Instantiate(laserPrefab,
                transform.position,///pravo da si velim da ovaj transform.position mi nije jasan ,stvori stvar tamo gdje treba a nezz kako jebo ga patak
                Quaternion.identity) as GameObject; //napravice klona od lasera tako da mozemo pucati lasere konstantno jer fakticki pravimo klonove
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed); //podesavanje brzine lasera
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);//ucinice ovo opet da period pucanja koji smo odabrali

        }
    }

    private void Move() //ovo je kretanje i funkcija mozda djeluje interesantno i fino al zapravo je kurac od ovce i kako je napisa pojma nemam 
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed; //ovo je pojedinacni pomjeraj po x osi (tj pomjeraj jednog frejma
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed; //isto ovo gore samo y
        //da naglasim da GetAxis sam uzima gumb koji je pritisnut za pomjeranje i provjeri ga da li je taj i izvrsi,  definisemo u unityu koji ce to gumb da bude 
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax); //provjera nove X pozicije da li je prekoracila granice uz pomoc Mathf.Clamp();
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax); //isto ovo gore samo y 

        transform.position = new Vector2(newXPos, newYPos); //i sad vrsimo pomjeraj nakon sto smo utvrdili da granica nije predjena
    }

    /*private void MoveX()
    {
        var deltaX = Input.GetAxis("Horizontal")* Time.deltaTime*GameSpeed;
        Debug.Log(deltaX);
        var newXPos = transform.position.x + deltaX;
        transform.position = new Vector2(newXPos, transform.position.y);
    }
    private void MoveY()
    {
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * GameSpeed;
        Debug.Log(deltaY);
        var newYPos = transform.position.y + deltaY;
        transform.position = new Vector2(transform.position.x, newYPos);
    }*/

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;//sluzi nam da ogranicimo povrsinu igrice na osnovu povrsine koju kamera obuhvata +padding koji nam je tu dodatak po nasoj mjeri
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;//pa posto pretvaramo kameru u oblast gdje igra se igra onda pokupimo maksimalne i minimalne kote
        //uz padding koji nam je tu radi korekcije da igrac ne izlazi izvan prostora kamere
    }

}
