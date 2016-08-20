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
        //private Dictionary<LevelAsset, TextMesh> m_textMeshes = new Dictionary<LevelAsset, TextMesh>();

        public bool useSlot;
        //public Material BackgroundMaterial;
        //public Material TextMaterial;

        private GameObject m_currentDraggingObject;
        private LevelAsset m_currentDraggingAsset;
        
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
            //cases:
            //user is currently not dragging an object.
            //----------------------------
            //  Case: starting selection.
            //  if no object is currently drag and dropped, and the user hits an asset in the selection tool
            //  instantiate a new one and let the user drag and drop it.
            //  -------------------------
            //  Case: moving allready instantiated object
            //  setting clicked object as currently dragged
            //  update possition of currently dragged and dropped objects
            //  ------------------------
            //user is currently dragging an object.
            //-----------------------------
            //  Case: all
            //  dragged object position gets updated
            //  Object colering is updated (valid, invalid position)
            //----------------------------
            //user is ending a drag drop operation.(dropping)
            //  Case: within the scene
            //  let the preview object on its position
            //  update colors
            //--------------------------------
            //user is ending drag and drop operation outside the scene. 
            //  case: outside the scene
            //  remove the preview object (it's put back into the selection tool)
            //  means: Update the selection tool ( + 1 count of objects)



            //Vector3? vector = GeometryHelper.GetPositionFromMouse();
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3? vector = Camera.main.ScreenToWorldPoint(screenPos);
            if (vector.HasValue)
            {
                bool startDrag = false;
                startDrag = Input.GetMouseButtonDown(0);

                //todo: Mobile support.

                //create new if not exist
                //if (currentPreviewObject == null)
                //{
                //    currentPreviewObject = Instantiate(m_currentSelectedAsset.Prefab) as GameObject;
                //    MakePreviewObject(currentPreviewObject);
                //}

                //currentPreviewObject.transform.position = vector.Value;

                if (startDrag)
                {

                    bool selectOtherPrefab = false;
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //RaycastHit[] allHits = Physics.RaycastAll(ray);

                    //foreach (RaycastHit hit in allHits)
                    //{
                    //    Debug.Log(hit);
                    //    if (m_assetSelectors.ContainsKey(hit.transform.gameObject))
                    //    {
                    //        LevelAsset asset = m_assetSelectors[hit.transform.gameObject];
                    //        m_currentSelectedAsset = asset;
                    //        selectOtherPrefab = true;
                    //    }
                    //}

                    Vector2 clickPoint2D = new Vector2(vector.Value.x, vector.Value.y);

                    foreach (var item in m_assetSelectors)
                    {
                        Vector3 itemPos = item.Key.transform.position;
                        Renderer renderer = item.Key.GetComponent<Renderer>();

                        if (renderer.bounds.Contains(clickPoint2D))
                        {
                            m_currentDraggingAsset = item.Value;
                        }
                    }


                    if (m_currentDraggingAsset != null)
                    {
                        m_currentDraggingObject = Instantiate(m_currentDraggingAsset.Prefab);
                        m_currentDraggingObject.transform.position = clickPoint2D;
                        MakePreviewObject(m_currentDraggingObject);
                    }

                    //if (!selectOtherPrefab && m_currentSelectedAsset != null)
                    //{
                    //    if (m_currentSelectedAsset.Count > 0)
                    //    {
                    //        GameObject obj = Instantiate(m_currentSelectedAsset.Prefab) as GameObject;
                    //        m_currentSelectedAsset.Count--;
                    //        obj.transform.position = vector.Value;
                    //        PlacedAssets.Add(new KeyValuePair<LevelAsset, GameObject>(m_currentSelectedAsset, obj));

                    //        UpdateCountText(m_currentSelectedAsset);
                    //        //Build();

                    //        if (m_currentSelectedAsset.Count <= 0)
                    //        {
                    //            m_currentSelectedAsset = null;
                    //            Destroy(currentPreviewObject);
                    //            currentPreviewObject = null;
                    //        }
                    //    }
                    //}
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

                            

                            if (asset.Count > 1)
                            {
                                GameObject textHolder = new GameObject();
                                textHolder.transform.parent = slot.transform;
                                TextMesh text = textHolder.AddComponent<TextMesh>();
                                text.offsetZ = -5; //???
                                text.alignment = TextAlignment.Center;
                                text.anchor = TextAnchor.MiddleLeft;

                                //textHolder.AddComponent<Tran>
                                //textHolder.transform.parent = slotsRect;

                                //UnityEngine.UI.Text  text = textHolder.AddComponent<UnityEngine.UI.Text>();
                                text.text = "X " + asset.Count;

                                //UnityEngine.UI.Text text = //UnityEngine.UI.Text.Instantiate<UnityEngine.UI.Text>
                            }

                            GameObject assetSelectorObject = Instantiate(asset.Prefab);
                            MakePreviewObject(assetSelectorObject);
                            MakeUILayerObject(assetSelectorObject);
                            m_assetSelectors.Add(assetSelectorObject, asset);

                            // solution with button click, not very handy, would prefer drag and drop.

                            //UnityEngine.UI.Button button = slot.AddComponent<UnityEngine.UI.Button>();
                            //button.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                            //button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => OnClicked(asset)));

                            assetSelectorObject.transform.SetParent(slot.transform);
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

        private void OnClicked(LevelAsset asset)
        {
            Debug.Log(asset.Prefab.name + " clicked.");            
        }

        private void MakeUILayerObject(GameObject previewObject)
        {
            
            var renderers = previewObject.GetComponents<SpriteRenderer>();

            //Transform oldTransform = previewObject.GetComponent<Transform>();
            //Destroy(oldTransform);

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
                //Debug.Log(item + " deactivated");
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