using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IInput playerInput;
    private Rigidbody rigid;
    private BoxCollider playerCollier;

    [SerializeField]
    private float playerMoveForce = 3f;

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
}
