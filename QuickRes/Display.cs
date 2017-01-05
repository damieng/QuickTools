using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QuickRes
{
    class Res : IEquatable<Res>
    {
        readonly int hashCode;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int? Frequency { get; private set; }

        public Res(int width, int height, int? frequency)
        {
            Width = width;
            Height = height;
            Frequency = frequency;
            hashCode = CreateHashCode();
        }

        public override String ToString()
        {
            return Frequency.HasValue ? $"{Width} × {Height} @ {Frequency}Hz" : $"{Width} × {Height}";
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Res && Equals((Res) obj);
        }

        public bool Equals(Res other)
        {
            return other.Width == Width && other.Height == Height && other.Frequency == Frequency;
        }

        public override Int32 GetHashCode()
        {
            return hashCode;
        }

        private int CreateHashCode()
        {
            int hash = 17;
            hash = hash*31 + Width.GetHashCode();
            hash = hash*31 + Height.GetHashCode();
            hash = hash*31 + (Frequency.HasValue ? Frequency.Value : 0).GetHashCode();
            return hash;
        }
    }

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

        public Res GetCurrentResolution()
        {
            var mode = new DEVMODE();
            if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref mode))
            {
                return new Res(mode.dmPelsWidth, mode.dmPelsHeight, mode.dmDisplayFrequency);
            }
            else
            {
                throw new Exception("Unable to obtain current display mode");
            }
        }

        public List<Res> GetResolutions()
        {
            return resolutions
            .GroupBy(g => new { g.dmPelsWidth, g.dmPelsHeight })
            .Select(r => new Res(r.Key.dmPelsWidth, r.Key.dmPelsHeight, r.Max(g => g.dmDisplayFrequency)))
            .OrderBy(o => o.Width)
            .ToList();
        }

        private List<DEVMODE> resolutions = new List<DEVMODE>();

        public bool SetResolution(Res res)
        {
            var bestMatch = resolutions
                .OrderByDescending(r => r.dmDisplayFrequency)
                .FirstOrDefault(r => r.dmPelsWidth == res.Width && r.dmPelsHeight == res.Height && (!res.Frequency.HasValue || res.Frequency.Value == r.dmDisplayFrequency));

            return ChangeDisplaySettings(ref bestMatch, CDS_UPDATEREGISTRY) == DISP_CHANGE_SUCCESSFUL;
        }
    }
}