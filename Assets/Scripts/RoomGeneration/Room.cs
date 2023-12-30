using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Vector3 roomSize = Vector3.one;
    [SerializeField] [Tooltip("한 번만 등장하고, 이후 파괴됩니다.")]
    private bool noRepeat = false;
    private Vector3 originPos = Vector3.zero;

    public Vector3 RoomSize
    {
        get { return roomSize; }
    }

    public bool NoRepeat
    {
        get { return noRepeat; }
    }

#if UNITY_EDITOR
    [SerializeField]
    private Color gizmoColor = Color.green;
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, roomSize);
    }
#endif

    private void Awake()
    {
        originPos = transform.position;
        gameObject.SetActive(false);
    }

    public void ResetPos()
    {
        if (noRepeat)
            Destroy(gameObject);
        transform.position = originPos;
        gameObject.SetActive(false);
    }

    public void ActiveRoom()
    {
        gameObject.SetActive(true);
    }
}
