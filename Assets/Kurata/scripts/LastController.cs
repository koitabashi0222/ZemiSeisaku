using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastController : MonoBehaviour
{
    public Transform player;
    public GameObject[] targetObjects; // 監視するオブジェクト（2つ設定する）
    public float moveSpeed = 5f;

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

    private bool AreAllTargetsDestroyed()
    {
        foreach (GameObject target in targetObjects)
        {
            if (target != null && target.activeInHierarchy) return false; // オブジェクトが破壊または非アクティブでない場合は移動しない
        }
        return true; // すべて破壊されていたら
    }

    private void MoveTowardsPlayer()
    {
        // プレイヤーへの方向
        Vector3 direction = (player.position - transform.position).normalized;

        // 移動ベクトル
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}