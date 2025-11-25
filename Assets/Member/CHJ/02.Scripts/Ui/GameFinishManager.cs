using UnityEngine;
using UnityEngine.Events;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameFinishManager : MonoSingleton<GameFinishManager>
    {
        public UnityEvent onGameOver;
        public UnityEvent onGameClear;
    }
}
