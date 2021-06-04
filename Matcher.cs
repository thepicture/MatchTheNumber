using System;
using System.Collections.Generic;
using System.Text;

namespace NumberGuesser
{
    class Matcher
    {
        private ulong number;
        private ulong infimum = ulong.MinValue;
        private ulong supremum;
        private readonly StringBuilder currentMessage = new StringBuilder();

        public Matcher(ulong supremum)
        {
            this.supremum = supremum + 1;
            UpdateState();
        }

        private readonly List<Tuple<ulong, LoggerEnum>> logList = new List<Tuple<ulong, LoggerEnum>>();

        /// <summary>
        /// A default logger for checking if the user is lying or not.
        /// </summary>
        /// <param name="status">Sets a status of the logger element.</param>
        public void PushLog(LoggerEnum status)
        {
            if (status.Equals(LoggerEnum.GREATER))
            {
                logList.Add(new Tuple<ulong, LoggerEnum>((supremum + infimum) / 2, status));
            }
            else if (status.Equals(LoggerEnum.LESS))
            {
                logList.Add(new Tuple<ulong, LoggerEnum>((supremum + infimum) / 2, status));
            }
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
                SayYes();
            }
            currentMessage.Clear().Append("Is this number " + (supremum + infimum) / 2 + "?");
        }

        public void SayYes()
        {
            number = (supremum + infimum) / 2;
            currentMessage.Clear().Append("The number is " + number + "\nThe count of questions is " + logList.Count);

            logList.ForEach(l =>
            {
                if (l.Item2 == LoggerEnum.GREATER && l.Item1 > number)
                {
                    currentMessage.Append("\nFalse that " + l.Item1 + " is greater than " + number);
                }
                else if (l.Item2 == LoggerEnum.LESS && l.Item1 < number)
                {
                    currentMessage.Append("\nFalse that " + l.Item1 + " is less than " + number);
                }
            });
        }
    }
}
