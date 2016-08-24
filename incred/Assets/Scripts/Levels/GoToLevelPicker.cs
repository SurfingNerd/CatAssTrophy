using UnityEngine;
using System.Collections;


namespace Levels
{
    public class GoToLevelPicker : MonoBehaviour {

        public int waitSeconds = 5;

        private bool rotating = false;
        GameObject bg;
        // Use this for initialization
        void Start()
        {
            bg = GameObject.Find("hiding-background");
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log (bg.transform.rotation.eulerAngles.x);

            if (rotating && bg.transform.rotation.eulerAngles.x < 180)
            {
                bg.transform.Rotate(1, 0, 0);
            }

            if (Input.anyKey)
            {
                rotating = true;
                StartCoroutine(LoadAfterTime(waitSeconds));
            }
        }

        public IEnumerator LoadAfterTime(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            LevelManager.LoadLevelPicker();
        }
    }
}