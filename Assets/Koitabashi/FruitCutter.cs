using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BLINDED_AM_ME;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;
    public Material capMaterialCenter; // 中心付近のマテリアル
    public Material capMaterialEdge;   // 通常のマテリアル
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);
    public string targetTag = "Cuttable";
    public float distanceThreshold = 0.5f; // 中心付近と判定する距離の閾値
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>();

    public Haptics hapticsController;
    private Effect effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                Bounds targetBounds = targetRenderer.bounds;

                if (cuttingBounds.Intersects(targetBounds))
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject);

                    // Haptics への通知
                    if (hapticsController != null)
                    {
                        hapticsController.TriggerHaptics(); // 振動をトリガー
                    }
                }
            }
        }
    }

    void PerformCut(GameObject target)
    {
        Vector3 anchorPoint = cuttingPlane.transform.position;
        Vector3 normalDirection = cuttingPlane.transform.up;

        Vector3 targetCenter = target.GetComponent<Collider>().bounds.center;
        float distanceFromCenter = Vector3.Distance(targetCenter, anchorPoint);

        Material selectedCapMaterial = capMaterialEdge; // デフォルトのマテリアル
        bool isNearCenter = distanceFromCenter < distanceThreshold;

        (GameObject pieceA, GameObject pieceB) = MeshCutNeo.CutMesh(target, anchorPoint, normalDirection, true, selectedCapMaterial);

        if (pieceA != null && pieceB != null)
        {
            alreadyCutObjects.Add(pieceA);
            alreadyCutObjects.Add(pieceB);

            // 中心付近なら中心マテリアルを追加適用
            if (isNearCenter)
            {
                ApplyCenterMaterial(pieceA);
                ApplyCenterMaterial(pieceB);
            }

            ApplyForceToPiece(pieceA, new Vector3(1, 1, 1));
            ApplyForceToPiece(pieceB, new Vector3(-1, 1, 1));

            // コライダーを無効化
            Collider colliderA = pieceA.GetComponent<Collider>();
            Collider colliderB = pieceB.GetComponent<Collider>();
            if (colliderA != null) colliderA.enabled = false;
            if (colliderB != null) colliderB.enabled = false;

            StartCoroutine(EnableColliderAfterDelay(colliderA, 0.5f));
            StartCoroutine(EnableColliderAfterDelay(colliderB, 0.5f));

            StartCoroutine(HideAfterDelay(pieceA, pieceA.GetComponent<Rigidbody>(), colliderA, 5f));
            StartCoroutine(HideAfterDelay(pieceB, pieceB.GetComponent<Rigidbody>(), colliderB, 5f));
        }
    }

    // 切断面に中心マテリアルを適用する処理
    void ApplyCenterMaterial(GameObject piece)
    {
        MeshRenderer meshRenderer = piece.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] currentMaterials = meshRenderer.sharedMaterials;
            List<Material> updatedMaterials = new List<Material>(currentMaterials);

            // 中心マテリアルを追加
            if (!updatedMaterials.Contains(capMaterialCenter))
            {
                updatedMaterials.Add(capMaterialCenter);
            }

            meshRenderer.sharedMaterials = updatedMaterials.ToArray();
        }
    }

    IEnumerator EnableColliderAfterDelay(Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider != null)
        {
            collider.enabled = true; // コライダーを再有効化
        }
    }

    void ApplyForceToPiece(GameObject piece, Vector3 direction)
    {
        foreach (Transform child in piece.transform)
        {
            // 子オブジェクトを削除
            Destroy(child.gameObject);
        }
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = 1;
        Vector3 forceDirection = direction.normalized;
        rb.AddForce(Vector3.up * Random.Range(1f, 3f), ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), ForceMode.Impulse);
    }

    IEnumerator HideAfterDelay(GameObject piece, Rigidbody rb, Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true; // コライダーを再有効化
        piece.SetActive(false); // 非表示にする
        alreadyCutObjects.Remove(piece);
    }

    void OnDrawGizmos()
    {
        if (cuttingPlane != null)
        {
            Vector3 anchorPoint = cuttingPlane.transform.position;
            Gizmos.color = Color.black;
            Gizmos.matrix = Matrix4x4.TRS(anchorPoint, cuttingPlane.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, cuttingBoxSize);
        }
    }
}
