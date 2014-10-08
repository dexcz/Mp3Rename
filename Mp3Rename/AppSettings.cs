using System;
using System.Collections.Generic;
using System.Text;

namespace Mp3Rename
{
    public class AppSettings
    {
        private int inputSetting, outSetting;


        public AppSettings()
        {
            inputSetting = 0;
            outSetting = 0;
        }

        public int Input
        {
            get { return inputSetting; }
            set { inputSetting = value; }
        }

        public int Out
        {
            get { return outSetting; }
            set { outSetting = value; }
        }

    }
}
