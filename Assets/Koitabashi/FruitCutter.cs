using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BLINDED_AM_ME;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;
    public Material capMaterialCenter;
    public Material capMaterialEdge;
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);
    public string targetTag = "Cuttable";
    public float distanceThreshold = 0.5f;
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>();

    public float forceAmount = 10f;  // ������΂��͂̑傫��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                // �E��E��E��E� cuttingBounds �E��E��E�X�E�V
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                Bounds targetBounds = targetRenderer.bounds;

                // �E�͈͂��E�m�E�F�E��E��E�Đؒf�E��E��E��E��E��E��E��E��E�s
                if (cuttingBounds.Intersects(targetBounds))
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // �E�ؒf�E�ς݂Ƃ��E�ēo�E�^
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
        Material selectedCapMaterial = (distanceFromCenter < distanceThreshold) ? capMaterialCenter : capMaterialEdge;

        // MeshCutNeo �E��E� CutMesh �E��E��E�\�E�b�E�h�E��E��E�g�E�p�E��E��E�ăI�E�u�E�W�E�F�E�N�E�g�E��E�ؒf
        (GameObject pieceA, GameObject pieceB) = MeshCutNeo.CutMesh(target, anchorPoint, normalDirection, true, selectedCapMaterial);

        if (pieceA != null && pieceB != null)
        {
            // �E�V�E��E��E�ɐ��E��E��E��E��E�ꂽ�E�s�E�[�E�X�E��E�alreadyCutObjects�E�ɒǉ�
            alreadyCutObjects.Add(pieceA);
            alreadyCutObjects.Add(pieceB);

            foreach (GameObject piece in new GameObject[] { pieceA, pieceB })
            {
                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody �E�̂ݒǉ�
                rb.mass = 1;
                Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;  // �����𐳋K��
                rb.AddForce(knockbackDirection * forceAmount, ForceMode.Impulse);
                StartCoroutine(HideAfterDelay(piece, rb, 5f));
                alreadyCutObjects.Add(piece);
            }
        }
    }

    IEnumerator HideAfterDelay(GameObject piece, Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        piece.SetActive(false);
        alreadyCutObjects.Remove(piece); // �E��E��E��E��E��E��E�Ǘ��E�̂��E�ߍ폜
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









//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using BLINDED_AM_ME;

//public class FruitCutter : MonoBehaviour
//{
//    public GameObject cuttingPlane;
//    public Material capMaterialCenter;
//    public Material capMaterialEdge;
//    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);
//    public string targetTag = "Cuttable";
//    public float distanceThreshold = 0.5f;
//    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>();

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
//        {
//            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
//            if (targetRenderer != null)
//            {
//                // �E��E��E��E� cuttingBounds �E��E��E�X�E�V
//                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
//                Bounds targetBounds = targetRenderer.bounds;

//                // �E�͈͂��E�m�E�F�E��E��E�Đؒf�E��E��E��E��E��E��E��E��E�s
//                if (cuttingBounds.Intersects(targetBounds))
//                {
//                    PerformCut(other.gameObject);
//                    alreadyCutObjects.Add(other.gameObject);
//                }
//            }
//        }
//    }

//    void PerformCut(GameObject target)
//    {
//        Vector3 anchorPoint = cuttingPlane.transform.position;
//        Vector3 normalDirection = cuttingPlane.transform.up;

//        Vector3 targetCenter = target.GetComponent<Collider>().bounds.center;
//        float distanceFromCenter = Vector3.Distance(targetCenter, anchorPoint);
//        Material selectedCapMaterial = (distanceFromCenter < distanceThreshold) ? capMaterialCenter : capMaterialEdge;

//        GameObject[] pieces = MeshCut.Cut(target, anchorPoint, normalDirection, selectedCapMaterial);
//        if (pieces != null)
//        {
//            foreach (GameObject piece in pieces)
//            {
//                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody �E�̂ݒǉ�
//                rb.mass = 1;

//                StartCoroutine(HideAfterDelay(piece, 5f));
//                alreadyCutObjects.Add(piece);
//            }
//        }
//    }

//    IEnumerator HideAfterDelay(GameObject piece, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        piece.SetActive(false);
//        alreadyCutObjects.Remove(piece); // �E��E��E��E��E��E��E�Ǘ��E�̂��E�ߍ폜
//    }

//    void OnDrawGizmos()
//    {
//        if (cuttingPlane != null)
//        {
//            Vector3 anchorPoint = cuttingPlane.transform.position;
//            Gizmos.color = Color.black;
//            Gizmos.matrix = Matrix4x4.TRS(anchorPoint, cuttingPlane.transform.rotation, Vector3.one);
//            Gizmos.DrawWireCube(Vector3.zero, cuttingBoxSize);
//        }
//    }
//}






/*using UnityEngine;
using System.Collections.Generic;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;       // �E�ؒf�E�ʂ��E��E��E��E��E�v�E��E��E�[�E��E�
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // �E�ؒf�E�ʂ͈̔�
    public string targetTag = "Cuttable"; // �E�ؒf�E�\�E�ȃI�E�u�E�W�E�F�E�N�E�g�E�̃^�E�O
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // �E�ؒf�E�ς݂̃I�E�u�E�W�E�F�E�N�E�g�E��E��E�Ǘ�

    private void OnTriggerEnter(Collider other)
    {
        // �E�I�E�u�E�W�E�F�E�N�E�g�E��E� "Cuttable" �E�^�E�O�E��E��E��E��E��E��E�Ă��E�āA�E��E��E�܂��E�ؒf�E��E��E��E�Ă��E�Ȃ��E��E��E�m�E�F
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // �E��E��E��E��E��E��E�Ă��E��E�ΐؒf
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // �E�ؒf�E�ς݂Ƃ��E�ēo�E�^
                }
                else
                {
                    Debug.Log("�E�I�E�u�E�W�E�F�E�N�E�g�E��E��E�ؒf�E�ʂɐG�E��E�Ă��E�܂��E��E�B");
                }
            }
        }
    }

    void PerformCut(GameObject target)
    {
        Vector3 anchorPoint = cuttingPlane.transform.position;
        Vector3 normalDirection = cuttingPlane.transform.up;

        GameObject[] pieces = SimpleMeshCut.Cut(target, anchorPoint, normalDirection);
        if (pieces != null)
        {
            foreach (GameObject piece in pieces)
            {
                // �E�I�E��E��E�W�E�i�E��E��E�̃}�E�e�E��E��E�A�E��E��E��E�V�E��E��E��E��E��E��E��E��E�ɓK�E�p
                MeshRenderer originalRenderer = target.GetComponent<MeshRenderer>();
                MeshRenderer newRenderer = piece.GetComponent<MeshRenderer>();
                if (originalRenderer != null && newRenderer != null)
                {
                    newRenderer.materials = originalRenderer.materials;
                }

                Rigidbody rb = piece.AddComponent<Rigidbody>();
                rb.mass = 1;
                rb.AddForce(Vector3.up * Random.Range(1f, 3f), ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);
                alreadyCutObjects.Add(piece); // �E�V�E��E��E�ɐ��E��E��E��E��E�ꂽ�E�s�E�[�E�X�E��E��E�ؒf�E�ς݂Ƃ��E�ēo�E�^
            }

            // �E�I�E��E��E�W�E�i�E��E��E�̃I�E�u�E�W�E�F�E�N�E�g�E��E�j�E��E�
            Destroy(target);
        }
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
}*/
