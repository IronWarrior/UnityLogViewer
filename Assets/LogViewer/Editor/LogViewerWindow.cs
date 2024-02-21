using System.IO;
using UnityEditor;
using UnityEngine;

namespace LogViewer
{
    public class LogViewerWindow : EditorWindow
    {
        [MenuItem("Window/Analysis/Log Viewer")]
        public static void Open()
        {
            GetWindow<LogViewerWindow>("Log Viewer");
        }

        private LogViewerOnGUI viewer;

        private void OnEnable()
        {
            Styles.Initialize();

            viewer = new LogViewerOnGUI();
        }

        private void OnDisable()
        {
            Styles.Deinitialize();
        }

        private void OnGUI()
        {
            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.DragPerform || currentEvent.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (currentEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (string path in DragAndDrop.paths)
                    {
                        if (File.Exists(path))
                        {
                            viewer.SetLog(LogFile.LoadFromFile(path));
                            titleContent = new GUIContent(Path.GetFileName(path));
                            break;
                        }
                    }
                }
            }

            viewer.OnGUI();
        }
    }
}
