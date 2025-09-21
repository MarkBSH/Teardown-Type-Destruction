using UnityEngine;

public class DestructingObject : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    public float m_Strength = 10f;
    public float m_Force;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.mass = m_Strength;
    }

    void Update()
    {
        m_Force = m_Rigidbody.linearVelocity.magnitude * m_Strength;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Connected Cube") || collision.gameObject.CompareTag("Disconnected Cube"))
        {
            DestructableCube destructedCube = collision.gameObject.GetComponent<DestructableCube>();
            if (m_Force > destructedCube.m_Strength)
            {
                destructedCube.DetachAndDespawn(gameObject, m_Force);
            }
        }
    }
}
