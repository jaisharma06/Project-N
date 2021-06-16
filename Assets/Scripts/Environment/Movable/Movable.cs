using UnityEngine;

public class Movable : MonoBehaviour
{
    public Vector2 m_dragOffset;
    private bool m_isGettingDragged { get; set; }
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_Velocity;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDragged(bool isDragged)
    {
        m_isGettingDragged = isDragged;
    }

    public void SetVelocity(float velocity)
    {
        if (!m_isGettingDragged)
        {
            m_Velocity = Vector2.zero;
            m_Rigidbody.velocity = m_Velocity;
            return;
        }

        m_Velocity.Set(velocity, m_Rigidbody.velocity.y);
        m_Velocity.x = velocity;
        m_Rigidbody.velocity = m_Velocity;
    }


    public Vector3 GetPlayerPosition(Vector3 playerPosition)
    {
        var result = playerPosition;
        var positionDifference = transform.position.x - playerPosition.x;
        if(positionDifference > 0) {
            result.x = transform.position.x - m_dragOffset.x;
        } else {
            result.x = transform.position.x + m_dragOffset.x;
        }
        return result;
    }
}
