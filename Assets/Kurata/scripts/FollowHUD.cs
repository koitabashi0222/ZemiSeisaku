using UnityEngine;

namespace raspberly.ovr
{
    public class FollowHUD : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target; // HUDが追従する対象
        [Header("Follow Settings")]
        [SerializeField] private float followMoveSpeed = 0.1f; // 追従する移動速度
        [SerializeField] private float followRotateSpeed = 0.02f; // 回転の追従速度
        [SerializeField] private float rotateSpeedThreshold = 0.9f; // 回転速度の閾値
        [SerializeField] private bool isImmediateMove = false; // 即座に追従するか
        [Header("Axis Lock")]
        [SerializeField] private bool isLockX = false; // 回転のX軸をロック
        [SerializeField] private bool isLockY = false; // 回転のY軸をロック
        [SerializeField] private bool isLockZ = false; // 回転のZ軸をロック

        private Quaternion targetRotation;
        private Quaternion rotationDifference;

        private void Start()
        {
            // 追従対象が設定されていなければメインカメラをデフォルトで使用
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
            // 位置の追従
            if (isImmediateMove)
            {
                transform.position = target.position;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.position, followMoveSpeed);
            }

            // 回転の追従
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            // 目標の回転を計算
            rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
            targetRotation = target.rotation;

            // 指定された軸をロック
            if (isLockX) targetRotation.x = 0;
            if (isLockY) targetRotation.y = 0;
            if (isLockZ) targetRotation.z = 0;

            // 回転速度を調整して追従
            float rotationSpeed = (rotationDifference.w < rotateSpeedThreshold) ? followRotateSpeed * 4 : followRotateSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
        }

        /// <summary>
        /// 指定したTransformに即座に同期
        /// </summary>
        /// <param name="targetTransform">同期対象のTransform</param>
        public void ImmediateSync(Transform targetTransform)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }
}