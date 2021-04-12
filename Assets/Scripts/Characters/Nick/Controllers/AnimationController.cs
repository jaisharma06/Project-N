using UnityEngine;

namespace Characters.Nick
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        public Animator m_animator { get; private set;}
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }


        public void SetWalkSpeed(float speed)
        {
            m_animator.SetFloat("Speed", speed);
        }

        public void SetVerticalSpeed(float verticalSpeed)
        {
            verticalSpeed = Mathf.Clamp(verticalSpeed, -1, 1);
            m_animator.SetFloat("VerticalSpeed", verticalSpeed);
        }

        public void SetIsInTheAir(bool isInAir)
        {
            m_animator.SetBool("IsInAir", isInAir);
        }

        public void SetIsDashing(bool isDashing) 
        {
            m_animator.SetBool("IsDashing", isDashing);
        }

        public void SetSliding(bool isSliding)
        {
            m_animator.SetBool("IsSliding", isSliding);
        }
    }
}

