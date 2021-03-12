using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask groundLayer;
    private Vector2 _characterSize;

    private Vector2 footPosition;
    private float circleRadius;

    public bool pIsGrounded { get; private set; }
    public Vector2 pGroundNormal { get; private set; }

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
        _characterSize = spriteRenderer.bounds.size;
        circleRadius = _characterSize.x / 2.0f;
    }

    private bool IsGrounded()
    {
        footPosition = transform.position;
        footPosition.y -= _characterSize.y / 2.0f - circleRadius + 0.2f;
        pIsGrounded = Physics2D.OverlapCircle(footPosition, circleRadius, groundLayer);
        if (pIsGrounded)
        {
            pGroundNormal = GetGroundNormal();
        }
        return pIsGrounded;
    }

    private Vector2 GetGroundNormal()
    {
        Vector2 result = Vector2.down;
        var hit = Physics2D.Raycast(footPosition, Vector2.down, groundLayer);
        if (hit && hit.collider)
        {
            result = hit.normal;
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(footPosition, circleRadius);
    }
}