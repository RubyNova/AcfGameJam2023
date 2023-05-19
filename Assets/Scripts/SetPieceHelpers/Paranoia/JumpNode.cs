using Environment;
using UnityEngine;

namespace SetPieceHelpers.Paranoia
{
    public class JumpNode : MonoBehaviour 
    {
        [Header("Dependencies"), SerializeField]
        private JumpNode[] _relatedNodes;

        public JumpNode[] RelatedNodes => _relatedNodes;

        public Vector2 CalculateJumpForce(Vector3 origin)
        {
            Vector3 p = origin;
            Vector3 direction = origin - transform.position;
            float initialAngle = Vector3.Angle(direction, transform.right);

            float gravity = Physics.gravity.magnitude;
            // Selected angle in radians
            float angle = initialAngle * Mathf.Deg2Rad;

            // Positions of this object and the target on the same plane
            Vector3 planarTarget = new Vector3(p.x, 0, p.z);
            Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

            // Planar distance between objects
            float distance = Vector3.Distance(planarTarget, planarPostion);
            // Distance along the y axis between objects
            float yOffset = transform.position.y - p.y;

            float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

            Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

            // Rotate our velocity to match the direction between the two objects
            float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1); Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

            return (Vector2)finalVelocity;
        }
    }
}
