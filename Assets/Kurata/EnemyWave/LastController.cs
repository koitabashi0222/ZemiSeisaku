using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastController : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public GameObject[] targetObjects; // 監視するオブジェクト（2つ設定する）
    public float moveSpeed = 5f; // 移動速度

    private bool shouldMoveToPlayer = false; // プレイヤーに向かって移動するかどうか

    void Update()
    {
        // 監視するオブジェクトがすべて破壊されているかチェック
        if (!shouldMoveToPlayer && AreAllTargetsDestroyed())
        {
            shouldMoveToPlayer = true; // プレイヤーに向かって移動開始
        }

        // プレイヤーに向かって移動
        if (shouldMoveToPlayer && player != null)
        {
            MoveTowardsPlayer();
        }
    }

    // 監視するオブジェクトがすべて破壊されているか判定
    private bool AreAllTargetsDestroyed()
    {
        foreach (GameObject target in targetObjects)
        {
            if (target != null) return false; // オブジェクトが存在する場合は移動しない
        }
        return true; // すべて破壊されていたらtrue
    }

    // プレイヤーに向かって移動する
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized; // プレイヤーへの方向
        transform.position += direction * moveSpeed * Time.deltaTime; // プレイヤーへ移動
    }
}
