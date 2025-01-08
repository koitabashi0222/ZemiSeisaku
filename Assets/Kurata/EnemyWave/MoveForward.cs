using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5f;  // �ړ����x
    public float moveDuration = 3f; // �ړ����s�����ԁi�b�j

    private float timer = 0f; // �o�ߎ��Ԃ̃^�C�}�[

    void Update()
    {
        // �^�C�}�[���X�V
        timer += Time.deltaTime;

        // �w�肵�����Ԃ��߂�����ړ����~
        if (timer <= moveDuration)
        {
            // �I�u�W�F�N�g��O�Ɉړ�������iZ�������j
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
