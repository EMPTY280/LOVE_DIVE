using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerMoveForce = 3f;
    [SerializeField]
    private int healthPoint = 3;

    private IInput playerInput;
    private Rigidbody rigid;
    private BoxCollider playerCollier;


    private void Start()
    {
        if(rigid==null)
        {
            rigid = GetComponent<Rigidbody>();
        }
        if(playerCollier==null)
        {
            playerCollier = GetComponent<BoxCollider>();
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        playerInput = new DesktopInput();
#endif

        healthPoint = 3;
        rigid.useGravity = false;
    }

    public PlayerController(IInput input)
    {
        this.playerInput = input;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        float hor = playerInput.GetHorizontal();
        float ver = playerInput.GetVertical();

        Vector3 direction = new Vector3(hor, 0f, ver).normalized;
        PlayerMove(direction);
    }

    private void PlayerMove(Vector3 direction)
    {
        Vector3 movement = direction * playerMoveForce * Time.deltaTime;
        transform.Translate(movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("산타 아야해~");
            //after the collision action add here
            SantaOuch();
        }
    }

    private void SantaOuch()
    {
        if (healthPoint > 1)
        {
            healthPoint--;
        }
        else if (healthPoint == 1 || healthPoint < 1)
        {
            healthPoint = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        //when the game is over, this method will be called
        Debug.Log("그후로 아이들은 크리스마스에 선물을 받지 못하였다..");
    }
}
