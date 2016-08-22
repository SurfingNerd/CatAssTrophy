using UnityEngine;
using System.Collections;


namespace Assets.Scripts.Levels
{
    public class UnfreezeOnStart : MonoBehaviour, IStartableGameObject
    {
        public void StartGame()
        {
            Rigidbody2D rigBody = GetComponent<Rigidbody2D>();
            rigBody.constraints = RigidbodyConstraints2D.None;
        }

    }
}