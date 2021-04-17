using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask groundLayer;
    public Transform wallCheck;
    public Transform groundCheck;
    public float wallCheckDistance;
    public float groundCheckRadius;
    private Vector2 _characterSize;


    public bool pIsGrounded { get; private set; }
    public bool pIsTouchingWall { get; private set; }
    public Vector2 pGroundNormal { get; private set; }

    private void Start()
    {
        pIsGrounded = true;
    }

    private void Update()
    {
        IsGrounded();
        CheckIfTouchingWall();
    }

    private bool IsGrounded()
    {
        pIsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (pIsGrounded)
        {
            pGroundNormal = GetGroundNormal();
        }
        return pIsGrounded;
    }

    private Vector2 GetGroundNormal()
    {
        Vector2 result = Vector2.down;
        var hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundLayer);
        if (hit && hit.collider)
        {
            result = hit.normal;
        }

        return result;
    }

    private void CheckIfTouchingWall()
    {
        var rayCastHit = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
        pIsTouchingWall = (rayCastHit) ? true : false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}