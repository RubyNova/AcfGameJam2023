using SetPieceHelpers.Pocos;
using UnityEngine;

namespace SetPieceHelpers
{
    public class InvocationListDisposableExecuter : MonoBehaviour
    {
        private void Start()
        {
            SetPieceActionQueue.Instance.ExecuteNextAction();
            Destroy(gameObject);
        }
    }
}
