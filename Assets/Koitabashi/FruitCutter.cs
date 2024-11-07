using UnityEngine;
using BLINDED_AM_ME;
using System.Collections.Generic;
using System.Collections;
public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;       // �ؒf�ʂ������v���[��
    public Material capMaterialCenter;    // ���S�ɋ߂������̐ؒf�ʂɎg�p����}�e���A��
    public Material capMaterialEdge;      // �[�ɋ߂������̐ؒf�ʂɎg�p����}�e���A��
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // �ؒf�ʂ͈̔�
    public string targetTag = "Cuttable"; // �ؒf�\�ȃI�u�W�F�N�g�̃^�O
    public float distanceThreshold = 0.5f; // ���S����ǂꂭ�炢�̋����Ń}�e���A����ς��邩
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // �ؒf�ς݂̃I�u�W�F�N�g���Ǘ�
    private void OnTriggerEnter(Collider other)
    {
        // �I�u�W�F�N�g�� "Cuttable" �^�O�������Ă��āA���܂��ؒf����Ă��Ȃ����m�F
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // �������Ă���ΐؒf
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // �ؒf�ς݂Ƃ��ēo�^
                }
                else
                {
                    Debug.Log("�I�u�W�F�N�g���ؒf�ʂɐG��Ă��܂���B");
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

        GameObject[] pieces = MeshCut.Cut(target, anchorPoint, normalDirection, selectedCapMaterial);
        if (pieces != null)
        {
            foreach (GameObject piece in pieces)
            {
                Rigidbody rb = piece.AddComponent<Rigidbody>();
                rb.mass = 1;
                rb.AddForce(Vector3.up * Random.Range(1f, 3f), ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);

                StartCoroutine(HideAfterDelay(piece, 5f)); // 5�b��ɔ�\��
                alreadyCutObjects.Add(piece);
            }
        }
    }

    IEnumerator HideAfterDelay(GameObject piece, float delay)
    {
        yield return new WaitForSeconds(delay);
        piece.SetActive(false); // ��\���ɂ���
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



/*using UnityEngine;
using System.Collections.Generic;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;       // �ؒf�ʂ������v���[��
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // �ؒf�ʂ͈̔�
    public string targetTag = "Cuttable"; // �ؒf�\�ȃI�u�W�F�N�g�̃^�O
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // �ؒf�ς݂̃I�u�W�F�N�g���Ǘ�

    private void OnTriggerEnter(Collider other)
    {
        // �I�u�W�F�N�g�� "Cuttable" �^�O�������Ă��āA���܂��ؒf����Ă��Ȃ����m�F
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // �������Ă���ΐؒf
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // �ؒf�ς݂Ƃ��ēo�^
                }
                else
                {
                    Debug.Log("�I�u�W�F�N�g���ؒf�ʂɐG��Ă��܂���B");
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
                // �I���W�i���̃}�e���A����V���������ɓK�p
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
                alreadyCutObjects.Add(piece); // �V���ɐ������ꂽ�s�[�X���ؒf�ς݂Ƃ��ēo�^
            }

            // �I���W�i���̃I�u�W�F�N�g��j��
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
