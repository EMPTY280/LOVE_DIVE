using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstaclePath : MonoBehaviour
{
    [SerializeField] private Vector3[] path;
    [SerializeField] private bool closedPath = true;

    [SerializeField] private float moveSpeed = 2.0f;
    private Vector3 startPos = Vector3.one;
    private int currNode = 0;

    private void Awake()
    {
        if (path.Length < 2) enabled = false;
        startPos = transform.position;
    }

    private void Update()
    {
        Vector3 targetPos = path[(currNode + 1) % path.Length] + startPos;
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = newPos;
        if (newPos == targetPos)
        {
            currNode = (currNode + 1) % path.Length;
            if (!closedPath && currNode == path.Length - 1)
            {
                transform.position = path[0] + startPos;
                currNode = 0;
            }
        }
    }

#if UNITY_EDITOR
    private bool startPosInit = false;

    private void Start()
    {
        startPosInit = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        if (startPosInit)
            origin = startPos;

        if (path.Length < 2) return;
        Gizmos.color = Color.red;
        Handles.color = Color.red;

        Vector3[] pathpos = (Vector3[])path.Clone();
        for (int i = 0; i < pathpos.Length; i++)
            pathpos[i] += origin;

        Gizmos.DrawLineStrip(pathpos, closedPath);
        for (int i = 0; i < path.Length; i++)
        {
            Vector3 pos = origin + path[i];
            Gizmos.DrawSphere(pos, 0.18f);
            Handles.Label(pos + Vector3.up * 0.5f, "p" + i);
        }
    }
#endif
}
