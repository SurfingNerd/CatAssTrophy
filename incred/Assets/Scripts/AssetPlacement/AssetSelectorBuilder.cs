using UnityEngine;
using System.Collections.Generic;


namespace AssetPlacement
{
    public class AssetSelectorBuilder : MonoBehaviour
    {
        private Canvas m_buttonPlacementCanvas;

        private PlacementService m_placementService;

        private Dictionary<GameObject, LevelAsset> m_assetSelectors = new Dictionary<GameObject, LevelAsset>();


        // Use this for initialization
        void Start()
        {

           // Build();

        }

        public void Awake()
        {
            m_placementService = GetComponent<PlacementService>();
            m_buttonPlacementCanvas = GetComponent<Canvas>();

        }

        //public void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        bool raycastOnThis = false;

        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //        RaycastHit[] allHits = Physics.RaycastAll(ray);

        //        foreach (RaycastHit hit in allHits)
        //        {
        //            //if (hit.)
        //            if (m_assetSelectors.ContainsKey(hit.transform.gameObject))
        //            {
        //                LevelAsset asset = m_assetSelectors[hit.transform.gameObject];
        //                m_placementService.SelectAsset(asset);
        //            }
        //        }

        //        if (raycastOnThis)
        //        {
        //            //m_placementService.SelectPrefab();
        //        }
        //        //Input.mousePosition
        //    }
        //}


        //private void Build()
        //{
        //    //float x = -427;
        //    //float y = -320;

        //    //float x = 1;
        //    //float y = 1;

        //    float x = 0.5f;
        //    float y = 0.5f;

        //    for (int i = 0; i < m_placementService.AvailableAssets.Length; i++)
        //    {
        //        var asset = m_placementService.AvailableAssets[i];

        //        if (asset != null)
        //        {
        //            if (asset.Prefab != null)
        //            {
        //                //create UI for that asset and register a click handler
        //                string uiName = asset.Prefab.name;
        //                if (asset.Count > 1)
        //                {
        //                    uiName += " x " + asset.Count;
        //                }
        //                //UnityEngine.GUI.Button(new Rect(50, i * 20, 40, 10), uiName);

        //                //WTF do i need a prefab for 

        //                GameObject objectHolder = Instantiate<GameObject>(new GameObject());

        //                objectHolder.transform.position = new Vector3(x, y, 0);
        //                objectHolder.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //                objectHolder.transform.parent = m_buttonPlacementCanvas.transform;


        //                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //                //cube.transform.position = new Vector3(x, y, 0);
        //                cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //                cube.transform.parent = objectHolder.transform.parent;

        //                m_assetSelectors.Add(cube, asset);
        //            }
        //            else
        //            {
        //                Debug.LogWarning("Level asset defined without a selected prefab!");
        //            }
        //        }
        //    }
        //}


        private void Cleanup()
        {
        }
    }
}