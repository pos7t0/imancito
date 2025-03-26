using AideTool.ExtendedEditor;
using AideTool.Extensions;
using UnityEditor;
using UnityEngine;

namespace AideTool
{
    [DisallowMultipleComponent, ComponentHeader(false)]
    public sealed class InspectorAide : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, InspectorValidator] private InspectorAideBehaviour m_behaviour = InspectorAideBehaviour.Default;
        public InspectorAideBehaviour Behaviour { get { return m_behaviour; } set { m_behaviour = value; } }

        #region SameBehaviour
        [SerializeField, InspectorName("Background")] private Color32 m_backgroundColor = AideColors.Gray;
        public Color32 BackgroundColor { get { return m_backgroundColor; } set { m_backgroundColor = value; } }
        [SerializeField, InspectorName("Text")] private Color32 m_textColor = AideColors.White;
        public Color TextColor { get { return m_textColor; } set { m_textColor = value; } }
        #endregion

        #region DetailedBehaviour
        [Foldout("Background"), SerializeField, InspectorName("Normal")] private Color32 m_backgroundNormalColor = AideColors.Gray;
        public Color32 BackgroundNormalColor { get { return m_backgroundNormalColor; } set { m_backgroundNormalColor = value; } }
        [SerializeField, InspectorName("Selected")] private Color32 m_backgroundSelectedColor = AideColors.Blue;
        public Color32 BackgroundSelectedColor { get { return m_backgroundSelectedColor; } set { m_backgroundSelectedColor = value; } }
        [EndFoldout, SerializeField, InspectorName("Hover")] private Color32 m_backgroundHoverColor = AideColors.Orange;
        public Color32 BackgroundHoverColor { get { return m_backgroundHoverColor; } set { m_backgroundHoverColor = value; } }

        [Foldout("Text"), SerializeField, InspectorName("Normal")] private Color32 m_textNormalColor = AideColors.Dark;
        public Color32 TextNormalColor { get { return m_textNormalColor; } set { m_textNormalColor = value; } }
        [SerializeField, InspectorName("Selected")] private Color32 m_textSelectedColor = AideColors.White;
        public Color32 TextSelectedColor { get { return m_textSelectedColor; } set { m_textSelectedColor = value; } }
        [EndFoldout, SerializeField, InspectorName("Hover")] private Color32 m_textHoverColor = AideColors.White;
        public Color32 TextHoverColor { get { return m_textHoverColor; } set { m_textHoverColor = value; } }
        #endregion

        [HideField(nameof(m_backgroundColor), nameof(m_textColor))]
        private bool ShowSameBehaviour() { return m_behaviour == InspectorAideBehaviour.Same; }
        
        [HideField(nameof(m_backgroundNormalColor), nameof(m_backgroundSelectedColor), nameof(m_backgroundHoverColor), nameof(m_textNormalColor), nameof(m_textHoverColor), nameof(m_textSelectedColor))]
        private bool ShowDetailedBehaviour() { return m_behaviour == InspectorAideBehaviour.Detailed; }

        private void OnValidate()
        {
            EditorApplication.RepaintHierarchyWindow();
        }
#endif
    }
}
