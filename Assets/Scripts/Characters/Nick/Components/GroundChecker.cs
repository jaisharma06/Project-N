using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask groundLayer;
    public Vector2 capsuleSize;
    public bool pIsGrounded { get; private set; }
    
    private void Start()
    {
        pIsGrounded = true;
        SetCapsuleSize();
    }

    private void Update()
    {
        IsGrounded();
    }

    private void SetCapsuleSize()
    {
        var spriteRenderer = GetComponentInChildren<Renderer>();
        capsuleSize = spriteRenderer.bounds.size;
        capsuleSize.y += 0.01f;
    }

    private bool IsGrounded()
    {
        pIsGrounded =
            Physics2D.OverlapCapsule(transform.position, capsuleSize, CapsuleDirection2D.Vertical, 0, groundLayer);

        return pIsGrounded;
    }
}