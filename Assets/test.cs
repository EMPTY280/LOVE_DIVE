using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 15.0f;

    // Update is called once per frame
    void Update()
    {
        // ������ �Է� ��
        float hor = Input.GetAxis("Horizontal");
        // ���� ������ ����, ���� ���� ������ �޾ƿ�
        Vector3 newAngle = transform.localRotation.eulerAngles;

        // ��ǥ ������ ����
        newAngle.y -= hor * Time.deltaTime * rotateSpeed;

        // ���� ������ 0f�� ���� ��, ������ �� ��� ���������δ� 360�� �̳��� ����Ǳ� ������,
        // (-1�� == 359��) 180 �ʰ� ���� ���� ��� 360�� ���� ������ ����
        if (newAngle.y > 180)
            newAngle.y -= 360;

        // �׷��� ���� clamp ���� ����
        newAngle.y = Mathf.Clamp(newAngle.y, -20f, 20f);

        // ��������
        transform.eulerAngles = newAngle;
    }
}