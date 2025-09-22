using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float m_Time = 3f;

    private void Start()
    {
        Destroy(gameObject, m_Time);
    }
}
