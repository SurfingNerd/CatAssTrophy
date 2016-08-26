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
        //private LevelAssetService m_levelAssetService;
        private bool m_isCurrentlyPlayingGame;
        private Dictionary<LevelAsset, GameObject> m_assetSelectors = new Dictionary<LevelAsset, GameObject>();
        private Dictionary<LevelAsset, List<GameObject>> m_inScenePreviewObjects = new Dictionary<LevelAsset, List<GameObject>>();
        //private Dictionary<LevelAsset, List<Vector3>> m_positionsAtLastGameStart = new Dictionary<LevelAsset, List<Vector3>>();

        private Dictionary<LevelAsset, GameObject> m_textHolders = new Dictionary<LevelAsset, GameObject>();
        private Dictionary<LevelAsset, TextMesh> m_textElements = new Dictionary<LevelAsset, TextMesh>();

        private GameObject m_currentDraggingObject;
        private LevelAsset m_currentDraggingAsset;
        private bool m_isDraggingCamera;
        private float m_originalCameraSize; 

        // Click on the plane in world space for camera movement interaction.
        private Vector2 m_lastCameraClickPosition;

        public GameObject PlacementPanel;
        public Color AssetCountTextColor;
        public LevelAssetService LevelAssetService;

        //Camera
        public UnityEngine.Camera Camera;
        public float MinCameraSize;
        public float MaxCameraSize;
        public float MaxCameraOutOfScene;
        public float ZoomSpeed;
        public GameObject Wall;

        [HideInInspector]
        // Key = Prefab, Value = Instance
        public List<System.Collections.Generic.KeyValuePair<LevelAsset, GameObject>> PlacedAssets
            = new List<KeyValuePair<LevelAsset, GameObject>>();

        [HideInInspector]
        public LevelAsset[] AvailableAssets;

        // Use this for initialization
        void Start()
        {
            AvailableAssets = LevelAssetService.Assets;
            m_originalCameraSize = Camera.orthographicSize;
            Build();
            ApplyPositions(StaticLocationPlacementStorage.GetLocationPlacementInfo());
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

            bool isButtonHold = Input.GetMouseButton(0);
            bool isButtonClicked = Input.GetMouseButtonDown(0);

            ApplyCameraZoom();
            
            if (m_isDraggingCamera)
            {
                if (isButtonHold)
                {
                    ApplyCameraMovement(clickPoint2D);
                }
                else
                {
                    m_isDraggingCamera = false;
                }
            }
            else if(m_currentDraggingObject == null)
            {
                if (isButtonClicked)
                {
                    StartDrag(clickPoint2D);
                }       
            }
            else //currently dragging
            {
                if (isButtonHold)
                {
                    UpdateDragPosition(clickPoint2D);
                }
                else //exiting drag
                {
                    EndDrag(clickPoint2D);
                }
            }
        }

        private void ApplyCameraZoom()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                float cameraSize = Camera.orthographicSize + (Input.mouseScrollDelta.y * Camera.orthographicSize * - ZoomSpeed);

                float oldOrthographicSize = Camera.orthographicSize;
                if (cameraSize > MaxCameraSize)
                {
                    Camera.orthographicSize = MaxCameraSize;
                }
                else if (cameraSize < MinCameraSize)
                {
                    Camera.orthographicSize = MinCameraSize;
                }
                else
                {
                    Camera.orthographicSize = cameraSize;
                }


                //Update scaling of panel to new orthographic size
                //float = PlacementPanel.transform.localScale.x

                UpdateDragPanelToCamera();
            }
        }

        private void UpdateDragPanelToCamera()
        {
            Vector3 worldPointInTheMiddle = Camera.ScreenToWorldPoint(new Vector3(Camera.pixelWidth * 0.95f, Camera.pixelHeight / 2));
            PlacementPanel.transform.position = new Vector3(worldPointInTheMiddle.x, worldPointInTheMiddle.y, 0);

            float panelScaleFactor = Camera.orthographicSize / m_originalCameraSize;
            PlacementPanel.transform.localScale = new Vector3(panelScaleFactor, panelScaleFactor, panelScaleFactor);

            //Debug.Log(worldPointInTheMiddle);

            //Debug.Log(Camera.rect);
            //Debug.Log(Camera.pixelWidth);
            //Debug.Log(Camera.)

            //PlacementPanel.transform.transform.localScale = new Vector3(
            //    Camera.rect.width, Camera.rect.height);
            //Camera.rect
        }

        private void ApplyCameraMovement(Vector2 clickPoint2D)
        {
            Camera.transform.position = Camera.transform.position + ((Vector3)m_lastCameraClickPosition - (Vector3)clickPoint2D);
            UpdateDragPanelToCamera();
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
                RectTransform rectTransform = PlacementPanel.transform as RectTransform;
                float biggestX = float.MinValue;
                bool contains = false;
                if (rectTransform != null)
                {
                    Vector3[] localcorners = new Vector3[4];
                    rectTransform.GetLocalCorners(localcorners);

                    for (int i = 0; i <= 4; i++)
                    {
                        Vector3 globalCorner = rectTransform.TransformPoint(localcorners[0]);
                        if (globalCorner.x > biggestX)
                        {
                            biggestX = globalCorner.x;
                        }
                    }
                    Rect rect = new Rect(biggestX, -1000, 1000, 2000);
                    contains = rect.Contains(clickPoint2D);
                }
                else
                {
                    Renderer renderer = null; 

                    for (int i = 0; i < PlacementPanel.transform.childCount; i++)
                    {
                        Transform transform = PlacementPanel.transform.GetChild(i);
                        if (transform.gameObject.name == "background")
                        {
                            renderer = transform.gameObject.GetComponent<Renderer>();
                        }
                    }

                    contains = renderer.bounds.Contains(clickPoint2D);

                    //PlacementPanel.
                }
                //todo: What if only a normal transform ? 
                //get values from renderer ?
                
                //be sure to even track positions right outside the panel.
                

                
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
                m_isDraggingCamera = true;
                m_lastCameraClickPosition = clickPoint2D;
                return;
            }
            //check if starting draging
            foreach (var item in m_assetSelectors)
            {
                Renderer renderer = item.Value.GetComponent<Renderer>();

                if (renderer.bounds.Contains(clickPoint2D))
                {
                    m_currentDraggingAsset = item.Key;
                    m_isDraggingCamera = false;
                }
            }

            if (m_currentDraggingAsset != null)
            {
                if (m_currentDraggingAsset.Count > 0)
                {
                    m_currentDraggingObject = PlaceAsset(clickPoint2D, m_currentDraggingAsset);
                    m_isDraggingCamera = false;
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
                            m_isDraggingCamera = false;
                            break;
                        }
                    }
                    if (m_currentDraggingObject != null)
                    {
                        break;
                    }
                }
            }

            if (m_currentDraggingAsset == null)
            {
                m_isDraggingCamera = true;
                m_lastCameraClickPosition = clickPoint2D;
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
            //m_positionsAtLastGameStart = new Dictionary<LevelAsset, List<Vector3>>();
            List<GameObject> objectsToDestroy = new List<GameObject>();
            //TODO: replace the preview objects with real objects that have physics reanabled.
            foreach (var kvp in m_inScenePreviewObjects)
            {
                List<Vector3> positions = new List<Vector3>();
                //m_positionsAtLastGameStart.Add(kvp.Key, positions);
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
            if (m_textHolders.ContainsKey(asset))
            {
                TextMesh text = m_textElements[asset];
                text.text = asset.Count.ToString();
                
            }
            else
            {
                GameObject textHolder = new GameObject("textHolder");

                textHolder.transform.SetParent(PlacementPanel.transform);
                //textHolder.transform.SetParent(m_assetSelectors[asset].transform);  //slot.transform;

                //Place the text relative to the asset.
                GameObject assetSelector = m_assetSelectors[asset];
                
                textHolder.transform.localPosition = new Vector3(assetSelector.transform.localPosition.x - 1, assetSelector.transform.localPosition.y, assetSelector.transform.localPosition.z);
                

                TextMesh text = textHolder.AddComponent<TextMesh>();
                text.offsetZ = -1.5f; //???
                text.fontSize = 128;
                text.characterSize = 0.1f;
                


                text.alignment = TextAlignment.Center;
                text.anchor = TextAnchor.MiddleCenter;
                text.text = asset.Count.ToString();
                text.color = AssetCountTextColor;
                m_textHolders.Add(asset, textHolder);
                m_textElements.Add(asset, text);
            }
        }

        private void Build()
        {
            Cleanup();

            //Renderer renderer = PlacementPanel.GetComponent<Renderer>();
            // renderer.bounds.size.y
            float availablesize = 8f;
            
            float currentY = availablesize / 2;
            float stepY = availablesize / 4f;

            for (int i = 0; i < AvailableAssets.Length; i++)
            {
                var asset = AvailableAssets[i];

                if (asset != null)
                {
                    if (asset.Prefab != null && asset.Count > 0)
                    {
                        GameObject assetSelectorObject = Instantiate(asset.Prefab);
                        
                        assetSelectorObject.transform.SetParent(PlacementPanel.transform);
                        assetSelectorObject.transform.localPosition = new Vector3(0, currentY, 0);
                        currentY -= stepY;

                        MakePreviewObject(assetSelectorObject);
                        MakeUILayerObject(assetSelectorObject);
                        m_assetSelectors.Add(asset, assetSelectorObject);

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

        public List<PrefabPositionInfo> GetPositionInfos()
        {
            List<PrefabPositionInfo> result = new List<PrefabPositionInfo>();

            foreach (var kvp in m_inScenePreviewObjects)
            {
                foreach (var obj in kvp.Value)
                {
                    result.Add(new PrefabPositionInfo() { Prefab = kvp.Key.Prefab, Position = obj.transform.position});
                }
            }

            return result;
        }

        public void ApplyPositions(List<PrefabPositionInfo> positions)
        {
            foreach (PrefabPositionInfo posInfo in positions)
            {
                foreach (LevelAsset asset in AvailableAssets)
                {
                    if (asset.Prefab == posInfo.Prefab)
                    {
                        PlaceAsset(posInfo.Position, asset);
                    }
                }
            }
        }

        private GameObject PlaceAsset(Vector3 position, LevelAsset asset)
        {
            GameObject result = Instantiate(asset.Prefab);
            asset.Count--;
            result.transform.position = position;
            RememberPreviewObject(result, asset);
            MakePreviewObject(result);
            UpdateButtonUI(asset);

            return result;
        }
    }

    public class PrefabPositionInfo
    {
        public Vector3 Position;
        public GameObject Prefab;
    }
}