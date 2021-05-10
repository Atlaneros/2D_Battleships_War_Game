using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100; //definisemo damage objekta koliki ce da bude 

    public int GetDamage()
    {
        return damage; //cilj je samo da pozovome ovu funkciju koja ce nam vratiti damage, ovo naravno moze biti znatno kompleksnije ako na napad ima vise faktora (shield, melee, range...)
    }
    
    public void Hit()
    {
        Destroy(gameObject); //poziv za unistavanje lasera pri koliziji ce uglavnom biti pozvana
    }
}
