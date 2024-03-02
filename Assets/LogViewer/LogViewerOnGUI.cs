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

        private LogFile.EventTypes visibleEventTypes = (LogFile.EventTypes)~0;
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

            int messageCount = logFile.MessageCount;
            int errorCount = logFile.ErrorCount;

            GUIStyle messageStyle = (visibleEventTypes & LogFile.EventTypes.Message) != 0 ? Styles.MiniButtonSelected : Styles.MiniButton;
            GUIStyle errorStyle = (visibleEventTypes & LogFile.EventTypes.Error) != 0 ? Styles.MiniButtonSelected : Styles.MiniButton;

            if (GUILayout.Button(new GUIContent(messageCount.ToString(), Styles.iconInfo.image), messageStyle, GUILayout.MinWidth(42), GUILayout.ExpandWidth(false)))
                visibleEventTypes ^= LogFile.EventTypes.Message;

            GUILayout.Space(2);

            if (GUILayout.Button(new GUIContent(errorCount.ToString(), Styles.iconError.image), errorStyle, GUILayout.MinWidth(42), GUILayout.ExpandWidth(false)))
                visibleEventTypes ^= LogFile.EventTypes.Error;

            GUILayout.EndHorizontal();

            logListScrollPosition = GUILayout.BeginScrollView(logListScrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            const float entryHeight = 40f;

            int startingIndex = Mathf.FloorToInt(logListScrollPosition.y / entryHeight);

            int visibleLogs = 0;

            for (int i = 0; i < logFile.Events.Count; i++)
            {
                LogFile.Event log = logFile.Events[i];

                if ((log.EventType & visibleEventTypes) == 0)
                    continue;

                if (i < startingIndex || i > startingIndex + 20)
                {
                    visibleLogs++;
                    continue;
                }
                else if (i == startingIndex)
                {
                    GUILayout.Space(visibleLogs * entryHeight);
                    visibleLogs = 0;
                }

                bool selected = selectedLogIndex == i;
                bool even = i % 2 == 0;

                if (DrawLogSummary(log, selected, even, entryHeight))
                {
                    selectedLogIndex = i;
                }               
            }

            GUILayout.Space(entryHeight * visibleLogs);

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

            bool clicked = false;

            if (log.EventType == LogFile.EventTypes.Message)
                clicked = GUILayout.Button(Styles.iconInfo, Styles.IconStyle) | clicked;
            else
                clicked = GUILayout.Button(Styles.iconError, Styles.IconStyle) | clicked;

            clicked = GUILayout.Button(log.Summary, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)) | clicked;

            GUILayout.EndHorizontal();

            return clicked;
        }
    }
}
