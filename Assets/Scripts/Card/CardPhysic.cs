using UnityEngine;

public class CardPhysic : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D cardCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cardCollider = GetComponent<Collider2D>();
    }
    public void SetPhysicsActive(bool active)
    {
        rb.simulated = active;
        cardCollider.enabled = active;
    }
}
