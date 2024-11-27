using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WPFNotepad
{
    internal class ConfigurationHelper
    {
        public static readonly int DefaultZoomLevel;
        public static readonly int MinZoomLevel;
        public static readonly int MaxZoomLevel;

        static ConfigurationHelper()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["DefaultZoomLevel"], out DefaultZoomLevel))
            {
                DefaultZoomLevel = 100; // Default value
            }
            if (!int.TryParse(ConfigurationManager.AppSettings["MinZoomLevel"], out MinZoomLevel))
            {
                MinZoomLevel = 50; // Default value
            }
            if (!int.TryParse(ConfigurationManager.AppSettings["MaxZoomLevel"], out MaxZoomLevel))
            {
                MaxZoomLevel = 200; // Default value
            }
        }
    }
}