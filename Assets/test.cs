using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 15.0f;

    // Update is called once per frame
    void Update()
    {
        // 가로축 입력 값
        float hor = Input.GetAxis("Horizontal");
        // 새로 적용할 각도, 먼저 기존 각도를 받아옴
        Vector3 newAngle = transform.localRotation.eulerAngles;

        // 목표 각도로 변경
        newAngle.y -= hor * Time.deltaTime * rotateSpeed;

        // 정면 각도를 0f로 잡을 때, 음수로 갈 경우 내부적으로는 360도 이내로 적용되기 때문에,
        // (-1도 == 359도) 180 초과 값이 나온 경우 360을 빼서 음수로 변경
        if (newAngle.y > 180)
            newAngle.y -= 360;

        // 그러면 이제 clamp 적용 가능
        newAngle.y = Mathf.Clamp(newAngle.y, -20f, 20f);

        // 만세만세
        transform.eulerAngles = newAngle;
    }
}