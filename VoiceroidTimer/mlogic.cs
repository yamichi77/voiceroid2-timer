using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceroidTimer
{
    class mlogic
    {
        Voiceroid2 voiceroid2 = new Voiceroid2();
        private DateTime yDt = new DateTime(1800/1/1);
        private bool sayEnd = false;
        public bool init()
        {
            return(voiceroid2.init());
        }
        public void MLogic(string[][] inputList)
        {
            DateTime dateTime = DateTime.Now;
            sayEnd = false;
            if (yDt.Date < dateTime.Date)
            {
                if(inputList != null)
                {
                    inputDayMatch(dateTime.Date, inputList);
                }
                if (!sayEnd)
                {
                    voiceroid2.say("今日は" + dateTime.ToString("MM月dd日") + "です。");
                }
                yDt = dateTime.Date;
                sayDelay();
                
            }else if(dateTime.Second.Equals(0))
            {
                if (inputList != null)
                {
                    inputTimeMatch(dateTime, inputList);
                }         
                if (!sayEnd && dateTime.Minute % 10 == 0)
                {
                    voiceroid2.say(dateTime.Hour.ToString() + "時" + dateTime.Minute.ToString() + "分です。");
                }
                sayDelay();
            }
        }
        private async void sayDelay()
        {
            await Task.Delay(5000);
        }
        private void inputDayMatch(DateTime dateTime, string[][] inputList)
        {
            string dayFormat = "MM/dd";
            DateTime inputTime;
            foreach (string[] strs in inputList)
            {
                if (DateTime.TryParseExact(strs[0], dayFormat, null, System.Globalization.DateTimeStyles.None, out inputTime))
                {
                    if(dateTime == inputTime)
                    {
                        voiceroid2.say(strs[1]);
                        sayEnd = true;
                        break;
                    }
                }
            }
        }
        private void inputTimeMatch(DateTime dateTime, string[][] inputList)
        {
            string hourFormat = "HH:mm";
            DateTime inputTime;
            foreach (string[] strs in inputList)
            {
                if (DateTime.TryParseExact(strs[0], hourFormat, null, System.Globalization.DateTimeStyles.None, out inputTime))
                {
                    if (inputTime.ToShortTimeString() == dateTime.ToShortTimeString())
                    {
                        voiceroid2.say(strs[1]);
                        sayEnd = true;
                        break;
                    }
                }
            }
        }
        public string[] getCharacter()
        {
            return voiceroid2.getCharacter();
        }
        public void setCharacter(int avatorIdx)
        {
            voiceroid2.setCharacter(avatorIdx);
        }
    }
}
