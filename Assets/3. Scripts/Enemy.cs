using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")] //ovdje sva ta sranja koja koristim u nastavku al lakse serializovati pa ih u unitiyu podesavam
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile; //ovo je projektil koji koristim za kloniranje a ucitavam ga u unityu 
    [SerializeField] float projectileSpeed = 50f;

    [Header("Sound Effects")]
    [SerializeField] GameObject deathVFX;  //ovaj spreman vfx u unitiyu cemo koristiti za kloniranje 
    [SerializeField] float durationOfExplosion=1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.8f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.2f;

    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot() //pa bilo bi malo bezveze ga se previdi kad ce enemi da puca pa onda malo randomizacije nije na odmet
    {
        shotCounter -= Time.deltaTime; //znaci ovako, svaki put kad se pozove funkcija u updateu frejm po delta timeu ce se oduzet vrijeme od countera kad ce da osuta
        if(shotCounter<= 0f) //dok ne dodje do nule
        {
            Fire(); //osuta
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots); //i sad da ne bi  nastavio sutati po svakom frejmu mi njemu damo novu vrijednost koja ce biti random jer 
            //ako postavimo konstantan broj predvidjecemo onda sutanja;
        }
    }

    private void Fire() //funkcija napravljena da enemi ispali laser da ne budem jedini koji puca pa koja svrha igranja
    {
        GameObject laser = Instantiate(
            projectile,
            transform.position, //position ce biti na poziciji enemija i pretice ga tako da ne moram unositi nikakve kote jer je skripta na enemiju samim tim je i pozicija 
            Quaternion.identity) as GameObject; //Quaternion.identity predstavlja da nema rotacije tj da ostaje ista kao i kod roditelja 
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        //projectile speed je negativan zato sto ide ka dole
    }

    private void OnTriggerEnter2D(Collider2D other) //znaci ako su se vec sudarili bilo bi dobro i da se povrijede a ne da prolaze jedni kroz druge 
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die(); //krepo a moze se kazati i da je ubiven 
        }
    }

    private void Die() //enemi umire isto kao i igrac sao eto mozda uz drugu muziku 
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation); //klonirace se vizuelni efekat i dogodice se n enemiju na kom se ispuni uslov tj da je krepo
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
