using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget = null;
    [SerializeField] private float moveMultiplier = 0.5f;

    private void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x = followTarget.position.x * moveMultiplier;
        newPos.y = followTarget.position.y * moveMultiplier;
        transform.position = newPos;
    }
}
