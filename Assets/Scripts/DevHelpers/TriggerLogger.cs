using UnityEngine;

namespace DevHelpers
{
    public class TriggerLogger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision) => print($"Trigger collider from the following gameobject has been detected: {collision.gameObject.name}");
    }
}
