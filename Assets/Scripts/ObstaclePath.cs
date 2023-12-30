using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstaclePath : MonoBehaviour
{
    [SerializeField] private Vector3[] path;
    [SerializeField] private bool closedPath = true;

    [SerializeField] private float moveSpeed = 2.0f;
    private int currNode = 0;

    private void Awake()
    {
        if (path.Length < 2) enabled = false;
    }

    private void Update()
    {
        Vector3 targetPos = transform.rotation * path[(currNode + 1) % path.Length] + transform.position;
        Transform child = transform.GetChild(0);
        Vector3 newPos = Vector3.MoveTowards(child.position, targetPos, moveSpeed * Time.deltaTime);
        child.position = newPos;
        if (newPos == targetPos)
        {
            currNode = (currNode + 1) % path.Length;
            if (!closedPath && currNode == path.Length - 1)
            {
                child.position = path[0] + transform.position;
                currNode = 0;
            }
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;

        if (path.Length < 2) return;
        Gizmos.color = Color.red;
        Handles.color = Color.red;

        Vector3[] pathpos = (Vector3[])path.Clone();
        for (int i = 0; i < pathpos.Length; i++)
            pathpos[i] = transform.rotation * pathpos[i] + origin;

        Gizmos.DrawLineStrip(pathpos, closedPath);
        for (int i = 0; i < path.Length; i++)
        {
            Vector3 pos = origin + (transform.rotation * path[i]);
            Gizmos.DrawSphere(pos, 0.18f);
            Handles.Label(pos + Vector3.up * 0.5f, "p" + i);
        }
    }
#endif
}
