using UnityEngine;

namespace Utilities
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance  = new GameObject().AddComponent<T>();
				}

				return _instance;
			}
		}

        public static bool HasInstanceCreated => _instance != null;

		private bool _isInitialised;

        private void Awake()
        {
            if (_isInitialised)
			{
				return;
			}

			Init();
			_isInitialised = true;
        }

        private void OnServerInitialized()
        {
			_isInitialised = true;
			OnInit();
        }
        protected abstract void OnInit();

    }
}
