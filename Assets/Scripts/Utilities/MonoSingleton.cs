using UnityEngine;

namespace Utilities
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
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

			DontDestroyOnLoad(gameObject);
			Init();
			_isInitialised = true;
        }

        public void Init()
        {
			_isInitialised = true;
			OnInit();
        }

        protected abstract void OnInit();

    }
}
