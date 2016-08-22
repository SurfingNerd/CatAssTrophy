using UnityEngine;
using System.Collections;
using System;
using Levels;

namespace Physics
{
    public class UnfreezeOnStart : MonoBehaviour, IStartableGameObject
    {
        public void StartGame()
        {
            if (isActiveAndEnabled)
            {
                Rigidbody2D rigBody = GetComponent<Rigidbody2D>();
                rigBody.constraints = RigidbodyConstraints2D.None;
            }
        }

    }
}