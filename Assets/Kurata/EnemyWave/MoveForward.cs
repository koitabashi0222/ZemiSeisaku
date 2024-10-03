using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5f;  // 移動速度

    void Update()
    {
        // オブジェクトを前に移動させる（Z軸方向）
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
