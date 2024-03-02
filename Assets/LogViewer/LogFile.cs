using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LogViewer
{
    public class LogFile
    {
        [Flags]
        public enum EventTypes 
        { 
            None = 0,
            Message = 1,
            Error = 2,
            Culled = 4,
        }

        public class Event
        {
            public readonly string Summary, Content;
            public readonly EventTypes EventType;

            private static readonly Regex regexEmptyIL2CPPLines = new(@"\[ line -\d+\]");

            public Event(string content)
            {
                Content = content.Trim(Environment.NewLine.ToCharArray());

                string[] splits = Content.Split(Environment.NewLine);

                Summary = splits[0];

                if (splits.Length > 1)
                    Summary += $"\n{splits[1]}";

                if (string.IsNullOrEmpty(Content) ||
                    regexEmptyIL2CPPLines.IsMatch(Content))
                {
                    EventType = EventTypes.Culled;
                }
                else if (Content.Contains("Exception"))
                {
                    EventType = EventTypes.Error;
                }
                else
                {
                    EventType = EventTypes.Message;
                }
            }
        }

        public readonly string Raw;

        public IReadOnlyDictionary<EventTypes, int> MessageCounts => messageCounts;
        private readonly Dictionary<EventTypes, int> messageCounts = new();        

        public IReadOnlyList<Event> Events => events;
        private readonly List<Event> events = new();

        public static LogFile LoadFromFile(string filepath)
        {
            string raw = File.ReadAllText(filepath);

            return new LogFile(raw);
        }

        public LogFile(string raw)
        {
            Raw = raw;

            foreach (EventTypes value in Enum.GetValues(typeof(EventTypes)))
                messageCounts[value] = 0;

            string[] splits = raw.Split($"{Environment.NewLine}{Environment.NewLine}");

            foreach (string split in splits)
            {
                Event logEvent = new(split);

                events.Add(logEvent);

                messageCounts[logEvent.EventType]++;
            }
        }
    }
}