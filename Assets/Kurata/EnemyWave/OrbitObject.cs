using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public float radius = 5f; // 半円の半径
    public float speed = 1f; // 回転速度

    private float angle = 0f; // 現在の角度

    void Update()
    {
        if (player == null) return;

        // 角度を更新
        angle += speed * Time.deltaTime;

        // 半円の軌道を計算 (0〜180度)
        float radian = Mathf.Deg2Rad * Mathf.Clamp(angle % 360, 0, 180);

        // 半円の座標を計算
        float x = player.position.x + Mathf.Cos(radian) * radius;
        float z = player.position.z + Mathf.Sin(radian) * radius;

        // オブジェクトの位置を更新 (高さはプレイヤーに合わせる)
        transform.position = new Vector3(x, player.position.y, z);

        // プレイヤーの方向を向く
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // 水平方向のみを向く
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
}
