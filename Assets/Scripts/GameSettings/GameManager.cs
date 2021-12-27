using UnityEngine;
using ProjectN.Settings;

namespace ProjectN.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameSettings gameSettings;

        private void Awake()
        {
            Application.targetFrameRate = gameSettings.targetFrameRate;
        }
    }
}