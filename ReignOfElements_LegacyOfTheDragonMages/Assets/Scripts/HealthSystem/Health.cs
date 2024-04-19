using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Health : MonoBehaviour
{
    [Header("Health Variables")]
    [SerializeField] int maxHealth;
    public int currentHealth;

    [Header("Status Alterations and its Duration")]
    public bool burning;
    [SerializeField] float burningTime;
    public bool wet;
    [SerializeField] float wetnessTime;
    public bool stunned;
    [SerializeField] float stunTime;

    [Header("Damage Type")]
    public Type damageType;

    // On Awake() sets current health to it's maximum and the default values for status duration 
    private void Awake()
    {
        currentHealth = maxHealth;
        burningTime = 3;
        wetnessTime = 3;
        stunTime = 3;
    }

    void Update()
    {
       
    }

    // On execution with a switch it activates the related damage depending on the damage Type 
    public void Damage(int damage, Type spellType)
    {
        switch (spellType)
        {
            case Type.None:
                damageType = spellType;
                NaturalDamage(damage);
                break;
            case Type.Normal:
                damageType = spellType;
                NormalDamage(damage);
                break;
            case Type.Fire:
                damageType = spellType;
                FireDamage(damage);
                break;
            case Type.Water:
                damageType = spellType;
                WaterDamage(damage);
                break;
            case Type.Earth:
                damageType = spellType;
                EarthDamage(damage);
                break;
        }
    }

    // This method does non magical damage
    void NaturalDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // This method does magical damage
    void NormalDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // This method does fire magical damage and burns the enemy by starting a Coroutine that handles the burning damage 
    void FireDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth > 0)
        {
            if (burning)
            {
                burningTime = 3;
            }
            else
            {
                burning = true;
                StartCoroutine(BurnDamage());
            }
        }
        else
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    // This is the Coroutine that handles the burning damage
    IEnumerator BurnDamage()
    {                
        while (burning)
        {
            yield return new WaitForSeconds(1.5f);
            currentHealth -= 1;
            if (currentHealth <= 0)
            {
                Destroy(this.gameObject); 
                break;
            }
            yield return new WaitForSeconds(burningTime);
            burning = false;           
        }
    }

    // This method does fire magical damage and burns the enemy by starting a Coroutine that handles the wet status
    void WaterDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth > 0)
        {
            if (wet)
            {
                wetnessTime = 3;
            }
            else
            {
                wet = true;
                StartCoroutine(ApplyWetness());
            }
        }
        else
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    // This is the Coroutine that handles the wet status duration
    IEnumerator ApplyWetness()
    {
        while (wet)
        {            
            yield return new WaitForSeconds(wetnessTime);
            wet = false;
        }
    }

    // This method does fire magical damage and burns the enemy by starting a Coroutine that handles the stunned status
    void EarthDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth > 0)
        {
            if (stunned)
            {
                stunTime = 0;
            }
            else
            {
                stunned = true;
                StartCoroutine(ApplyStun());
            }
        }
        else
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    // This is the Coroutine that handles the stunned status duration
    IEnumerator ApplyStun()
    {
        while (stunned)
        {
            yield return new WaitForSeconds(stunTime);
            stunned = false;
        }        
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
