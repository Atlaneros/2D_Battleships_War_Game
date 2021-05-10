using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyPathing : MonoBehaviour
{
    [SerializeField] WaveConfig waveConfig;
    List<Transform> waypoints;
    [SerializeField] float moveSpeed = 2f;
    int waypointIndex = 0;
    

    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig; 
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1) //izvrsava se sve dok je broj waypointsa veci ili jednak waypointindex koji se u povecava pomjerajima
        {
            var targetPosition = waypoints[waypointIndex].transform.position; //uzimamo poziciju waypointa i dajemo je targetu da znamo kuda idemo 
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime; //pomjeraj ce se ovdje stabilizovati bez obzira na frejmove 
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame); //sad cuvena funkcija
            if (transform.position == targetPosition)
            {
                waypointIndex++; //putanja je podjeljena na vise tacaka kad dodje do jedne tacke povecace se indeks da onda ide na sledecu tacku
            }
        }
        else
        {
            Destroy(gameObject);//ako je stigao na odrediste bice unisten 
        }
    }
}
