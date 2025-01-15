using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5f;
    public float moveDuration = 3f; // �ړ����s������

    private float timer = 0f; // �o�ߎ��Ԃ̃^�C�}�[
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;

        //�w�莞�ԕ������O�Ɉړ�
        targetPosition = startPosition + transform.forward * speed * moveDuration;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= moveDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
