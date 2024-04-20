using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SpellType;

public class Manager : MonoBehaviour
{
    [Header("Reference Variables")]
    [SerializeField] Type playerTransformation; 
    Health playerHealth;
    SpriteRenderer player;

    [Header("Sprite List For Player")]
    [SerializeField] Sprite[] spriteTransformation;

    [Header("Altar States")]
    [SerializeField] GameObject[] altarsObject;
    [SerializeField] List<Altar> altars;
    [SerializeField] List<bool> altarState;

    [Header("Own Instance")]
    public static Manager instance;

    [Header("Game State Check")]
    static GameManagerUI gameManagerUI;
    public static bool gamePaused;
    public static bool newGame;

    private void Awake()
    {
        //Destroies itself so only one of this exists per scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    private void Start()
    {
        //Referencing
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        if (player != null)
        {
            player.sprite = spriteTransformation[0];
        }

        altarsObject = GameObject.FindGameObjectsWithTag("Altar");
        GatherAltarsState();
    }

    //TODO: Winning Condition, altars on
    void Update()
    {
        CheckPlayerState();
        CheckWinningCondition();
        CheckPlayerTransformation();
    }

    void CheckPlayerState()
    {
        if(playerHealth != null)
        {
            if(playerHealth.currentHealth <= 0)
            {
                //Set GameOver state for canvas
            }
        }
    }
    
    void GatherAltarsState()
    {
        for(int i = 0; i < altarsObject.Length; i++)
        {
            altars.Add(altarsObject[i].GetComponent<Altar>());
            altarState.Add(altars[i].activated);
        }
    }

    void CheckWinningCondition()
    {
        for (int i = 0; i < altarState.Count; i++)
        {
            altarState[i] = altars[i].activated;
        }
        if (altarState[0] && altarState[1] && altarState[2])
        {
            //Se WinGame state for canvas
            Debug.Log("You Won");
        }
    }

    void CheckPlayerTransformation()
    {
        playerTransformation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transformation;
        if (player != null)
        {
            switch (playerTransformation)
            {
                case Type.Normal:
                    player.sprite = spriteTransformation[0];
                    break;
                case Type.Fire:
                    player.sprite = spriteTransformation[1];
                    break;
                case Type.Water:
                    player.sprite = spriteTransformation[2];
                    break;
                case Type.Earth:
                    player.sprite = spriteTransformation[3];
                    break;
            }
        }
    }
}
