using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Sounds
{

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        private static bool m_bIsCurrentlyInStartupProcess;
        public AudioSource AudioSource;
        private bool m_bIsCurrentlyPlaying;
        

        // Use this for initialization
        void Start()
        {
            Instance = this;
            m_bIsCurrentlyInStartupProcess = false;
        }

        public static void LoadAndEnsureGameMusic(MonoBehaviour owningBehavior)
        {
            if (!m_bIsCurrentlyInStartupProcess)
            {
                m_bIsCurrentlyInStartupProcess = true;
                //for some very strange reason, this triggers more than once...
                //Instance is static and IS SET MORE THAN once.
                if (Instance == null)
                {
                    owningBehavior.StartCoroutine(LoadEndEnsureGameMusicRoutine());
                }
            }
        }

        private static IEnumerator LoadEndEnsureGameMusicRoutine()
        {
            if (Instance == null)
            {
                var result = SceneManager.LoadSceneAsync("SoundManagmentScene", LoadSceneMode.Additive);

                while (!result.isDone)
                {
                    yield return new WaitForEndOfFrame();
                }

                while (Instance == null)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            Instance.EnsureGameMusic();
        }

        private void EnsureGameMusic()
        {
            if (!m_bIsCurrentlyPlaying)
            {
                AudioSource.Play();
                m_bIsCurrentlyPlaying = true;
            }
        }

        public void StopGameMusic()
        {
            if (m_bIsCurrentlyPlaying)
            {
                AudioSource.Stop();
                m_bIsCurrentlyPlaying = false;
            }
        }

    }
}
