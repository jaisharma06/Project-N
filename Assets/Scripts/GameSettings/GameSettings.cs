using UnityEngine;

namespace ProjectN.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ProjectN/Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public int targetFrameRate = 120;
    }
}