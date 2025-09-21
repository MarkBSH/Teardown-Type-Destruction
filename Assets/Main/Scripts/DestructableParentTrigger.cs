using UnityEngine;

public class DestructableParentTrigger : MonoBehaviour
{
    private bool m_HasChecked = false;

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited by: " + other.gameObject.name);
        if (other.CompareTag("Destructing Object") && !m_HasChecked)
        {
            DestructableParent parent = transform.parent.GetComponent<DestructableParent>();
            parent.ConnectionCheck();
            m_HasChecked = true;
        }
    }
}
