using UnityEngine;
using System.Collections;

public class BtnResponse : MonoBehaviour {

    public RectTransform rectTransform;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void hoverEffect()
    {
        rectTransform.sizeDelta = new Vector2(120, 120);
    }

    public void btnExit()
    {
        rectTransform.sizeDelta = new Vector2(100, 100);
    }
}
