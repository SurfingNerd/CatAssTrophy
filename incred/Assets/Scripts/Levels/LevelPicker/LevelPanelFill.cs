using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Levels.LevelPicker
{
    public class LevelPanelFill : MonoBehaviour
    {
        public GameObject LevelIconPrefab;
        public GameObject Navigate10MinusButton;
        public GameObject Navigate10PlusButton;

        private bool m_isFirstInitialisationRun = true;

        // Use this for initialization
        void Start()
        {
            Build();
            AttachButtonHandlers();
        }

        private void AttachButtonHandlers()
        {
            Button nav10Minus = Navigate10MinusButton.GetComponent<Button>();
            nav10Minus.onClick.AddListener(new UnityEngine.Events.UnityAction(() => Navigate10Minus()));

            Button nav10Plus = Navigate10PlusButton.GetComponent<Button>();
            nav10Plus.onClick.AddListener(new UnityAction(() => Navigate10Plus()));
        }

        private void Navigate10Plus()
        {
            LevelManager.CurrentLevel += 10;
            Build();
        }

        private void Navigate10Minus()
        {
            LevelManager.CurrentLevel -= 10;
            Build();
        }

        private void Build()
        {
            int levelRange = LevelManager.CurrentLevel / 10;
            
            if (levelRange == 0)
            {
                DeactivateNavigationButton(Navigate10MinusButton);
            }
            else
            {
                ActivateNavigationButton(Navigate10MinusButton);
            }

            if (levelRange >= (LevelManager.MaxLevels / 10) - 1)
            {
                DeactivateNavigationButton(Navigate10PlusButton);
            }
            else
            {
                ActivateNavigationButton(Navigate10PlusButton);
            }

            //GameObject panel = Instantiate<GameObject>(null);
            //panel.AddComponent<RectTransform>();
            //GridLayoutGroup group = panel.AddComponent<GridLayoutGroup>();

            int iconIndex = 0;
            for (int levelNumber = (levelRange * 10) + 1; levelNumber <= levelRange * 10 + 10; levelNumber++)
            {
                GameObject levelIcon = null;

                if (m_isFirstInitialisationRun)
                {
                    levelIcon = Instantiate(LevelIconPrefab);
                    levelIcon.transform.SetParent(gameObject.transform);
                    levelIcon.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    UnityEngine.UI.Button button = levelIcon.GetComponent<UnityEngine.UI.Button>();
                    button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => LoadLevel(levelIcon)));
                }
                else
                {
                    levelIcon = gameObject.transform.GetChild(iconIndex).gameObject;
                }

                Transform childTransform = levelIcon.transform.GetChild(0);
                UnityEngine.UI.Text text = childTransform.GetComponent<UnityEngine.UI.Text>();
                text.text = levelNumber.ToString();
                iconIndex++;
            }

            m_isFirstInitialisationRun = false;
        }

        private void LoadLevel(GameObject icon)
        {
            Text text = icon.transform.GetChild(0).gameObject.GetComponent<Text>();
            int levelNumber = int.Parse(text.text);
            LevelManager.LoadLevel(levelNumber);
        }

        private int m_initialFontSize;

        private void ActivateNavigationButton(GameObject navigateButton)
        {
            UnityEngine.UI.Button button = navigateButton.GetComponent<UnityEngine.UI.Button>();
            button.enabled = true;

            UnityEngine.UI.Text text = navigateButton.GetComponent<UnityEngine.UI.Text>();
            if (m_initialFontSize != 0)
            {
                text.fontSize = m_initialFontSize;
            }
            text.enabled = true;
        }

        private void DeactivateNavigationButton(GameObject navigateButton)
        {
            UnityEngine.UI.Button button = navigateButton.GetComponent<UnityEngine.UI.Button>();
            button.enabled = false;

            UnityEngine.UI.Text text = navigateButton.GetComponent<UnityEngine.UI.Text>();
            m_initialFontSize = text.fontSize;
            text.fontSize = 0;
            text.enabled = false;
        }
    }
}