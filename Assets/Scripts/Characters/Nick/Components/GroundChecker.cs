using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask groundLayer;
    public Vector2 characterSize;

    private Vector2 footPosition;
    private float circleRadius;

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
        characterSize = spriteRenderer.bounds.size;
        circleRadius = characterSize.x / 2.0f;
    }

    private bool IsGrounded()
    {
        footPosition = transform.position;
        footPosition.y -= characterSize.y / 2.0f - circleRadius + 0.2f;
        pIsGrounded = Physics2D.OverlapCircle(footPosition, circleRadius, groundLayer);

        return pIsGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(footPosition, circleRadius);
    }
}