using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int maxHits = 3; // �ő�q�b�g��
    [SerializeField] private string bulletLayerName = "Bullet"; // ���肷�郌�C���[��

    [Header("Audio Settings")]
    [SerializeField] private AudioClip damageSound; // �_���[�W��
    [SerializeField] private AudioSource audioSource;

    [Header("UI Settings")]
    [SerializeField] private Text hitsText; // �q�b�g����\������UI

    private int currentHits = 0; // ���݂̃q�b�g��
    private int bulletLayer; // ���肷�郌�C���[ID

    private void Start()
    {
        bulletLayer = LayerMask.NameToLayer(bulletLayerName);
        if (bulletLayer == -1)
        {
            Debug.LogError($"Layer '{bulletLayerName}' ��������܂���B�v���W�F�N�g�̃��C���[�ݒ���m�F���Ă��������B");
        }

        // �������F�q�b�g����UI�X�V
        UpdateHitsUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �q�b�g����: attack�^�O�A�܂��͎w�背�C���[
        if (other.CompareTag("attack") || other.gameObject.layer == bulletLayer)
        {
            currentHits++;
            Debug.Log($"[GameOverManager] ���݂̃q�b�g��: {currentHits}");

            // �_���[�WSE�Đ�
            PlayDamageSound();

            // UI�X�V
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
            Debug.LogWarning("AudioSource�܂��̓_���[�W�����ݒ肳��Ă��܂���B");
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
            Debug.LogWarning("�q�b�g����\������UI Text���ݒ肳��Ă��܂���B");
        }
    }

    private void GameOver()
    {
        Debug.Log("[GameOverManager] �Q�[���I�[�o�[����");
        SceneManager.LoadScene("Over");
    }
}