using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haptics : MonoBehaviour
{
    public AudioSource audiosource;
    private OVRHapticsClip hapticsClip;

    void Start()
    {
        hapticsClip = new OVRHapticsClip(audiosource.clip);
    }

    public void TriggerHaptics()
    {
        StartCoroutine(VibrateController(0.5f, 1f, 1f)); // 0.5ïbä‘êUìÆ

        if (hapticsClip != null)
        {
            OVRHaptics.RightChannel.Mix(hapticsClip);
        }

        if (audiosource != null)
        {
            audiosource.Play();
        }
    }

    private IEnumerator VibrateController(float duration, float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
