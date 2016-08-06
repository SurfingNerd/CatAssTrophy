using UnityEngine;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AssetPlacement
{
    [RequireComponent(typeof(LevelAssetService))]
    public class PlacementService : MonoBehaviour
    {
        private LevelAssetService m_levelAssetService;

        private Dictionary<GameObject, LevelAsset> m_assetSelectors = new Dictionary<GameObject, LevelAsset>();



        //private GameObject m_currentSelectedPrefab;
        private LevelAsset m_currentSelectedAsset;
        
        public Canvas m_buttonPlacementCanvas;

        [HideInInspector]
        // Key = Prefab, Value = Instance
        public List<System.Collections.Generic.KeyValuePair<LevelAsset, GameObject>> PlacedAssets
            = new List<KeyValuePair<LevelAsset, GameObject>>();



        [HideInInspector]
        public LevelAsset[] AvailableAssets;

        // Use this for initialization
        void Start()
        {
            Build();

        }

        void Awake()
        {
            m_levelAssetService = GetComponent<LevelAssetService>();
            m_buttonPlacementCanvas = GetComponent<Canvas>();
            if (m_levelAssetService != null)
            {
                AvailableAssets = m_levelAssetService.Assets;
            }
            else
            {
                Debug.LogError(typeof(LevelAssetService).FullName + " not defined!");
            }
        }

        GameObject currentPreviewObject;


        // Update is called once per frame
        void Update()
        {
            //if (m_currentSelectedAsset != null)
            //{

            Vector3? vector = GeometryHelper.GetPositionFromMouse();
            if (vector.HasValue)
            {
                //create new if not exist
                if (currentPreviewObject == null && m_currentSelectedAsset != null && m_currentSelectedAsset.Prefab != null)
                {
                    currentPreviewObject = Instantiate(m_currentSelectedAsset.Prefab) as GameObject;
                    MakePreviewObject(currentPreviewObject);
                }

                if (currentPreviewObject != null)
                {
                    currentPreviewObject.transform.position = vector.Value;
                }

                //DeactivateAllColliders(currentPreviewObject);


                if (Input.GetMouseButtonDown(0))
                {

                    bool selectOtherPrefab = false;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit[] allHits = Physics.RaycastAll(ray);

                    foreach (RaycastHit hit in allHits)
                    {
                        //if (hit.)
                        if (m_assetSelectors.ContainsKey(hit.transform.gameObject))
                        {
                            LevelAsset asset = m_assetSelectors[hit.transform.gameObject];
                            m_currentSelectedAsset = asset;
                            
                            selectOtherPrefab = true;

                            //m_assetSelectors.Remove(hit.transform.gameObject);
                            //Destroy(hit.transform.gameObject);
                        }
                    }


                    if (!selectOtherPrefab && m_currentSelectedAsset != null)
                    {
                        
                        GameObject obj = Instantiate(m_currentSelectedAsset.Prefab) as GameObject;
                        m_currentSelectedAsset.Count--;
                        obj.transform.position = vector.Value;
                        PlacedAssets.Add(new KeyValuePair<LevelAsset, GameObject>(m_currentSelectedAsset, obj));

                        Build();
                    }



                    //foreach (LevelAsset asset in AvailableAssets)
                    //{
                    //    if (asset.Prefab == m_currentSelectedPrefab)
                    //    {
                    //        asset.Count--;
                    //    }
                    //}
                    //}
                }
            }
        }

        private void Build()
        {

            Cleanup();

            //float x = -427;
            //float y = -320;

            //float x = 1;
            //float y = 1;

            float x = 0.5f;
            float y = 0.5f;

            for (int i = 0; i < AvailableAssets.Length; i++)
            {
                var asset = AvailableAssets[i];

                if (asset != null)
                {
                    if (asset.Prefab != null)
                    {
                        //create UI for that asset and register a click handler
                        string uiName = asset.Prefab.name;
                        if (asset.Count > 1)
                        {
                            //uiName += " x " + asset.Count;
                        }
                        //UnityEngine.GUI.Button(new Rect(50, i * 20, 40, 10), uiName);

                        //WTF do i need a prefab for 

                        GameObject objectHolder = Instantiate<GameObject>(new GameObject());

                        objectHolder.transform.position = new Vector3(x, y, 0);
                        objectHolder.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        objectHolder.transform.parent = m_buttonPlacementCanvas.transform;


                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //cube.transform.position = new Vector3(x, y, 0);
                        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        cube.transform.parent = objectHolder.transform.parent;

                        m_assetSelectors.Add(cube, asset);
                    }
                    else
                    {
                        Debug.LogWarning("Level asset defined without a selected prefab!");
                    }
                }
            }
        }

        private void Cleanup()
        {
            foreach (var item in this.m_assetSelectors)
            {
                Destroy(item.Key);
            }
        }

        private void MakePreviewObject(GameObject currentPreviewObject)
        {
            DeactivateAllBehaviors(currentPreviewObject);
            //DeactivateAllColliders(currentPreviewObject);
            DeactivateAllRigidBodies(currentPreviewObject);
            //DeactivateAllJoints(currentPreviewObject);
        }

        private void DeactivateAllBehaviors(GameObject currentPreviewObject)
        {
            foreach (var item in GetAll<Behaviour>(currentPreviewObject))
            {
                item.enabled = false;
            }
        }

        private void DeactivateAllRigidBodies(GameObject currentPreviewObject)
        {
            foreach (var rig in GetAll<Rigidbody2D>(currentPreviewObject))
            {
                rig.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        private void DeactivateAllJoints(GameObject currentPreviewObject)
        {
            foreach (var item in GetAll<Joint2D>(currentPreviewObject))
            {
                item.enabled = false;
            }


            //Rigidbody2D rigBody = currentPreviewObject.GetComponent<Rigidbody2D>();

            //rigBody.enabled = false;


        }

        private void DeactivateAllColliders(GameObject obj)
        {

            foreach (var collider in GetAll<Collider2D>(obj))
            {
                collider.enabled = false;
            }
        }

        //private System.Collections.Generic.IEnumerable<T> GetAll<T>(GameObject obj)
        //{
        //    T parentObject = obj.GetComponent<T>();
        //    if (parentObject != null)
        //        yield return parentObject;

        //    foreach (T child in obj.GetComponentsInChildren<T>())
        //    {
        //        yield return child;
        //    }
        //}

        private IEnumerable<T> GetAll<T>(GameObject obj)
        {
            List<Component> components = new List<Component>();
            obj.GetComponents(typeof(T), components);
            components.AddRange(obj.GetComponentsInChildren(typeof(T)));

            return components.Cast<T>();
        }
    }


    class GeometryHelper
    {
        public static Vector3? GetPositionFromMouse()
        {
            //Transform transform = GetComponent<Transform>();
            Plane plane = new UnityEngine.Plane(Vector3.back, Vector3.zero);

            //Plane plane = Plane.GetComponent<Plane>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //        RaycastHit hit;

            //Vector3 newPosition = new Vector3((float)System.Math.Round(ray.origin.x), 1, (float)System.Math.Round(ray.origin.z));
            float center;

            if (plane.Raycast(ray, out center))
            {
                Vector3 point = ray.GetPoint(center);
                return point;
            }

            return null;
        }
    }
}