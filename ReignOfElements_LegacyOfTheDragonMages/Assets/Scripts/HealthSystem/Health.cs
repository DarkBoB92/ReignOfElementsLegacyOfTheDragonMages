using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth, currentHealth;
    [SerializeField] float burningTime, wetnessTime;
    [SerializeField] bool burning, wet;
    public Type damageType;

    private void Awake()
    {
        currentHealth = maxHealth;
        burningTime = 3;
        wetnessTime = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (burning)
        {
            burningTime -= Time.deltaTime;
        }
    }

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
    void NaturalDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void NormalDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
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
            Destroy(this.gameObject);
        }
    }

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
            if(burningTime <= 0)
            {
                burning = false;
                burningTime = 3;
            }
        }
    }

    void WaterDamage(int damageAmount)
    {
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
            Destroy(this.gameObject);
        }
    }

    IEnumerator ApplyWetness()
    {
        while (wet)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>(); 
            if (rb != null)
            {
                rb.velocity *= 0.5f; 
            }
            yield return new WaitForSeconds(wetnessTime);
            wet = false;
        }
    }

    void EarthDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }    
}
