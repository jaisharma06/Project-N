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
            m_animator.SetFloat("VerticalSpeed", verticalSpeed);
        }

        public void SetIsInTheAir(bool isInAir)
        {
            m_animator.SetBool("IsInAir", isInAir);
        }
    }
}

