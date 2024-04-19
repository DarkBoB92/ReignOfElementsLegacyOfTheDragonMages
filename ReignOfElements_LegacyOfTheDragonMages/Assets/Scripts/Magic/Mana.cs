using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Mana : MonoBehaviour
{
    [Header("Health Variables")]
    [SerializeField] int maxMana;
    public int currentMana;

    private void Awake()
    {
        currentMana = maxMana;
    }

    public void UseMana(Type spellType)
    {
        switch (spellType)
        {
            case Type.Normal:
                currentMana -= 2;
                break;
            case Type.Fire:
                currentMana -= 4;
                break;
            case Type.Water:
                currentMana -= 3;
                break;
            case Type.Earth:
                currentMana -= 5;
                break;
        }
        
    }

    public bool CheckManaAmount()
    {
        if (currentMana > 0)
        {            
            return true;
        }
        else
        {
            currentMana = 0;
            return false;
        }
    }

    public int GetMaxMana()
    {
        return maxMana;
    }
}
