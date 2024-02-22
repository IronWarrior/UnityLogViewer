#if !UNITY_EDITOR
using B83.Win32;
using System.IO;
#endif
using System.IO;
using UnityEngine;

namespace LogViewer
{
    public class LogViewerPlayer : MonoBehaviour
    {
#if UNITY_EDITOR
        public string testLogFilePath;
#endif

        private LogViewerOnGUI logViewer;

        private void Start()
        {
            logViewer = new LogViewerOnGUI();

#if UNITY_EDITOR
            if (File.Exists(testLogFilePath))
            {
                logViewer.SetLog(LogFile.LoadFromFile(testLogFilePath));
            }
#else
            UnityDragAndDropHook.InstallHook();
            UnityDragAndDropHook.OnDroppedFiles += OnDroppedFiles;

            string[] args = System.Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                string path = args[1];

                if (File.Exists(path))
                {
                    logViewer.SetLog(LogFile.LoadFromFile(path));
                }
            }
#endif
        }

#if !UNITY_EDITOR
        private void OnDroppedFiles(System.Collections.Generic.List<string> paths, POINT aDropPoint)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    logViewer.SetLog(LogFile.LoadFromFile(path));
                    // titleContent = new GUIContent(Path.GetFileName(path));

                    break;
                }
            }
        }

        private void OnDestroy()
        {
            UnityDragAndDropHook.UninstallHook();
        }
#endif

        private void OnGUI()
        {
            if (!Styles.Initialized)
                Styles.Initialize();

            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

            logViewer.OnGUI();

            GUILayout.EndArea();
        }
    }
}
