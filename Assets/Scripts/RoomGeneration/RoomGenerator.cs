using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // ------------------------------------------------------------
    // ------------------------ [ 룸 진행 ] ------------------------
    // ------------------------------------------------------------
    [SerializeField] [Tooltip("맵 스크롤 속도입니다.")]
    private float scrollSpeed = 2.5f;
    [SerializeField] [Tooltip("최대 버텨야하는 시간입니다.")]
    private float timeMax = 60f;
    private float timeCurr = 0f;
    [SerializeField] [Tooltip("결과 씬의 이름입니다.")]
    private string resultSceneName = "Result";

    // ------------------------------------------------------------
    // ------------------------ [ 룸 배치 ] ------------------------
    // ------------------------------------------------------------

    [SerializeField] [Tooltip("이 거리까지 룸이 생성됩니다. 안개의 거리 이하로 설정하십시오.")]
    private float sightDist = 20f;
    private List<Room> currentRooms = new List<Room>();
    [SerializeField] [Tooltip("시작 시 연결될 방들을 담은 트랜스폼입니다.")]
    private Transform startRooms = null;
    [SerializeField] [Tooltip("생성될 방들을 담은 트랜스폼입니다.")]
    private Transform randomRooms = null;
    [SerializeField] [Tooltip("이 수치동안 같은 방이 연속으로 등장하지 않습니다.")]
    private int roomDelay = 0;
    // 제거한 방들을 대기시킬 트랜스폼
    private Transform delayedRooms = null;

    // ------------------------------------------------------------
    // ------------------------ [ 룸 제거 ] ------------------------
    // ------------------------------------------------------------

    // 맨 앞의 룸을 이동한 거리, 모두 이동한 경우 제거
    [SerializeField] private float offset = 0.0f;
    // 가장 앞의 룸의 위치. (= 메인 카메라의 위치)
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

        // 시작 방들 가져오기
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

        // 시간 진행
        timeCurr += deltaTime;
        UIControl ui = UIControl.Instance;
        if (ui != null)
            ui.UpdateProgressBar(timeCurr / timeMax);
        if (timeCurr >= timeMax)
            GameManager.Instance.ChangeScene(resultSceneName);

        // 리스트의 모든 현재 룸을 이동시킴.
        foreach (Room item in currentRooms)
        {
            item.transform.Translate(Vector3.back * deltaTime * scrollSpeed);
            totalDist += item.RoomSize.z;
        }
        totalDist -= offset;

        // 만약 (시야 거리까지의 길이) < (모든 룸의 길이 - 오프셋) 이면 방 새로 로드
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

        // 마지막 방이 룸 끝까지 갔을 경우 초기화 후 돌려보냄
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

    // 그 룸을 현재 리스트에 넣고, 룸의 부모 오브젝트를 자신으로 재설정
    private void PullRoom(Room r)
    {
        currentRooms.Add(r);
        r.transform.parent = transform;
        r.ActiveRoom();
    }
}
