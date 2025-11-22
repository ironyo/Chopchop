using UnityEngine;
using UnityEngine.Events;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameFinishManager : MonoBehaviour
    {
        public UnityEvent onGameOver;
        public UnityEvent onGameClear;
        public static GameFinishManager Instance;
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}
