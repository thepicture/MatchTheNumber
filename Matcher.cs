using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MatchTheNumber
{
    class Matcher
    {
        static public ulong MAX_VALUE;
        private readonly ulong number;
        private ulong infimum = ulong.MinValue;
        private ulong supremum;
        private readonly StringBuilder currentMessage = new StringBuilder();

        public Matcher(ulong number)
        {
            this.number = number;
            supremum = MAX_VALUE + 1;
            UpdateState();
        }

        private readonly List<Tuple<ulong, bool, LoggerEnum>> logList = new List<Tuple<ulong, bool, LoggerEnum>>();

        /// <summary>
        /// A default logger for checking if the user is lying or not.
        /// </summary>
        /// <param name="status">Sets a status of the logger element.</param>
        public void PushLog(LoggerEnum status)
        {
            if (status.Equals(LoggerEnum.GREATER))
            {
                logList.Add(new Tuple<ulong, bool, LoggerEnum>((supremum + infimum) / 2, (supremum + infimum) / 2 < number, status));
            }
            else if (status.Equals(LoggerEnum.LESS))
            {
                logList.Add(new Tuple<ulong, bool, LoggerEnum>((supremum + infimum) / 2, (supremum + infimum) / 2 > number, status));
            }
            else if (status.Equals(LoggerEnum.EQUAL))
            {
                logList.Add(new Tuple<ulong, bool, LoggerEnum>((supremum + infimum) / 2, (supremum + infimum) / 2 == number, status));
            }
        }

        /// <summary>
        /// Actions for getting a lying info about the user through the game.
        /// </summary>
        public string GetLog()
        {
            return "The count of false answers: "
                + logList.Where(l => !l.Item2).Count()
                + "\nThe count of true answers:"
                + logList.Where(l => l.Item2).Count();
        }

        /// <summary>
        /// Actions for getting a message from the class object.
        /// </summary>
        /// <returns>A message.</returns>
        public string GetMessage()
        {
            return currentMessage.ToString();
        }

        public void SayGreater()
        {
            PushLog(LoggerEnum.GREATER);
            infimum = (supremum + infimum) / 2;
            UpdateState();
        }

        public void SayLess()
        {
            PushLog(LoggerEnum.LESS);
            supremum = (supremum + infimum) / 2;
            UpdateState();
        }

        /// <summary>
        /// Checking if the supremum equals to the infimum and updates them according to the logger.
        /// </summary>
        private void UpdateState()
        {
            if (infimum == supremum)
            {
                MessageBox.Show("The program will be closed due to the count of false answers.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                App.Current.Shutdown();
            }
            currentMessage.Clear().Append("Is this number " + (supremum + infimum) / 2 + "?");
        }

        internal void SayYes()
        {
            PushLog(LoggerEnum.EQUAL);
            currentMessage.Clear().Append("The number is : " + logList.Count + "\n" + GetLog());
            logList.ForEach(l =>
            {
                if (!l.Item2)
                {
                    if (l.Item3 == LoggerEnum.GREATER)
                        currentMessage.Append("\nFalse that " + l.Item1 + " is greater than " + number);
                    else if (l.Item3 == LoggerEnum.LESS)
                        currentMessage.Append("\nFalse that " + l.Item1 + " is less than " + number);
                    else if (l.Item3 == LoggerEnum.EQUAL)
                        currentMessage.Append("\nFalse that " + l.Item1 + " is equal to " + number);
                }
            });
        }
    }
}
