using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevHelpers
{
    public class CineMachineUnityEventTest : MonoBehaviour
    {
        public void OnObjectEnter()
        {
            print("Object entered!");
        }

        public void OnObjectExit()
        {
            print("Object exited!");
        }
    }

}
