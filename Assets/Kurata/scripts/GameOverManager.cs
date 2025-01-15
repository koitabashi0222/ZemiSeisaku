using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int maxHits = 3; // 最大ヒット回数
    [SerializeField] private string bulletLayerName = "Bullet"; // 判定するレイヤー名

    [Header("Audio Settings")]
    [SerializeField] private AudioClip damageSound; // ダメージ音
    [SerializeField] private AudioSource audioSource;

    [Header("UI Settings")]
    [SerializeField] private Text hitsText; // ヒット数を表示するUI

    private int currentHits = 0; // 現在のヒット回数
    private int bulletLayer; // 判定するレイヤーID

    private void Start()
    {
        bulletLayer = LayerMask.NameToLayer(bulletLayerName);
        if (bulletLayer == -1)
        {
            Debug.LogError($"Layer '{bulletLayerName}' が見つかりません。プロジェクトのレイヤー設定を確認してください。");
        }

        // 初期化：ヒット数のUI更新
        UpdateHitsUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ヒット判定: attackタグ、または指定レイヤー
        if (other.CompareTag("attack") || other.gameObject.layer == bulletLayer)
        {
            currentHits++;
            Debug.Log($"[GameOverManager] 現在のヒット数: {currentHits}");

            // ダメージSE再生
            PlayDamageSound();

            // UI更新
            UpdateHitsUI();

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
            audioSource.PlayOneShot(damageSound);
        }
        else
        {
            Debug.LogWarning("AudioSourceまたはダメージ音が設定されていません。");
        }
    }

    private void UpdateHitsUI()
    {
        if (hitsText != null)
        {
            hitsText.text = $"Hits: {currentHits} / {maxHits}";
        }
        else
        {
            Debug.LogWarning("ヒット数を表示するUI Textが設定されていません。");
        }
    }

    private void GameOver()
    {
        Debug.Log("[GameOverManager] ゲームオーバー発生");
        SceneManager.LoadScene("Over");
    }
}