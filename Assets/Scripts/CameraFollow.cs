using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget = null;
    [SerializeField] private float moveMultiplier = 0.5f;

    private float shakeForce = 0f;
    private bool isShaking = false;

    private void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x = followTarget.position.x * moveMultiplier + Random.Range(-shakeForce*0.5f, shakeForce*0.5f);
        newPos.y = followTarget.position.y * moveMultiplier + Random.Range(-shakeForce * 0.5f, shakeForce * 0.5f);
        transform.position = newPos;
    }

    public void Shake(float force, float duration)
    {
        if (isShaking) return;
        shakeForce = force;
        StartCoroutine(CShake(duration));
    }

    private IEnumerator CShake(float duration)
    {
        float decrease = shakeForce / duration;
        while(shakeForce > 0f)
        {
            shakeForce -= decrease * Time.deltaTime;
            yield return null;
        }
    }
}
