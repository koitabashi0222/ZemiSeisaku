using UnityEngine;

namespace raspberly.ovr
{
    public class FollowHUD : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target; // HUD���Ǐ]����Ώ�
        [Header("Follow Settings")]
        [SerializeField] private float followMoveSpeed = 0.1f; // �Ǐ]����ړ����x
        [SerializeField] private float followRotateSpeed = 0.02f; // ��]�̒Ǐ]���x
        [SerializeField] private float rotateSpeedThreshold = 0.9f; // ��]���x��臒l
        [SerializeField] private bool isImmediateMove = false; // �����ɒǏ]���邩
        [Header("Axis Lock")]
        [SerializeField] private bool isLockX = false; // ��]��X�������b�N
        [SerializeField] private bool isLockY = false; // ��]��Y�������b�N
        [SerializeField] private bool isLockZ = false; // ��]��Z�������b�N

        private Quaternion targetRotation;
        private Quaternion rotationDifference;

        private void Start()
        {
            // �Ǐ]�Ώۂ��ݒ肳��Ă��Ȃ���΃��C���J�������f�t�H���g�Ŏg�p
            if (target == null)
            {
                if (Camera.main != null)
                {
                    target = Camera.main.transform;
                }
                else
                {
                    Debug.LogError("Target is not assigned", this);
                }
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;
            // �ʒu�̒Ǐ]
            if (isImmediateMove)
            {
                transform.position = target.position;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.position, followMoveSpeed);
            }

            // ��]�̒Ǐ]
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            // �ڕW�̉�]���v�Z
            rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
            targetRotation = target.rotation;

            // �w�肳�ꂽ�������b�N
            if (isLockX) targetRotation.x = 0;
            if (isLockY) targetRotation.y = 0;
            if (isLockZ) targetRotation.z = 0;

            // ��]���x�𒲐����ĒǏ]
            float rotationSpeed = (rotationDifference.w < rotateSpeedThreshold) ? followRotateSpeed * 4 : followRotateSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
        }

        /// <summary>
        /// �w�肵��Transform�ɑ����ɓ���
        /// </summary>
        /// <param name="targetTransform">�����Ώۂ�Transform</param>
        public void ImmediateSync(Transform targetTransform)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }
}