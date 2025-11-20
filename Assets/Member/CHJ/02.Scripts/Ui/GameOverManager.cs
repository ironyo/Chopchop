using UnityEngine;
using UnityEngine.Events;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameOverManager : MonoBehaviour
    {
        public UnityEvent onGameOver;
        public static GameOverManager Instance;
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}
