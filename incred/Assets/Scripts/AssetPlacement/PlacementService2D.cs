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
        private bool m_isCurrentlyPlayingGame;
        private Dictionary<LevelAsset, GameObject> m_assetSelectors = new Dictionary<LevelAsset, GameObject>();
        private Dictionary<LevelAsset, List<GameObject>> m_inScenePreviewObjects = new Dictionary<LevelAsset, List<GameObject>>();
        private Dictionary<LevelAsset, List<Vector3>> m_positionsAtLastGameStart = new Dictionary<LevelAsset, List<Vector3>>();

        private Dictionary<LevelAsset, GameObject> m_textHolders = new Dictionary<LevelAsset, GameObject>();
        private Dictionary<LevelAsset, TextMesh> m_textElements = new Dictionary<LevelAsset, TextMesh>();

        //public Material BackgroundMaterial;
        //public Material TextMaterial;

        private GameObject m_currentDraggingObject;
        private LevelAsset m_currentDraggingAsset;
        
        public GameObject PlacementPanel;
        public GameObject SlotPrefab;
        public Color Color;

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



            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 vector = UnityEngine.Camera.main.ScreenToWorldPoint(screenPos);
            Vector2 clickPoint2D = new Vector2(vector.x, vector.y);

            if (m_currentDraggingObject == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartDrag(clickPoint2D);
                }       
            }
            else //currently dragging
            {
                if (Input.GetMouseButton(0))
                {
                    UpdateDragPosition(clickPoint2D);
                }
                else //exiting drag
                {
                    EndDrag(clickPoint2D);
                }
            }
        }

        private void EndDrag(Vector2 clickPoint2D)
        {
            if (m_isCurrentlyPlayingGame)
            {
                return;
            }
            //If object is dropped out of scene, it gets brought back to asset selection.
            if (m_currentDraggingObject != null)
            {
                RectTransform rectTransform = PlacementPanel.GetComponent<RectTransform>();
                Vector3[] localcorners = new Vector3[4];
                rectTransform.GetLocalCorners(localcorners);

                float biggestX = float.MinValue;
                //Vector3[] globalCorners = new Vector3[4];
                for (int i = 0; i <= 4; i++)
                {
                    Vector3 globalCorner = rectTransform.TransformPoint(localcorners[0]);
                    if (globalCorner.x > biggestX)
                    {
                        biggestX = globalCorner.x;
                    }
                }
                
                //be sure to even track positions right outside the panel.
                Rect rect = new Rect(biggestX, -1000, 1000, 2000);

                bool contains = rect.Contains(clickPoint2D);
                //Debug.Log("Rect: " + rect + " | " + clickPoint2D + " | " + contains);
                if (contains)
                {
                    m_currentDraggingAsset.Count++;
                    UpdateCountText(m_currentDraggingAsset);
                    m_inScenePreviewObjects[m_currentDraggingAsset].Remove(m_currentDraggingObject);
                    Destroy(m_currentDraggingObject);
                }
            }
            m_currentDraggingObject = null;
            m_currentDraggingAsset = null;

        }

        private void UpdateDragPosition(Vector2 clickPoint2D)
        {
            m_currentDraggingObject.transform.position = clickPoint2D;
        }

        private void StartDrag(Vector2 clickPoint2D)
        {
            if (m_isCurrentlyPlayingGame)
            {
                return;
            }
            //check if starting draging
            foreach (var item in m_assetSelectors)
            {
                Renderer renderer = item.Value.GetComponent<Renderer>();

                if (renderer.bounds.Contains(clickPoint2D))
                {
                    m_currentDraggingAsset = item.Key;
                }
            }

            if (m_currentDraggingAsset != null)
            {
                if (m_currentDraggingAsset.Count > 0)
                {
                    m_currentDraggingObject = Instantiate(m_currentDraggingAsset.Prefab);
                    m_currentDraggingAsset.Count--;
                    m_currentDraggingObject.transform.position = clickPoint2D;
                    RememberPreviewObject(m_currentDraggingObject, m_currentDraggingAsset);
                    MakePreviewObject(m_currentDraggingObject);
                    UpdateButtonUI(m_currentDraggingAsset);
                }
            }
            else //not started to drag an object from the asset selection
            {
                //maybe user is dragging existing object ?
                foreach (var kvp in m_inScenePreviewObjects)
                {
                    foreach (GameObject obj in kvp.Value)
                    {
                        Renderer renderer = obj.GetComponent<Renderer>();
                        if (renderer.bounds.Contains(clickPoint2D))
                        {
                            m_currentDraggingAsset = kvp.Key;
                            m_currentDraggingObject = obj;
                            m_currentDraggingObject.transform.position = clickPoint2D;
                            break;
                        }
                    }
                    if (m_currentDraggingObject != null)
                    {
                        break;
                    }
                }
            }
        }

        private void RememberPreviewObject(GameObject gameObject, LevelAsset asset)
        {
            //if (m_inScenePreviewObjects.ContainsKey(asset)
            List<GameObject> gameObjects = null;
            if (!m_inScenePreviewObjects.TryGetValue(asset, out gameObjects ))
            {
                gameObjects = new List<GameObject>();
                m_inScenePreviewObjects.Add(asset, gameObjects);
            }

            gameObjects.Add(gameObject);
        }

        private void UpdateButtonUI(LevelAsset currentDraggingAsset)
        {
            if (currentDraggingAsset.Count > 0)
            {
                //TODO: Undo make button gray
            }
            else
            {
                //todo: make buton gray
            }

            UpdateCountText(currentDraggingAsset);
        }

        public void StartGame()
        {
            m_isCurrentlyPlayingGame = true; //<< deactivates drag and drop.
            m_positionsAtLastGameStart = new Dictionary<LevelAsset, List<Vector3>>();
            List<GameObject> objectsToDestroy = new List<GameObject>();
            //TODO: replace the preview objects with real objects that have physics reanabled.
            foreach (var kvp in m_inScenePreviewObjects)
            {
                List<Vector3> positions = new List<Vector3>();
                m_positionsAtLastGameStart.Add(kvp.Key, positions);
                foreach (var previewObject in kvp.Value)
                {
                    GameObject go = (GameObject)Instantiate(kvp.Key.Prefab);
                    go.transform.position = previewObject.transform.position;
                    positions.Add(previewObject.transform.position);
                    objectsToDestroy.Add(previewObject);
                }
            }

            m_inScenePreviewObjects.Clear();
            m_currentDraggingObject = null;
            m_currentDraggingAsset = null;

            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
        }

        private void UpdateCountText(LevelAsset asset)
        {
            //todo: 
            //update m_textMeshes
            //if (asset.Count > 1)
            //{
            if (m_textHolders.ContainsKey(asset))
            {
                TextMesh text = m_textElements[asset];
                text.text = "X " + asset.Count;
            }
            else
            {
                GameObject textHolder = new GameObject();
                textHolder.transform.parent = m_assetSelectors[asset].transform;  //slot.transform;
                //textHolder.transform.localScale = new Vector3(1, 1, 1);
                TextMesh text = textHolder.AddComponent<TextMesh>();
                text.offsetZ = -5; //???
                text.fontSize = 64;
                text.characterSize = 0.1f;
                textHolder.transform.position = new Vector3(-2.5f, 0, 0);
                text.alignment = TextAlignment.Center;
                text.anchor = TextAnchor.MiddleLeft;
                text.text = "X " + asset.Count;
                text.color = Color;
                m_textHolders.Add(asset, textHolder);
                m_textElements.Add(asset, text);
            }
            //}
            //else
            //{

            //}
        }

        private void Build()
        {
            Cleanup();

            for (int i = 0; i < AvailableAssets.Length; i++)
            {
                var asset = AvailableAssets[i];

                if (asset != null)
                {
                    if (asset.Prefab != null && asset.Count > 0)
                    {
                        GameObject slot = (GameObject)Instantiate(SlotPrefab);
                        slot.transform.SetParent(this.PlacementPanel.transform);
                        RectTransform slotsRect = slot.GetComponent<RectTransform>();
                        slotsRect.localScale = Vector3.one;
                        
                        GameObject assetSelectorObject = Instantiate(asset.Prefab);
                        MakePreviewObject(assetSelectorObject);
                        MakeUILayerObject(assetSelectorObject);
                        m_assetSelectors.Add(asset, assetSelectorObject);
                        assetSelectorObject.transform.SetParent(slot.transform);

                        UpdateButtonUI(asset);
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
            
            //SpriteRenderer thisRenderer = GetComponent<SpriteRenderer>();
            foreach (var spriteRenderer  in renderers)
            {
                spriteRenderer.sortingLayerName = "UI";
                spriteRenderer.sortingOrder = 1;
            }
        }

        private void Cleanup()
        {
            //TODO: remove ?!
            foreach (var item in this.m_assetSelectors)
            {
                Destroy(item.Value);
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