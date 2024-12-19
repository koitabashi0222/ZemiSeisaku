using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    public enum FollowMode
    {
        Straight, // 直線追従
        ZigZag    // ジグザグ追従
    }

    public FollowMode followMode = FollowMode.Straight; // 現在の追従モード
    public float speed = 3f;                           // 移動速度

    // ジグザグ用パラメータ
    public float waveFrequency = 2f;                  // 周波数

    public List<Vector3> bounceDirections;            // 各段階ごとの折り返し方向
    public List<int> waveSteps;                       // 各方向の段階数 (ジグザグ回数)
    public List<float> moveDistances;                 // 各方向の移動距離

    private float waveTimer = 0f;                     // ジグザグ用のタイマー
    private int currentStep = 0;                      // 現在の段階数
    private int currentDirectionIndex = 0;            // 現在の折り返し方向のインデックス
    private int remainingSteps;                       // 現在の方向で残っている段階数
    private float currentAmplitude;                  // 現在の移動距離

    void Start()
    {
        // 初期化: 段階数と振れ幅を設定
        if (waveSteps.Count > 0 && moveDistances.Count > 0)
        {
            remainingSteps = waveSteps[0];
            currentAmplitude = moveDistances[0];
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform targetParent = other.transform.parent;
            Transform thisParent = this.transform.parent;

            if (targetParent != null && thisParent != null)
            {
                // モードに応じて処理を切り替え
                switch (followMode)
                {
                    case FollowMode.Straight:
                        StraightFollow(targetParent, thisParent);
                        break;

                    case FollowMode.ZigZag:
                        ZigZagFollow(targetParent, thisParent);
                        break;
                }
            }
        }
    }

    // 直線追従の処理
    private void StraightFollow(Transform targetParent, Transform thisParent)
    {
        Vector3 targetPosition = targetParent.position;
        thisParent.position = Vector3.MoveTowards(thisParent.position, targetPosition, speed * Time.deltaTime);

        thisParent.LookAt(targetParent);
    }

    // ジグザグ追従の処理
    private void ZigZagFollow(Transform targetParent, Transform thisParent)
    {
        if (bounceDirections.Count == 0 || waveSteps.Count == 0 || moveDistances.Count == 0) return;

        // 現在の方向と残りの段階数に基づいて処理を進める
        Vector3 currentBounceDirection = bounceDirections[currentDirectionIndex % bounceDirections.Count].normalized;

        // 現在の移動距離を基にした振れ幅を設定
        float amplitude = currentAmplitude;

        // ジグザグのタイマーを進める
        waveTimer += Time.deltaTime * waveFrequency;

        // プレイヤーへの直線方向を計算
        Vector3 direction = (targetParent.position - thisParent.position).normalized;

        // 垂直方向を計算
        Vector3 perpendicular = Vector3.Cross(direction, currentBounceDirection);

        // ジグザグのオフセットを計算
        Vector3 waveOffset = perpendicular * Mathf.Sin(waveTimer) * amplitude;

        // 最終目標地点
        Vector3 targetPosition = targetParent.position + waveOffset;

        // 移動
        thisParent.position = Vector3.MoveTowards(thisParent.position, targetPosition, speed * Time.deltaTime);

        thisParent.LookAt(targetParent);

        // 段階数の管理
        if (waveTimer >= Mathf.PI * 2)
        {
            waveTimer = 0f; // タイマーをリセット
            remainingSteps--; // 残りの段階数を減らす

            if (remainingSteps <= 0)
            {
                // 次の方向に切り替え
                currentDirectionIndex++;
                currentStep++;
                if (currentStep < waveSteps.Count && currentStep < moveDistances.Count)
                {
                    remainingSteps = waveSteps[currentStep];
                    currentAmplitude = moveDistances[currentStep];
                }
                else
                {
                    remainingSteps = waveSteps[waveSteps.Count - 1];
                    currentAmplitude = moveDistances[moveDistances.Count - 1]; // 繰り返し処理の場合
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが判定範囲を離れた");
        }
    }
}
