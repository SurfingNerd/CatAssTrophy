using UnityEngine;

namespace Levels
{
    public class GameConfig : MonoBehaviour
    {
        [Header("Camera")]
        public float cameraMovementSpeed = 5.0f;

        [Header("Player")]
        public float playerScaleDownFactor = 0.5f; //0.5f for pressing control; else 8


        [Header("Background")]
        public float backgroundWidth = 29.0f;
        public float backgroundScrollSpeed = 0.5f;


        [Header("Background Music")]
        public AudioClip intro;
        public AudioClip level;
        public AudioClip selector;
        //private AudioSource audio;

        [Header("Game Over")]
        public float gameOverDuration = 3;

    }
}