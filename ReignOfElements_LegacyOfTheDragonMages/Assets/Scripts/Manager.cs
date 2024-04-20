using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpellType;
using UnityEditor.Animations;

public class Manager : MonoBehaviour
{
    [Header("Reference Variables")]
    [SerializeField] Type playerTransformation;
    [SerializeField] GameObject player;
    [SerializeField] Health playerHealth;
    [SerializeField] Mana playerMana;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Animator playerAnimator;

    [Header("Transformation Lists For Player")]
    [SerializeField] Sprite[] spriteTransformation;
    [SerializeField] AnimatorController[] animatorTransformation;
    
    [Header("Altar States")]
    [SerializeField] List<GameObject> altarsObject;
    [SerializeField] List<Altar> altars;
    [SerializeField] List<bool> altarState;

    [Header("Own Instance")]
    public static Manager instance;

    [Header("Game State Check")]
    static GameManagerUI gameManagerUI;
    public static bool gamePaused;


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
        
        //Referencing
        gameManagerUI = FindAnyObjectByType<GameManagerUI>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerMana = player.GetComponent<Mana>();
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            playerAnimator = player.GetComponent<Animator>();
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.sprite = spriteTransformation[0];
                playerAnimator.runtimeAnimatorController = animatorTransformation[0];
            }
        }

        altarsObject = new List<GameObject>(GameObject.FindGameObjectsWithTag("Altar"));
        GatherAltarsState();
    }

    void Update()
    {
        if (gameManagerUI.currentState == GameManagerUI.GameState.Playing)
        {
            CheckPlayerState();
            CheckWinningCondition();
            CheckPlayerTransformation();
        }
    }

    void CheckPlayerState()
    {
        if (player == null)
        {
            gameManagerUI.CheckGameState(GameManagerUI.GameState.GameOver);
        }
    }
    
    void GatherAltarsState()
    {        
        for (int i = 0; i < altarsObject.Count; i++)
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
        if (player != null)
        {
            playerTransformation = player.GetComponent<PlayerController>().transformation;
            if (playerSpriteRenderer != null)
            {
                switch (playerTransformation)
                {
                    case Type.Normal:
                        playerSpriteRenderer.sprite = spriteTransformation[0];
                        playerAnimator.runtimeAnimatorController = animatorTransformation[0];
                        break;
                    case Type.Fire:
                        playerSpriteRenderer.sprite = spriteTransformation[1];
                        playerAnimator.runtimeAnimatorController = animatorTransformation[1];
                        break;
                    case Type.Water:
                        playerSpriteRenderer.sprite = spriteTransformation[2];
                        playerAnimator.runtimeAnimatorController = animatorTransformation[2];
                        break;
                    case Type.Earth:
                        playerSpriteRenderer.sprite = spriteTransformation[3];
                        playerAnimator.runtimeAnimatorController = animatorTransformation[3];
                        break;
                }
            }
        }
    }
}
