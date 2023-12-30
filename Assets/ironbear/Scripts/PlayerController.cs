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
    private float invincibleDuration = 2.5f;

    private bool isDead = false;
    private bool isInvincible = false;
    private float invincibleStartTime;
    //private float returnDuration = 3f;
    private Vector3 startPosition;
    
    private IInput playerInput;
    private Rigidbody rigid;
    private Collider playerCollier;


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

#if UNITY_STANDALONE || UNITY_EDITOR
        playerInput = new DesktopInput();
#endif
        
        startPosition = transform.position;
        playerMoveForce = 3f;
        healthPoint = 3;
        invincibleDuration = 2.5f;
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
        }
    }

    private void PlayerInput()
    {
        float hor = playerInput.GetHorizontal();
        float ver = playerInput.GetVertical();

        Vector3 direction = new Vector3(hor, ver, 0f).normalized;

        PlayerMove(direction);
    }

    private void PlayerMove(Vector3 direction)
    {
        Vector3 movement = direction * playerMoveForce * Time.deltaTime;
        //transform.Translate(movement);

        rigid.AddForce(movement, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && isInvincible == false)
        {
            Debug.Log("산타 아야해~");
            //after the collision action will be added here
            SantaOuch();
        }
    }

    private void SantaOuch()
    {
        isInvincible = true;
        invincibleStartTime = Time.time;

        if (healthPoint > 1)
        {
            healthPoint--;
            StartCoroutine(MoveBack());
            //StartCoroutine(Reposition());
            //HitAnimation();
        }
        else if (healthPoint == 1 || healthPoint < 1)
        {
            isDead = true;
            healthPoint = 0;
            StartCoroutine(MoveBack());
            //HitAnimation();
            GameOver();
        }
    }

    private void GameOver()
    {
        //when the game is over, this method will be called
        Debug.Log("그후로 아이들은 크리스마스에 선물을 받지 못하였다..");
    }

    //trash code~
    /*
    IEnumerator Reposition()
    {     
        yield return new WaitForSeconds(1f);
        playerCollier.enabled = false;

        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(0, 0, -4.5f);
        transform.position = Vector3.MoveTowards(transform.position, startPosition, playerMoveForce * Time.deltaTime);
    }

    private void HitAnimation()
    {
        rigid.velocity = Vector3.zero;

        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(0, 0, -10);

        
        float duration = 1f;
        float elapedTime = 0f;

        while (elapedTime < duration)
        {
            transform.position = Vector3.Lerp(curPos, targetPos, duration);
            elapedTime += Time.deltaTime;
        }

        transform.position = targetPos;
        

        Vector3 backVec = targetPos - curPos;
        backVec.Normalize();
        rigid.AddForce(backVec * playerMoveForce, ForceMode.Impulse);
        
    }
    */
    
    IEnumerator MoveBack()
    {
        Vector3 initPos = transform.position;
        Vector3 targetPos = new Vector3(0f, 0f, -10f);

        while(transform.position!=targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 5f);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(Reposition());
    }

    IEnumerator Reposition()
    {
        Vector3 returnPos = new Vector3(0f, 0f, -4.5f);
        while (transform.position != returnPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnPos, Time.deltaTime * 5f);
            yield return null;
        }

        rigid.velocity = Vector3.zero;
    }
}
