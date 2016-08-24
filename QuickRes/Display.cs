using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QuickRes
{
    class Display
    {
        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        [StructLayout(LayoutKind.Sequential)]
        struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            string dmDeviceName;
            short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        const int ENUM_CURRENT_SETTINGS = -1;
        const int CDS_UPDATEREGISTRY = 0x01;
        const int CDS_TEST = 0x02;
        const int DISP_CHANGE_SUCCESSFUL = 0;
        const int DISP_CHANGE_RESTART = 1;
        const int DISP_CHANGE_FAILED = -1;

        public Display()
        {
            SystemEvents.DisplaySettingsChanged += SystemEventsOnDisplaySettingsChanged;
            RefreshResolutionList();
        }

        private void SystemEventsOnDisplaySettingsChanged(Object sender, EventArgs eventArgs)
        {
            RefreshResolutionList();
        }

        public event EventHandler OnResolutionsChanged;

        private void RefreshResolutionList()
        {
            var newResolutionList = new List<DEVMODE>();
            var mode = new DEVMODE();
            var i = 0;
            while (EnumDisplaySettings(null, i++, ref mode))
            {
                newResolutionList.Add(mode);
                mode = new DEVMODE();
            }
            resolutions = newResolutionList.Where(r => r.dmDisplayFrequency >= 60).ToList();
                        
            OnResolutionsChanged?.Invoke(this, EventArgs.Empty);
        }

        public List<Tuple<int, int, int>> GetResolutions()
        {
            return resolutions
            .GroupBy(g => new { g.dmPelsWidth, g.dmPelsHeight })
            .Select(r => Tuple.Create(r.Key.dmPelsWidth, r.Key.dmPelsHeight, r.Max(g => g.dmDisplayFrequency)))
            .OrderBy(o => o.Item1)
            .ToList();
        }

        private List<DEVMODE> resolutions = new List<DEVMODE>();

        public bool SetResolution(int width, int height, int? frequency)
        {
            var bestMatch = resolutions
                .OrderByDescending(r => r.dmDisplayFrequency)
                .FirstOrDefault(r => r.dmPelsWidth == width && r.dmPelsHeight == height && (!frequency.HasValue || frequency.Value == r.dmDisplayFrequency));

            return ChangeDisplaySettings(ref bestMatch, CDS_UPDATEREGISTRY) == DISP_CHANGE_SUCCESSFUL;
        }
    }
}