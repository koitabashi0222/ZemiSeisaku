using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public int maxHits = 3; // 最大ヒット回数
    private int currentHits = 0; // 現在のヒット回数
    public AudioClip damageSound; // ダメージを受けたときのSE
    public AudioSource audioSource; // 使用する AudioSource を指定
    public string bulletLayerName = "Bullet"; // 判定するレイヤー名
    public Text hitsText; // ヒット数を表示するUI Text

    private int bulletLayer; // 判定するレイヤーのID

    private void Start()
    {
        // 判定するレイヤーのIDを取得
        bulletLayer = LayerMask.NameToLayer(bulletLayerName);
        if (bulletLayer == -1)
        {
            Debug.LogError($"Layer '{bulletLayerName}' が存在しません。プロジェクト設定でレイヤーを確認してください。");
        }

        // 初期状態でヒット数を更新
        UpdateHitsUI();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Over");
    }

    private void OnTriggerEnter(Collider other)
    {
        // attack タグ、または指定レイヤーのオブジェクトにヒットした場合
        if (other.CompareTag("attack") || other.gameObject.layer == bulletLayer)
        {
            currentHits++; // ヒット回数をカウント
            Debug.Log($"Hit count: {currentHits}");

            // SEを再生
            PlayDamageSound();

            // ヒット数をUIに更新
            UpdateHitsUI();

            // ヒット回数が maxHits に達したらゲームオーバー
            if (currentHits >= maxHits)
            {
                GameOver();
            }
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound); // SE を再生
        }
        else
        {
            Debug.LogWarning("ダメージ効果音が設定されていないか、AudioSource がありません。");
        }
    }

    private void UpdateHitsUI()
    {
        if (hitsText != null)
        {
            hitsText.text = $"Hits: {currentHits}/{maxHits}"; // ヒット数を更新
        }
        else
        {
            Debug.LogWarning("UI Text が設定されていません。");
        }
    }
}
