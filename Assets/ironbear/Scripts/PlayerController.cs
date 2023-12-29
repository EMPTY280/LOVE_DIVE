using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Move Settings")]
    [SerializeField]
    private float playerMoveForce = 3f;
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float acceleration = 5f;

    [Header("The Other Settings")]
    [SerializeField]
    private int healthPoint = 3;
    [SerializeField]
    private float invincibleDuration = 5f;

    private bool isDead = false;
    private bool isInvincible = false;
    private float invincibleStartTime;

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
        invincibleDuration = 5f;
        maxSpeed = 5f;
        acceleration = 5f;
        rigid.useGravity = false;
    }

    public PlayerController(IInput input)
    {
        this.playerInput = input;
    }

    private void Update()
    {
        PlayerInput();

        if (isInvincible && Time.time - invincibleStartTime > invincibleDuration)
        {
            isInvincible = false;
        }
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
        //transform.Translate(movement);

        rigid.AddForce(movement, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && isInvincible == false)
        {
            Debug.Log("��Ÿ �ƾ���~");
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
        }
        else if (healthPoint == 1 || healthPoint < 1)
        {
            isDead = true;
            healthPoint = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        //when the game is over, this method will be called
        Debug.Log("���ķ� ���̵��� ũ���������� ������ ���� ���Ͽ���..");
    }
}
