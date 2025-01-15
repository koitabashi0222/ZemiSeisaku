using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5f;
    public float moveDuration = 3f; // 移動を行う時間

    private float timer = 0f; // 経過時間のタイマー
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;

        //指定時間分だけ前に移動
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
