using UnityEngine;
using System.Collections;

namespace Texture
{
    public class BtnResponse : MonoBehaviour
    {
        public RectTransform rectTransform;
        public int normalSizeX;
        public int normalSizeY;
        public int biggerSizeX;
        public int biggerSizeY;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void hoverEffect()
        {
            rectTransform.sizeDelta = new Vector2(biggerSizeX, biggerSizeY);
        }

        public void btnExit()
        {
            rectTransform.sizeDelta = new Vector2(normalSizeX, normalSizeY);
        }
    }
}