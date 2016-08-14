using UnityEngine;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AssetPlacement
{
    [RequireComponent(typeof(LevelAssetService))]
    public class PlacementService2D : MonoBehaviour
    {
        private LevelAssetService m_levelAssetService;

        private Dictionary<GameObject, LevelAsset> m_assetSelectors = new Dictionary<GameObject, LevelAsset>();
        private Dictionary<LevelAsset, TextMesh> m_textMeshes = new Dictionary<LevelAsset, TextMesh>();

        public bool useSlot;
        //public Material BackgroundMaterial;
        //public Material TextMaterial;

        //private GameObject m_currentSelectedPrefab;
        private LevelAsset m_currentSelectedAsset;
        
        public GameObject PlacementPanel;
        public GameObject SlotPrefab;

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
            //ButtonPlacementCanvas =  GetComponent<Canvas>();


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

                if (Input.GetMouseButtonDown(0))
                {

                    bool selectOtherPrefab = false;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit[] allHits = Physics.RaycastAll(ray);

                    foreach (RaycastHit hit in allHits)
                    {
                        if (m_assetSelectors.ContainsKey(hit.transform.gameObject))
                        {
                            LevelAsset asset = m_assetSelectors[hit.transform.gameObject];
                            m_currentSelectedAsset = asset;
                            selectOtherPrefab = true;
                        }
                    }


                    if (!selectOtherPrefab && m_currentSelectedAsset != null)
                    {
                        if (m_currentSelectedAsset.Count > 0)
                        {
                            GameObject obj = Instantiate(m_currentSelectedAsset.Prefab) as GameObject;
                            m_currentSelectedAsset.Count--;
                            obj.transform.position = vector.Value;
                            PlacedAssets.Add(new KeyValuePair<LevelAsset, GameObject>(m_currentSelectedAsset, obj));

                            UpdateCountText(m_currentSelectedAsset);
                            //Build();

                            if (m_currentSelectedAsset.Count <= 0)
                            {
                                m_currentSelectedAsset = null;
                                Destroy(currentPreviewObject);
                                currentPreviewObject = null;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateCountText(LevelAsset m_currentSelectedAsset)
        {
            //todo: 
            //update m_textMeshes

        }

        private void Build()
        {

            Cleanup();

            //float x = -427;
            //float y = -320;

            //float x = 1;
            //float y = 1;

            //float x = 0.5f;
            //float y = 0.5f;

            for (int i = 0; i < AvailableAssets.Length; i++)
            {
                var asset = AvailableAssets[i];

                if (asset != null)
                {
                    if (asset.Prefab != null && asset.Count > 0)
                    {
                        if (useSlot)
                        {
                            GameObject slot = (GameObject)Instantiate(SlotPrefab);
                            slot.transform.SetParent(this.PlacementPanel.transform);
                            RectTransform slotsRect = slot.GetComponent<RectTransform>();
                            slotsRect.localScale = Vector3.one;


                            GameObject previewObject = Instantiate(asset.Prefab);

                            //previewObject.GetComponents<SpriteRenderer>();
                            MakePreviewObject(previewObject);
                            MakeUILayerObject(previewObject);
                            //previewObject.AddComponent<CanvasRenderer>();
                            previewObject.transform.SetParent(slot.transform);
                        }
                        else
                        {
                            GameObject previewObject = Instantiate(asset.Prefab);
                            MakePreviewObject(previewObject);
                           // previewObject.AddComponent<CanvasRenderer>();
                            previewObject.transform.SetParent(PlacementPanel.transform);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Level asset defined without a selected prefab!");
                    }
                }
            }
        }

        private void MakeUILayerObject(GameObject previewObject)
        {
            
            var renderers = previewObject.GetComponents<SpriteRenderer>();

            Transform oldTransform = previewObject.GetComponent<Transform>();
            Destroy(oldTransform);

            //TODO: correct scaling of the asset.
            //RectTransform rectTransfor = previewObject.AddComponent<RectTransform>();


            //SpriteRenderer thisRenderer = GetComponent<SpriteRenderer>();
            foreach (var spriteRenderer  in renderers)
            {
                //spriteRenderer.sortingLayerID = thisRenderer.sortingLayerID;
                //spriteRenderer.sortingLayerName = thisRenderer.sortingLayerName;
                spriteRenderer.sortingLayerName = "UI";
                spriteRenderer.sortingOrder = 1;
            }

        }

        private void Cleanup()
        {
            foreach (var item in this.m_assetSelectors)
            {
                Destroy(item.Key);
            }

            this.m_assetSelectors.Clear();
        }

        private void MakePreviewObject(GameObject currentPreviewObject)
        {
            DeactivateAllBehaviors(currentPreviewObject);
            DeactivateAllRigidBodies(currentPreviewObject);
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

        }

        private void DeactivateAllColliders(GameObject obj)
        {

            foreach (var collider in GetAll<Collider2D>(obj))
            {
                collider.enabled = false;
            }
        }

        private IEnumerable<T> GetAll<T>(GameObject obj)
        {
            List<Component> components = new List<Component>();
            obj.GetComponents(typeof(T), components);
            components.AddRange(obj.GetComponentsInChildren(typeof(T)));

            return components.Cast<T>();
        }
    }
}