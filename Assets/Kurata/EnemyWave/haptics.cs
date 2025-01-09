using System.Collections;
using UnityEngine;

public class Haptics : MonoBehaviour
{
    public AudioSource audioSource;
    private OVRHapticsClip hapticsClipRight;
    private OVRHapticsClip hapticsClipLeft;

    void Start()
    {
        // �I�[�f�B�I�N���b�v���g�p���ăn�v�e�B�N�X�N���b�v���쐬
        if (audioSource != null && audioSource.clip != null)
        {
            hapticsClipRight = new OVRHapticsClip(audioSource.clip);
            hapticsClipLeft = new OVRHapticsClip(audioSource.clip);
        }
    }

    /// <summary>
    /// �E��R���g���[���[�̐U�����g���K�[���܂�
    /// </summary>
    public void TriggerHaptics()
    {
        StartCoroutine(VibrateController(OVRInput.Controller.RTouch, 0.5f, 1f, 1f)); // 0.5�b�ԐU��

        if (hapticsClipRight != null)
        {
            OVRHaptics.RightChannel.Mix(hapticsClipRight);
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// ����R���g���[���[�̐U�����g���K�[���܂�
    /// </summary>
    public void TriggerHapticsLeft()
    {
        StartCoroutine(VibrateController(OVRInput.Controller.LTouch, 0.5f, 1f, 1f)); // 0.5�b�ԐU��

        if (hapticsClipLeft != null)
        {
            OVRHaptics.LeftChannel.Mix(hapticsClipLeft);
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// �w�肵���R���g���[���[�ŐU�����J�n���܂�
    /// </summary>
    private IEnumerator VibrateController(OVRInput.Controller controller, float duration, float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
