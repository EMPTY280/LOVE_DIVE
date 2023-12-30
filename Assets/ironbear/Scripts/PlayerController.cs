using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Move Settings")]
    [SerializeField]
    private float playerMoveForce = 3f;


    [Header("The Other Settings")]
    [SerializeField]
    private int healthPoint = 3;
    [SerializeField]
    private float invincibleDuration = 3.5f;

    private bool isDead = false;
    private bool isInvincible = false;
    private float invincibleStartTime;
    
    private IInput playerInput;
    private Rigidbody rigid;
    private Collider playerCollier;
    private Animator playerAnim;


    private void Start()
    {
        if(rigid==null)
        {
            rigid = GetComponent<Rigidbody>();
        }
        if(playerCollier==null)
        {
            playerCollier = GetComponent<Collider>();
        }
        if (playerAnim == null)
        {
            playerAnim = GetComponent<Animator>();
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        playerInput = new DesktopInput();
#endif
        
        playerMoveForce = 3f;
        healthPoint = 3;
        invincibleDuration = 3.5f;
        rigid.useGravity = false;
    }

    public PlayerController(IInput input)
    {
        this.playerInput = input;
    }

    private void Update()
    {
        if(!isInvincible)
        {
            PlayerInput();
        }

        if (isInvincible && Time.time - invincibleStartTime > invincibleDuration)
        {
            isInvincible = false;
            playerCollier.enabled = true;
            playerAnim.SetBool("IsReturn", false);
        }
    }

    private void PlayerInput()
    {
        float hor = playerInput.GetHorizontal();
        float ver = playerInput.GetVertical();

        Vector3 direction = new Vector3(hor, ver, 0f).normalized;

        PlayerMove(direction);

        //left and down are negative and right and up are positive
        //Debug.Log(hor);
        //Debug.Log(ver);
    }

    private void PlayerMove(Vector3 direction)
    {
        Vector3 movement = direction * playerMoveForce * Time.deltaTime;
        rigid.AddForce(movement, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && isInvincible == false)
        {
            //Debug.Log("산타 아야해~");
            //after the collision action will be added here
            playerAnim.SetBool("IsHit", true);
            SantaOuch();         
        }
    }

    private void SantaOuch()
    {
        isInvincible = true;
        invincibleStartTime = Time.time;
        playerAnim.SetBool("IsHit", false);

        if (healthPoint > 1)
        {
            healthPoint--;
            StartCoroutine(MoveBack());
            StartCoroutine(Reposition());
        }
        else if (healthPoint == 1 || healthPoint < 1)
        {
            isDead = true;
            healthPoint = 0;
            StartCoroutine(MoveBack());
            GameOver();
        }
    }

    private void GameOver()
    {
        //when the game is over, this method will be called
        //Debug.Log("그후로 아이들은 크리스마스에 선물을 받지 못하였다..");
    }
    
    IEnumerator MoveBack()
    {
        Vector3 initPos = transform.position;
        Vector3 targetPos = new Vector3(0f, 0f, -12f);

        while(transform.position!=targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 5f);
            yield return null;
        }
    }

    IEnumerator Reposition()
    {
        yield return new WaitForSeconds(2.5f);
        playerAnim.SetBool("IsReturn", true);

        Vector3 returnPos = new Vector3(0f, 0f, -4.5f);
        while (transform.position != returnPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnPos, Time.deltaTime * 6f);
            yield return null;
        }

        rigid.velocity = Vector3.zero;
    }
}
