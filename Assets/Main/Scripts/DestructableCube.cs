using System.Collections;
using UnityEngine;

public class DestructableCube : MonoBehaviour
{
    private GameObject m_ParentObject;
    public GameObject m_ConnectedCubeCheckerUp;
    public GameObject m_ConnectedCubeCheckerDown;
    public GameObject m_ConnectedCubeCheckerLeft;
    public GameObject m_ConnectedCubeCheckerRight;
    public GameObject m_ConnectedCubeCheckerForward;
    public GameObject m_ConnectedCubeCheckerBack;

    public float m_Strength = 10f;
    public float m_Mass = 1f;
    public float m_DespawnTime = 5f;

    private void Start()
    {
        m_ParentObject = transform.parent.gameObject;
    }

    public IEnumerator CheckConnectedSides()
    {
        transform.parent.GetComponent<DestructableParent>().AddPartCube(gameObject);

        m_ConnectedCubeCheckerUp.transform.localPosition = Vector3.up;
        m_ConnectedCubeCheckerDown.transform.localPosition = Vector3.down;
        m_ConnectedCubeCheckerLeft.transform.localPosition = Vector3.left;
        m_ConnectedCubeCheckerRight.transform.localPosition = Vector3.right;
        m_ConnectedCubeCheckerForward.transform.localPosition = Vector3.forward;
        m_ConnectedCubeCheckerBack.transform.localPosition = Vector3.back;

        yield return null;

        m_ConnectedCubeCheckerUp.transform.localPosition = Vector3.zero;
        m_ConnectedCubeCheckerDown.transform.localPosition = Vector3.zero;
        m_ConnectedCubeCheckerLeft.transform.localPosition = Vector3.zero;
        m_ConnectedCubeCheckerRight.transform.localPosition = Vector3.zero;
        m_ConnectedCubeCheckerForward.transform.localPosition = Vector3.zero;
        m_ConnectedCubeCheckerBack.transform.localPosition = Vector3.zero;

        if (m_ConnectedCubeCheckerUp.GetComponent<ConnectedCubeChecker>().m_ConnectedCube != null)
        {
            DestructableCube connectedCube = m_ConnectedCubeCheckerUp.GetComponent<ConnectedCubeChecker>().m_ConnectedCube.GetComponent<DestructableCube>();
            if (!transform.parent.GetComponent<DestructableParent>().m_ConnectedPartsList.Contains(m_ConnectedCubeCheckerUp.GetComponent<ConnectedCubeChecker>().m_ConnectedCube))
            {
                StartCoroutine(connectedCube.CheckConnectedSides());
            }
        }

        if (m_ConnectedCubeCheckerDown.GetComponent<ConnectedCubeChecker>().m_ConnectedCube != null)
        {
            DestructableCube connectedCube = m_ConnectedCubeCheckerDown.GetComponent<ConnectedCubeChecker>().m_ConnectedCube.GetComponent<DestructableCube>();
            if (!transform.parent.GetComponent<DestructableParent>().m_ConnectedPartsList.Contains(m_ConnectedCubeCheckerDown.GetComponent<ConnectedCubeChecker>().m_ConnectedCube))
            {
                StartCoroutine(connectedCube.CheckConnectedSides());
            }
        }

        if (m_ConnectedCubeCheckerLeft.GetComponent<ConnectedCubeChecker>().m_ConnectedCube != null)
        {
            DestructableCube connectedCube = m_ConnectedCubeCheckerLeft.GetComponent<ConnectedCubeChecker>().m_ConnectedCube.GetComponent<DestructableCube>();
            if (!transform.parent.GetComponent<DestructableParent>().m_ConnectedPartsList.Contains(m_ConnectedCubeCheckerLeft.GetComponent<ConnectedCubeChecker>().m_ConnectedCube))
            {
                StartCoroutine(connectedCube.CheckConnectedSides());
            }
        }

        if (m_ConnectedCubeCheckerRight.GetComponent<ConnectedCubeChecker>().m_ConnectedCube != null)
        {
            DestructableCube connectedCube = m_ConnectedCubeCheckerRight.GetComponent<ConnectedCubeChecker>().m_ConnectedCube.GetComponent<DestructableCube>();
            if (!transform.parent.GetComponent<DestructableParent>().m_ConnectedPartsList.Contains(m_ConnectedCubeCheckerRight.GetComponent<ConnectedCubeChecker>().m_ConnectedCube))
            {
                StartCoroutine(connectedCube.CheckConnectedSides());
            }
        }

        if (m_ConnectedCubeCheckerForward.GetComponent<ConnectedCubeChecker>().m_ConnectedCube != null)
        {
            DestructableCube connectedCube = m_ConnectedCubeCheckerForward.GetComponent<ConnectedCubeChecker>().m_ConnectedCube.GetComponent<DestructableCube>();
            if (!transform.parent.GetComponent<DestructableParent>().m_ConnectedPartsList.Contains(m_ConnectedCubeCheckerForward.GetComponent<ConnectedCubeChecker>().m_ConnectedCube))
            {
                StartCoroutine(connectedCube.CheckConnectedSides());
            }
        }

        if (m_ConnectedCubeCheckerBack.GetComponent<ConnectedCubeChecker>().m_ConnectedCube != null)
        {
            DestructableCube connectedCube = m_ConnectedCubeCheckerBack.GetComponent<ConnectedCubeChecker>().m_ConnectedCube.GetComponent<DestructableCube>();
            if (!transform.parent.GetComponent<DestructableParent>().m_ConnectedPartsList.Contains(m_ConnectedCubeCheckerBack.GetComponent<ConnectedCubeChecker>().m_ConnectedCube))
            {
                StartCoroutine(connectedCube.CheckConnectedSides());
            }
        }

        m_ConnectedCubeCheckerUp.GetComponent<ConnectedCubeChecker>().m_ConnectedCube = null;
        m_ConnectedCubeCheckerDown.GetComponent<ConnectedCubeChecker>().m_ConnectedCube = null;
        m_ConnectedCubeCheckerLeft.GetComponent<ConnectedCubeChecker>().m_ConnectedCube = null;
        m_ConnectedCubeCheckerRight.GetComponent<ConnectedCubeChecker>().m_ConnectedCube = null;
        m_ConnectedCubeCheckerForward.GetComponent<ConnectedCubeChecker>().m_ConnectedCube = null;
        m_ConnectedCubeCheckerBack.GetComponent<ConnectedCubeChecker>().m_ConnectedCube = null;
    }

    public void DetachAndDespawn(GameObject destructedBy, float force)
    {
        if (m_ParentObject != null)
        {
            m_ParentObject.GetComponent<DestructableParent>().m_ConnectedCubes.Remove(gameObject);
            Destroy(transform.GetChild(0).gameObject);
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
