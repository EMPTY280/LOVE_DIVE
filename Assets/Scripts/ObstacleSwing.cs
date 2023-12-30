using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleSwing : MonoBehaviour
{
    [SerializeField]
    [Tooltip("시계방향 회전 여부입니다.")]
    private bool isClockwise = true;
    [SerializeField]
    [Tooltip("회전 속도입니다.")]
    private float speed = 5f;

    private void Update()
    {
        Vector3 dir = Vector3.forward;
        if (isClockwise)
            dir *= -1;
        transform.Rotate(dir * speed * Time.deltaTime);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Handles.color = Color.red;
        
        Gizmos.DrawSphere(transform.position, 0.3f);

        Handles.DrawWireArc(transform.position, transform.forward, transform.up, 360, 2, 2f);
        Vector3[] tri = new Vector3[3];

        int dir = -1;
        if (isClockwise)
            dir = 1;
        tri[0] = transform.position + transform.right * 2 - transform.up * 0.25f * dir;
        tri[1] = transform.position + transform.right * 1.5f + transform.up * 0.25f * dir;
        tri[2] = transform.position + transform.right * 2.5f + transform.up * 0.25f * dir;
        Gizmos.DrawLineStrip(tri, true);
    }
#endif
}
