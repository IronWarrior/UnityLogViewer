using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LogViewer
{
    public static class Styles
    {
        public static GUIStyle MiniButton;
        public static GUIStyle MiniButtonSelected;
        public static GUIStyle LogStyle;
        public static GUIStyle IconStyle;
        public static GUIStyle EvenBackground;
        public static GUIStyle OddBackground;
        public static GUIStyle SelectedBackground;
        public static GUIStyle MessageStyle;
        public static GUIStyle CenteredMessage;

        public static GUIContent iconInfo, iconError;

        public static bool Initialized { get; private set; }

        private static readonly List<Texture> textures = new();

        public static void Initialize()
        {
            iconInfo = new GUIContent((Texture)Resources.Load("d_console.infoicon@2x"));
            iconError = new GUIContent((Texture)Resources.Load("d_console.erroricon@2x"));

            Color textColor = new(0.824f, 0.824f, 0.824f);

            MiniButton = new GUIStyle()
            {
                border = new RectOffset(6, 6, 0, 0),
                padding = new RectOffset(5, 5, 2, 2),
                fixedHeight = 20
            };

            MiniButton.normal.textColor = textColor;

            MiniButtonSelected = CopyStyleWithColor(MiniButton, new Color32(92, 92, 92, 255));

            IconStyle = new GUIStyle()
            {
                fixedHeight = 32,
                fixedWidth = 32,
                margin = new RectOffset(5, 5, 5, 5)
            };

            LogStyle = new GUIStyle()
            {
                border = new RectOffset(30, 3, 3, 3),
                fixedHeight = 33,
                fontSize = 12,
                margin = new RectOffset(10, 10, 10, 10),
                padding = new RectOffset(32, 0, 3, 0)
            };

            EvenBackground = new GUIStyle()
            {
                border = new RectOffset(30, 3, 3, 3),
                alignment = TextAnchor.MiddleLeft,
            };

            EvenBackground.normal.background = MakeTexture(2, 2, new Color32(63, 63, 63, 255));
            EvenBackground.normal.textColor = textColor;

            OddBackground = CopyStyleWithColor(EvenBackground, new Color32(56, 56, 56, 255));
            SelectedBackground = CopyStyleWithColor(OddBackground, new Color32(44, 93, 135, 255));

            MessageStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 3, 3),
                fontSize = 13,
                padding = new RectOffset(10, 10, 10, 10),
                wordWrap = true
            };

            MessageStyle.normal.textColor = textColor;

            CenteredMessage = new GUIStyle(MessageStyle)
            {
                alignment = TextAnchor.MiddleCenter
            };

            Initialized = true;
        }

        public static void Deinitialize()
        {
            foreach (Texture t in textures)
            {
                Object.DestroyImmediate(t);
            }

            textures.Clear();

            Initialized = false;
        }

        private static GUIStyle CopyStyleWithColor(GUIStyle target, Color color)
        {
            GUIStyle cloned = new(target);
            cloned.normal.background = MakeTexture(2, 2, color);

            return cloned;
        }

        private static Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = color;
            }

            Texture2D result = new(width, height);
            result.SetPixels(pix);
            result.Apply();

            textures.Add(result);

            return result;
        }
    }
}