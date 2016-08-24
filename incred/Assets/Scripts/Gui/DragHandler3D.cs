using UnityEngine;
using System.Collections;

namespace Gui
{

    public class DragHandler3D : MonoBehaviour
    {

        private Vector3 screenPoint;
        //private Vector3 offset;
        private Vector3 scanPos = Vector3.zero;

        void OnMouseDown()
        {
            screenPoint = UnityEngine.Camera.main.WorldToScreenPoint(scanPos);


            //offset = scanPos - Camera.main.ScreenToWorldPoint(
            //	new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        }


        void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = UnityEngine.Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = curPosition;

        }

        public void addThisObject()
        {
            Debug.Log("moving..");
            transform.position = new Vector3(5f, 3f, -2.0f);
        }
    }
}
