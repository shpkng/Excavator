using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Obi
{
    public class ObiEditorSettings : ScriptableObject
    {
        public const string m_ObiEditorSettingsPath = "Assets/ObiEditorSettings.asset";

        [SerializeField] private Color m_ParticleBrush;
        [SerializeField] private Color m_BrushWireframe;
        [SerializeField] private Color m_Particle;
        [SerializeField] private Color m_SelectedParticle;
        [SerializeField] private Color m_ActiveParticle;
        [SerializeField] private Gradient m_PropertyGradient;
        [SerializeField] private bool m_ParticlePicking;

        public Color brushColor
        {
            get { return m_ParticleBrush; }
        }
        public Color brushWireframeColor
        {
            get { return m_BrushWireframe; }
        }
        public Color particleColor
        {
            get { return m_Particle; }
        }
        public Color selectedParticleColor
        {
            get { return m_SelectedParticle; }
        }
        public Color activeParticleColor
        {
            get { return m_ActiveParticle; }
        }
        public Gradient propertyGradient
        {
            get { return m_PropertyGradient; }
        }
        public bool sceneViewParticlePicking
        {
            get { return m_ParticlePicking; }
        }

#if UNITY_EDITOR
        public static ObiEditorSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ObiEditorSettings>(m_ObiEditorSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ObiEditorSettings>();
                settings.m_ParticleBrush = new Color32(243, 77, 43, 255);
                settings.m_BrushWireframe = new Color32(0, 0, 0, 128);
                settings.m_Particle = new Color32(240, 240, 240, 255);
                settings.m_SelectedParticle = new Color32(243, 77, 43, 255);
                settings.m_ActiveParticle = new Color32(243, 243, 43, 255);
                settings.m_PropertyGradient = new Gradient();
                settings.m_ParticlePicking = true;

                // Populate the color keys at the relative time 0 and 1 (0 and 100%)
                var colorKey = new GradientColorKey[2];
                colorKey[0].color = Color.grey * 0.7f;
                colorKey[0].time = 0.0f;
                colorKey[1].color = Color.white;
                colorKey[1].time = 1.0f;

                // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
                var alphaKey = new GradientAlphaKey[2];
                alphaKey[0].alpha = 1.0f;
                alphaKey[0].time = 0.0f;
                alphaKey[1].alpha = 1.0f;
                alphaKey[1].time = 1.0f;

                settings.m_PropertyGradient.SetKeys(colorKey, alphaKey);

                AssetDatabase.CreateAsset(settings, m_ObiEditorSettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }


        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
#endif
    }
}