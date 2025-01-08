using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5f;  // 移動速度
    public float moveDuration = 3f; // 移動を行う時間（秒）

    private float timer = 0f; // 経過時間のタイマー

    void Update()
    {
        // タイマーを更新
        timer += Time.deltaTime;

        // 指定した時間を過ぎたら移動を停止
        if (timer <= moveDuration)
        {
            // オブジェクトを前に移動させる（Z軸方向）
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
