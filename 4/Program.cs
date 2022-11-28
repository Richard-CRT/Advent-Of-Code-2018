using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    public enum LogTypes { fallsAsleep, wakesUp, beginShift };

    class Program
    {

        static LogEntry ParseLogEntry(string logEntryString)
        {
            int month = Int32.Parse(logEntryString.Substring(6, 2));
            int day = Int32.Parse(logEntryString.Substring(9, 2));
            int hour = Int32.Parse(logEntryString.Substring(12, 2));
            int minute = Int32.Parse(logEntryString.Substring(15, 2));

            string text = logEntryString.Substring(19, logEntryString.Length - 19);
            if (text == "falls asleep")
            {
                return new LogEntry(month, day, hour, minute, logEntryString, LogTypes.fallsAsleep);
            }
            else if (text == "wakes up")
            {
                return new LogEntry(month, day, hour, minute, logEntryString, LogTypes.wakesUp);
            }
            else
            {
                int guard = Int32.Parse(text.Substring(7, text.Length - 7 - 13));
                return new LogEntry(month, day, hour, minute, logEntryString, LogTypes.beginShift, guard);
            }
        }

        static void Main(string[] args)
        {
            var logFile = File.ReadAllLines("input.txt");
            var logList = new List<string>(logFile);

            List<LogEntry> logEntries = new List<LogEntry>();
            for (int i = 0; i < logList.Count; i++)
            {
                logEntries.Add(ParseLogEntry(logList[i]));
            }
            logEntries = logEntries.OrderBy(entry => entry.Month).ThenBy(entry => entry.Day).ThenBy(entry => entry.Hour).ThenBy(entry => entry.Minute).ToList();

            Dictionary<int, int[]> GuardRecord = new Dictionary<int, int[]>();

            int currentGuard = -1;
            int lastFallAsleep = -1;
            
            for (int i = 0; i < logEntries.Count; i++)
            {
                LogEntry logEntry = logEntries[i];

                Console.WriteLine("{0}", logEntry.OriginalText);

                if (logEntry.LogType == LogTypes.beginShift)
                {
                    currentGuard = logEntry.Guard;
                }
                else if (logEntry.LogType == LogTypes.fallsAsleep)
                {
                    lastFallAsleep = logEntry.Minute;
                }
                else if (logEntry.LogType == LogTypes.wakesUp)
                {
                    if (!GuardRecord.ContainsKey(currentGuard))
                    {
                        GuardRecord[currentGuard] = new int[60];
                        Array.Clear(GuardRecord[currentGuard], 0, 60);
                    }

                    for (int n = lastFallAsleep; n < logEntry.Minute; n++)
                    {
                        GuardRecord[currentGuard][n]++;
                    }
                }
            }

            List<KeyValuePair<int, int[]>> GuardRecordList = GuardRecord.OrderByDescending(guard => guard.Value.Sum()).ToList();

            Console.Write("#      ");
            for (int i = 0; i < 60; i ++ )
            {
                Console.Write("{0} ", i.ToString().PadLeft(2, '0'));
            }
            Console.WriteLine();
            foreach (KeyValuePair<int, int[]> guard in GuardRecordList)
            {
                Console.Write("#{0}", guard.Key.ToString().PadRight(6, ' '));
                for (int i = 0; i < 60; i++)
                {
                    Console.Write("{0} ", guard.Value[i].ToString().PadLeft(2, '0'));
                }
                Console.WriteLine();
            }
            



            Console.ReadLine();
        }
    }

    public class LogEntry
    {
        public int Month;
        public int Day;
        public int Hour;
        public int Minute;

        public string OriginalText;

        public LogTypes LogType;

        public int Guard;

        public LogEntry(int month, int day, int hour, int minute, string originalText, LogTypes logType, int guard = -1)
        {
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            OriginalText = originalText;
            LogType = logType;
            Guard = guard;
        }
    }
}
