using UnityEditor;
using UnityEngine;

namespace LogViewer
{
    public class LogViewerOnGUI
    {
        public void SetLog(LogFile logFile)
        {
            this.logFile = logFile;

            selectedLogIndex = -1;
            logListScrollPosition = Vector2.zero;
            logScrollPosition = Vector2.zero;
        }

        private LogFile logFile;

        private LogFile.EventTypes visibleEventTypes = LogFile.EventTypes.Message | LogFile.EventTypes.Error;
        private Vector2 logListScrollPosition, logScrollPosition;
        private int selectedLogIndex = -1;

        public void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            if (logFile != null)
            {
                Formatted();
            }
            else
            {
                Pending();
            }

            GUILayout.EndVertical();
        }

        private void Pending()
        {
            GUILayout.Label("Drop a <b>Player.log</b> here", Styles.CenteredMessage, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        }

        private void Formatted()
        {
            GUILayout.BeginHorizontal();

            if (DrawEventTypeToggle(LogFile.EventTypes.Message, logFile.MessageCounts[LogFile.EventTypes.Message], (visibleEventTypes & LogFile.EventTypes.Message) != 0))
                visibleEventTypes ^= LogFile.EventTypes.Message;

            GUILayout.Space(2);

            if (DrawEventTypeToggle(LogFile.EventTypes.Error, logFile.MessageCounts[LogFile.EventTypes.Error], (visibleEventTypes & LogFile.EventTypes.Error) != 0))
                visibleEventTypes ^= LogFile.EventTypes.Error;

            GUILayout.Space(2);

            if (DrawEventTypeToggle(LogFile.EventTypes.Culled, logFile.MessageCounts[LogFile.EventTypes.Culled], (visibleEventTypes & LogFile.EventTypes.Culled) != 0))
                visibleEventTypes ^= LogFile.EventTypes.Culled;

            GUILayout.EndHorizontal();

            logListScrollPosition = GUILayout.BeginScrollView(logListScrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            const float entryHeight = 40f;

            for (int i = 0; i < logFile.Events.Count; i++)
            {
                LogFile.Event log = logFile.Events[i];

                if ((log.EventType & visibleEventTypes) == 0)
                    continue;

                bool selected = selectedLogIndex == i;
                bool even = i % 2 == 0;

                if (DrawLogSummary(log, selected, even, entryHeight))
                {
                    selectedLogIndex = i;
                }               
            }

            GUILayout.EndScrollView();

            logScrollPosition = GUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(256));

            string selectedLogContents = string.Empty;

            if (selectedLogIndex != -1)
            {
                selectedLogContents = logFile.Events[selectedLogIndex].Content;
            }

            GUILayout.TextArea(selectedLogContents, Styles.MessageStyle);
            GUILayout.EndScrollView();
        }

        private static bool DrawLogSummary(LogFile.Event log, bool selected, bool even, float height)
        {
            GUIStyle style;

            if (selected)
                style = Styles.SelectedBackground;
            else
                style = even ? Styles.EvenBackground : Styles.OddBackground;

            GUILayout.BeginHorizontal(style, GUILayout.Height(height));

            GUIContent iconContent = log.EventType switch
            {
                LogFile.EventTypes.Message => Styles.iconInfo,
                LogFile.EventTypes.Error => Styles.iconError,
                _ => Styles.iconCulled
            };

            bool clicked = GUILayout.Button(iconContent, Styles.IconStyle);

            clicked = GUILayout.Button(log.Summary, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)) | clicked;

            GUILayout.EndHorizontal();

            return clicked;
        }

        private static bool DrawEventTypeToggle(LogFile.EventTypes eventType, int count, bool selected)
        {
            GUIStyle style = selected ? Styles.MiniButtonSelected : Styles.MiniButton;

            Texture texture = eventType switch
            {
                LogFile.EventTypes.Message => Styles.iconInfo.image,
                LogFile.EventTypes.Error => Styles.iconError.image,
                _ => Styles.iconCulled.image
            };

            return GUILayout.Button(
                new GUIContent(count.ToString(), texture),
                style,
                GUILayout.MinWidth(42),
                GUILayout.ExpandWidth(false));
        }
    }
}
