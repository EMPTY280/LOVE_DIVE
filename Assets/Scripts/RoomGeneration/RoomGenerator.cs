using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // ------------------------------------------------------------
    // ------------------------ [ �� ���� ] ------------------------
    // ------------------------------------------------------------
    [SerializeField] [Tooltip("�� ��ũ�� �ӵ��Դϴ�.")]
    private float scrollSpeed = 2.5f;
    [SerializeField] [Tooltip("�ִ� ���߾��ϴ� �ð��Դϴ�.")]
    private float timeMax = 60f;
    private float timeCurr = 0f;
    [SerializeField] [Tooltip("��� ���� �̸��Դϴ�.")]
    private string resultSceneName = "Result";

    // ------------------------------------------------------------
    // ------------------------ [ �� ��ġ ] ------------------------
    // ------------------------------------------------------------

    [SerializeField] [Tooltip("�� �Ÿ����� ���� �����˴ϴ�. �Ȱ��� �Ÿ� ���Ϸ� �����Ͻʽÿ�.")]
    private float sightDist = 20f;
    private List<Room> currentRooms = new List<Room>();
    [SerializeField] [Tooltip("���� �� ����� ����� ���� Ʈ�������Դϴ�.")]
    private Transform startRooms = null;
    [SerializeField] [Tooltip("������ ����� ���� Ʈ�������Դϴ�.")]
    private Transform randomRooms = null;
    [SerializeField] [Tooltip("�� ��ġ���� ���� ���� �������� �������� �ʽ��ϴ�.")]
    private int roomDelay = 0;
    // ������ ����� ����ų Ʈ������
    private Transform delayedRooms = null;

    // ------------------------------------------------------------
    // ------------------------ [ �� ���� ] ------------------------
    // ------------------------------------------------------------

    // �� ���� ���� �̵��� �Ÿ�, ��� �̵��� ��� ����
    [SerializeField] private float offset = 0.0f;
    // ���� ���� ���� ��ġ. (= ���� ī�޶��� ��ġ)
    private Vector3 backPos = Vector3.zero;

    private void Start()
    {
        if (startRooms == null || randomRooms == null)
            enabled = false;
        SoundManager.Instance.PlayBGM("BGM_Ingame");
        if (roomDelay > 0)
        {
            delayedRooms = new GameObject().transform;
            delayedRooms.name = "DelayedRooms";
        }

        backPos = Camera.main.transform.position;
        transform.position = Vector3.zero;
        Vector3 prevPos = backPos;

        // ���� ��� ��������
        bool isFirstRoom = true;
        while (startRooms.childCount > 0)
        {
            Room item = startRooms.GetChild(0).GetComponent<Room>();
            PullRoom(item);
            if (isFirstRoom)
            {
                isFirstRoom = false;
                prevPos.z += item.RoomSize.z * 0.5f;
            }    
            item.transform.position = prevPos;
            prevPos.z += item.RoomSize.z;
        }
        offset += currentRooms[0].RoomSize.z * 0.5f;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        offset += deltaTime * scrollSpeed;
        float totalDist = 0.0f;

        // �ð� ����
        timeCurr += deltaTime;
        UIControl ui = UIControl.Instance;
        if (ui != null)
            ui.UpdateProgressBar(timeCurr / timeMax);
        if (timeCurr >= timeMax)
            GameManager.Instance.ChangeScene(resultSceneName);

        // ����Ʈ�� ��� ���� ���� �̵���Ŵ.
        foreach (Room item in currentRooms)
        {
            item.transform.Translate(Vector3.back * deltaTime * scrollSpeed);
            totalDist += item.RoomSize.z;
        }
        totalDist -= offset;

        // ���� (�þ� �Ÿ������� ����) < (��� ���� ���� - ������) �̸� �� ���� �ε�
        if (sightDist > totalDist && randomRooms.childCount > 0) 
        {
            Room nextRoom = GetRandomRoom();
            Room lastRoom = transform.GetChild(transform.childCount - 1).GetComponent<Room>();
            Vector3 newPos = lastRoom.transform.position;
            newPos.z += 0.5f * (nextRoom.RoomSize.z + lastRoom.RoomSize.z);
            nextRoom.transform.position = newPos;
            PullRoom(nextRoom);
        }

        if (currentRooms.Count < 1) return;

        // ������ ���� �� ������ ���� ��� �ʱ�ȭ �� ��������
        Room frontRoom = currentRooms[0];

        float zPos = frontRoom.transform.position.z;
        float endZPos = backPos.z - frontRoom.RoomSize.z;

        if (zPos <= endZPos)
        {
            currentRooms.RemoveAt(0);
            offset -= frontRoom.RoomSize.z;
            if (roomDelay < 1)
                frontRoom.transform.parent = randomRooms;
            frontRoom.ResetPos();
        }
    }

    //
    private Room GetRandomRoom()
    {
        int childCount = randomRooms.childCount;
        Transform t = randomRooms.GetChild(Random.Range(0, childCount));
        return t.GetComponent<Room>();
    }

    // �� ���� ���� ����Ʈ�� �ְ�, ���� �θ� ������Ʈ�� �ڽ����� �缳��
    private void PullRoom(Room r)
    {
        currentRooms.Add(r);
        r.transform.parent = transform;
        r.ActiveRoom();
    }
}
