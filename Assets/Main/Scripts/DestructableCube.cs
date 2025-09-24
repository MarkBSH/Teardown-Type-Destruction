using UnityEngine;

public class DestructableCube : MonoBehaviour
{
    private GameObject m_ParentObject;

    public float m_Strength = 10f;
    public float m_Mass = 1f;
    public float m_DespawnTime = 5f;

    public void DetachAndDespawn(GameObject destructedBy, float force)
    {
        m_ParentObject = transform.parent.gameObject;

        if (m_ParentObject != null)
        {
            m_ParentObject.GetComponent<DestructableParent>().m_ConnectedCubes.Remove(gameObject);
            m_ParentObject.GetComponent<DestructableParent>().m_ConnectedCubeNames.Remove(gameObject.name);
            transform.SetParent(null);
        }
        gameObject.tag = "Disconnected Cube";
        m_Strength = 1f;
        BoxCollider box = gameObject.GetComponent<BoxCollider>();
        box.excludeLayers = LayerMask.GetMask("Destructing Object");
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = m_Mass;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(destructedBy.transform.forward * force - destructedBy.transform.up * m_Strength, ForceMode.Impulse);
        Destroy(gameObject, m_DespawnTime);
    }
}
