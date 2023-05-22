using UnityEngine;

namespace SetPieceHelpers.BossFightHelpers
{
    public class JumpNode : MonoBehaviour 
    {
        [Header("Dependencies"), SerializeField]
        private JumpNode[] _relatedNodes;

        public JumpNode[] RelatedNodes => _relatedNodes;

        public Vector2 CalculateJumpForce(Vector2 origin)
        {
            Vector2 displacement = (Vector2)transform.position - origin;
            float gravity = Physics2D.gravity.magnitude;

            /* Calculate time of flight to reach the target
             * 
             * This value is calculated based on the equation for vertical motion for a projectile
             * under the influence of gravity. Original formula is like so:
             * 
             * y = v0y * t - (1/2) * g * t^2
             * 
             * By rearranging this formula, we can get an approximation of the time the
             * parabolic arc will take. As such, we can use the time to calculate the velocity components
             * on both axes.
             * 
             * I'm really lucky I like algebra, and just needed the formula. - Matt
             */
            float time = Mathf.Sqrt((2f * displacement.magnitude) / gravity);

            // Calculate the initial X and Y velocities
            float initialVelocityX = displacement.x / time;
            float initialVelocityY = (displacement.y + 0.5f * gravity * Mathf.Pow(time, 2)) / time;

            // Create and return the launch velocity vector
            return new Vector2(initialVelocityX, initialVelocityY);
        }
    }
}
