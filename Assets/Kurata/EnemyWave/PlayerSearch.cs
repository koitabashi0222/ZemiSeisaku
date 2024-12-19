using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    public enum FollowMode
    {
        Straight, // �����Ǐ]
        ZigZag    // �W�O�U�O�Ǐ]
    }

    public FollowMode followMode = FollowMode.Straight; // ���݂̒Ǐ]���[�h
    public float speed = 3f;                           // �ړ����x

    // �W�O�U�O�p�p�����[�^
    public float waveFrequency = 2f;                  // ���g��

    public List<Vector3> bounceDirections;            // �e�i�K���Ƃ̐܂�Ԃ�����
    public List<int> waveSteps;                       // �e�����̒i�K�� (�W�O�U�O��)
    public List<float> moveDistances;                 // �e�����̈ړ�����

    private float waveTimer = 0f;                     // �W�O�U�O�p�̃^�C�}�[
    private int currentStep = 0;                      // ���݂̒i�K��
    private int currentDirectionIndex = 0;            // ���݂̐܂�Ԃ������̃C���f�b�N�X
    private int remainingSteps;                       // ���݂̕����Ŏc���Ă���i�K��
    private float currentAmplitude;                  // ���݂̈ړ�����

    void Start()
    {
        // ������: �i�K���ƐU�ꕝ��ݒ�
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
                // ���[�h�ɉ����ď�����؂�ւ�
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

    // �����Ǐ]�̏���
    private void StraightFollow(Transform targetParent, Transform thisParent)
    {
        Vector3 targetPosition = targetParent.position;
        thisParent.position = Vector3.MoveTowards(thisParent.position, targetPosition, speed * Time.deltaTime);

        thisParent.LookAt(targetParent);
    }

    // �W�O�U�O�Ǐ]�̏���
    private void ZigZagFollow(Transform targetParent, Transform thisParent)
    {
        if (bounceDirections.Count == 0 || waveSteps.Count == 0 || moveDistances.Count == 0) return;

        // ���݂̕����Ǝc��̒i�K���Ɋ�Â��ď�����i�߂�
        Vector3 currentBounceDirection = bounceDirections[currentDirectionIndex % bounceDirections.Count].normalized;

        // ���݂̈ړ���������ɂ����U�ꕝ��ݒ�
        float amplitude = currentAmplitude;

        // �W�O�U�O�̃^�C�}�[��i�߂�
        waveTimer += Time.deltaTime * waveFrequency;

        // �v���C���[�ւ̒����������v�Z
        Vector3 direction = (targetParent.position - thisParent.position).normalized;

        // �����������v�Z
        Vector3 perpendicular = Vector3.Cross(direction, currentBounceDirection);

        // �W�O�U�O�̃I�t�Z�b�g���v�Z
        Vector3 waveOffset = perpendicular * Mathf.Sin(waveTimer) * amplitude;

        // �ŏI�ڕW�n�_
        Vector3 targetPosition = targetParent.position + waveOffset;

        // �ړ�
        thisParent.position = Vector3.MoveTowards(thisParent.position, targetPosition, speed * Time.deltaTime);

        thisParent.LookAt(targetParent);

        // �i�K���̊Ǘ�
        if (waveTimer >= Mathf.PI * 2)
        {
            waveTimer = 0f; // �^�C�}�[�����Z�b�g
            remainingSteps--; // �c��̒i�K�������炷

            if (remainingSteps <= 0)
            {
                // ���̕����ɐ؂�ւ�
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
                    currentAmplitude = moveDistances[moveDistances.Count - 1]; // �J��Ԃ������̏ꍇ
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�v���C���[������͈͂𗣂ꂽ");
        }
    }
}
