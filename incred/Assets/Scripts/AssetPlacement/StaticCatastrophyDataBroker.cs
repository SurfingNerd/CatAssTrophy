using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetPlacement
{
    public class StaticCatastrophyDataBroker
    {
        private static List<PrefabPositionInfo> m_positionInfos;
        private static string m_lastSceneName;

        public static bool IsGameStartMode;

        public static void StoreLocationPlacementInfo(List<PrefabPositionInfo> positionInfos)
        {
            m_lastSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            m_positionInfos = positionInfos;
        }

        public static List<PrefabPositionInfo> GetLocationPlacementInfo()
        {
            if (m_lastSceneName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            {
                return m_positionInfos;
            }
            else
            {
                return new List<PrefabPositionInfo>();
            }
        }
    }
}
