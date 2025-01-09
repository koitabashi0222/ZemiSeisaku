using System.Collections;
using UnityEngine;

public class Haptics : MonoBehaviour
{
    public AudioSource audioSource;
    private OVRHapticsClip hapticsClipRight;
    private OVRHapticsClip hapticsClipLeft;

    void Start()
    {
        // オーディオクリップを使用してハプティクスクリップを作成
        if (audioSource != null && audioSource.clip != null)
        {
            hapticsClipRight = new OVRHapticsClip(audioSource.clip);
            hapticsClipLeft = new OVRHapticsClip(audioSource.clip);
        }
    }

    /// <summary>
    /// 右手コントローラーの振動をトリガーします
    /// </summary>
    public void TriggerHaptics()
    {
        StartCoroutine(VibrateController(OVRInput.Controller.RTouch, 0.5f, 1f, 1f)); // 0.5秒間振動

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
    /// 左手コントローラーの振動をトリガーします
    /// </summary>
    public void TriggerHapticsLeft()
    {
        StartCoroutine(VibrateController(OVRInput.Controller.LTouch, 0.5f, 1f, 1f)); // 0.5秒間振動

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
    /// 指定したコントローラーで振動を開始します
    /// </summary>
    private IEnumerator VibrateController(OVRInput.Controller controller, float duration, float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
