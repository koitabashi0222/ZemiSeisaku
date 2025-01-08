using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public int maxHits = 3; // �ő�q�b�g��
    private int currentHits = 0; // ���݂̃q�b�g��
    public AudioClip damageSound; // �_���[�W���󂯂��Ƃ���SE
    public AudioSource audioSource; // �g�p���� AudioSource ���w��
    public string bulletLayerName = "Bullet"; // ���肷�郌�C���[��
    public Text hitsText; // �q�b�g����\������UI Text

    private int bulletLayer; // ���肷�郌�C���[��ID

    private void Start()
    {
        // ���肷�郌�C���[��ID���擾
        bulletLayer = LayerMask.NameToLayer(bulletLayerName);
        if (bulletLayer == -1)
        {
            Debug.LogError($"Layer '{bulletLayerName}' �����݂��܂���B�v���W�F�N�g�ݒ�Ń��C���[���m�F���Ă��������B");
        }

        // ������ԂŃq�b�g�����X�V
        UpdateHitsUI();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Over");
    }

    private void OnTriggerEnter(Collider other)
    {
        // attack �^�O�A�܂��͎w�背�C���[�̃I�u�W�F�N�g�Ƀq�b�g�����ꍇ
        if (other.CompareTag("attack") || other.gameObject.layer == bulletLayer)
        {
            currentHits++; // �q�b�g�񐔂��J�E���g
            Debug.Log($"Hit count: {currentHits}");

            // SE���Đ�
            PlayDamageSound();

            // �q�b�g����UI�ɍX�V
            UpdateHitsUI();

            // �q�b�g�񐔂� maxHits �ɒB������Q�[���I�[�o�[
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
            audioSource.PlayOneShot(damageSound); // SE ���Đ�
        }
        else
        {
            Debug.LogWarning("�_���[�W���ʉ����ݒ肳��Ă��Ȃ����AAudioSource ������܂���B");
        }
    }

    private void UpdateHitsUI()
    {
        if (hitsText != null)
        {
            hitsText.text = $"Hits: {currentHits}/{maxHits}"; // �q�b�g�����X�V
        }
        else
        {
            Debug.LogWarning("UI Text ���ݒ肳��Ă��܂���B");
        }
    }
}
