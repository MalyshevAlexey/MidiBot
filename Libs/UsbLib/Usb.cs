using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;

namespace MidiBot.UsbLib
{
    public class Usb
    {
        UsbDevice device;
        UsbEndpointWriter writer;
        ErrorCode ec;
        int bytesWritten;

        private void GetDevice (int vid, int pid)
        {
            device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(vid, pid));
        }

        private void GetWriter()
        {
            writer = device.OpenEndpointWriter(WriteEndpointID.Ep01);
        }

        public Usb (int vid, int pid)
        {
            //UsbRegDeviceList allDevices = UsbDevice.AllDevices;
            //UsbRegistry push = allDevices.Find(new UsbDeviceFinder(vid, pid));
            //push.Open(out device);
            //IUsbDevice wholePush = device as IUsbDevice;

            //Console.WriteLine(usbRegistry.Name + " " + usbRegistry.IsAlive);

            ec = ErrorCode.None;
            GetDevice(vid, pid);
            GetWriter();
        }
        
        public void Write (byte[] frame, int timeout)
        {
            ec = writer.Write(frame, timeout, out bytesWritten);
        }
    }
    //[Flags]
    //public enum DICFG
    //{
    //    /// <summary>
    //    /// Return only the device that is associated with the system default device interface, if one is set, for the specified device interface classes. 
    //    ///  only valid with <see cref="DEVICEINTERFACE"/>.
    //    /// </summary>
    //    DEFAULT = 0x00000001,
    //    /// <summary>
    //    /// Return only devices that are currently present in a system. 
    //    /// </summary>
    //    PRESENT = 0x00000002,
    //    /// <summary>
    //    /// Return a list of installed devices for all device setup classes or all device interface classes. 
    //    /// </summary>
    //    ALLCLASSES = 0x00000004,
    //    /// <summary>
    //    /// Return only devices that are a part of the current hardware profile. 
    //    /// </summary>
    //    PROFILE = 0x00000008,
    //    /// <summary>
    //    /// Return devices that support device interfaces for the specified device interface classes. 
    //    /// </summary>
    //    DEVICEINTERFACE = 0x00000010,
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct SP_DEVINFO_DATA
    //{
    //    public static readonly SP_DEVINFO_DATA Empty = new SP_DEVINFO_DATA(Marshal.SizeOf(typeof(SP_DEVINFO_DATA)));

    //    public UInt32 cbSize;
    //    public Guid ClassGuid;
    //    public UInt32 DevInst;
    //    public IntPtr Reserved;

    //    private SP_DEVINFO_DATA(int size)
    //    {
    //        cbSize = (uint)size;
    //        ClassGuid = Guid.Empty;
    //        DevInst = 0;
    //        Reserved = IntPtr.Zero;
    //    }
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct SP_DEVICE_INTERFACE_DATA
    //{
    //    public static readonly SP_DEVICE_INTERFACE_DATA Empty = new SP_DEVICE_INTERFACE_DATA(Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA)));

    //    public UInt32 cbSize;
    //    public Guid interfaceClassGuid;
    //    public UInt32 flags;
    //    private UIntPtr reserved;

    //    private SP_DEVICE_INTERFACE_DATA(int size)
    //    {
    //        cbSize = (uint)size;
    //        reserved = UIntPtr.Zero;
    //        flags = 0;
    //        interfaceClassGuid = Guid.Empty;
    //    }
    //}

    //public class Usb
    //{
    //    const int ABLETON_VENDOR_ID = 0x2982;
    //    const int PUSH2_PRODUCT_ID = 0x1967;
    //    [DllImport("setupapi.dll", CharSet = CharSet.Ansi, EntryPoint = "SetupDiGetClassDevsA")]
    //    public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid,
    //                                                    [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
    //                                                    IntPtr hwndParent,
    //                                                    DICFG Flags);

    //    [DllImport("setupapi.dll", CharSet = CharSet.Ansi, EntryPoint = "SetupDiGetClassDevsA")]
    //    public static extern IntPtr SetupDiGetClassDevs(int ClassGuid, string Enumerator, IntPtr hwndParent, DICFG Flags);

    //    [DllImport("setupapi.dll", CharSet = CharSet.Ansi, EntryPoint = "SetupDiGetClassDevsA")]
    //    public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, int Enumerator, IntPtr hwndParent, DICFG Flags);

    //    [DllImport("setupapi.dll", SetLastError = true)]
    //    public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

    //    [DllImport("setupapi.dll", CharSet = CharSet.Unicode /*, SetLastError = true*/)]
    //    public static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

    //    [DllImport("hid.dll", EntryPoint = "HidD_GetHidGuid", SetLastError = true)]
    //    static extern void HidD_GetHidGuid(out Guid Guid);

    //    [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    //    public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo,
    //                                                             ref SP_DEVINFO_DATA devInfo,
    //                                                             ref Guid interfaceClassGuid,
    //                                                             int memberIndex,
    //                                                             ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

    //    [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    //    public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo,
    //                                                             [MarshalAs(UnmanagedType.AsAny)] object devInfo,
    //                                                             ref Guid interfaceClassGuid,
    //                                                             int memberIndex,
    //                                                             ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

    //    [DllImport(@"setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    //    public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo,
    //                                                                 ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
    //                                                                 DEVICE_INTERFACE_DETAIL_HANDLE deviceInterfaceDetailData,
    //                                                                 int deviceInterfaceDetailDataSize,
    //                                                                 out int requiredSize,
    //                                                                 [MarshalAs(UnmanagedType.AsAny)] object deviceInfoData);
    //    [DllImport(@"setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    //    public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo,
    //                                                                 ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
    //                                                                 DEVICE_INTERFACE_DETAIL_HANDLE deviceInterfaceDetailData,
    //                                                                 int deviceInterfaceDetailDataSize,
    //                                                                 out int requiredSize,
    //                                                                 ref SP_DEVINFO_DATA deviceInfoData);


    //    public static void EnumClassDevs()
    //    {
    //        Guid gHid = new Guid();
    //        HidD_GetHidGuid(out gHid);
    //        //gHid = new  Guid("88bae032-5a81-49f0-bc3d-a4ff138216d6");
    //        SP_DEVICE_INTERFACE_DATA dev_interface_data = SP_DEVICE_INTERFACE_DATA.Empty;
    //        IntPtr deviceInfo = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DICFG.PRESENT | DICFG.DEVICEINTERFACE);
    //        int devicePathIndex = 0;
    //        while ((SetupDiEnumDeviceInterfaces(deviceInfo, null, ref gHid, devicePathIndex, ref dev_interface_data)))
    //        {
    //            int length = 1024;
    //            DeviceInterfaceDetailHelper detailHelper = new DeviceInterfaceDetailHelper(length);
    //            SetupDiGetDeviceInterfaceDetail(deviceInfo, ref dev_interface_data, detailHelper.Handle, length, out length, null);
    //            Console.WriteLine(detailHelper.DevicePath);
    //            devicePathIndex++;
    //        }

    //        //DICFG flags = DICFG.ALLCLASSES | DICFG.PRESENT;
    //        //List<SP_DEVINFO_DATA> data = new List<SP_DEVINFO_DATA>();
    //        //SP_DEVINFO_DATA dev_info_data = SP_DEVINFO_DATA.Empty;
    //        //int dev_index = 0;
    //        //IntPtr dev_info = SetupDiGetClassDevs(0, null, IntPtr.Zero, flags);
    //        //while (SetupDiEnumDeviceInfo(dev_info, dev_index, ref dev_info_data))
    //        //{
    //        //    data.Add(dev_info_data);
    //        //    dev_index++;
    //        //    dev_info_data = SP_DEVINFO_DATA.Empty;
    //        //}
    //        //SetupDiDestroyDeviceInfoList(dev_info);
    //        //foreach (SP_DEVINFO_DATA item in data)
    //        //{
    //        //    if (item.ClassGuid == Guid.Parse("{88bae032-5a81-49f0-bc3d-a4ff138216d6}"))
    //        //        Console.WriteLine(item.ClassGuid + " " + item.DevInst);
    //        //}

    //        //Console.WriteLine(data.Count);
    //    }

    //    public class DeviceInterfaceDetailHelper
    //    {
    //        public static readonly int SIZE = Is64Bit ? 8 : 6;
    //        private IntPtr mpDevicePath;
    //        private IntPtr mpStructure;

    //        public DeviceInterfaceDetailHelper(int maximumLength)
    //        {
    //            mpStructure = Marshal.AllocHGlobal(maximumLength);
    //            mpDevicePath = new IntPtr(mpStructure.ToInt64() + Marshal.SizeOf(typeof(int)));
    //        }

    //        public DEVICE_INTERFACE_DETAIL_HANDLE Handle
    //        {
    //            get
    //            {
    //                Marshal.WriteInt32(mpStructure, SIZE);
    //                return new DEVICE_INTERFACE_DETAIL_HANDLE(mpStructure);
    //            }
    //        }

    //        public string DevicePath
    //        {
    //            get { return Marshal.PtrToStringUni(mpDevicePath); }
    //        }


    //        public void Free()
    //        {
    //            if (mpStructure != IntPtr.Zero)
    //                Marshal.FreeHGlobal(mpStructure);

    //            mpDevicePath = IntPtr.Zero;
    //            mpStructure = IntPtr.Zero;
    //        }


    //        ~DeviceInterfaceDetailHelper() { Free(); }
    //    }

    //    [StructLayout(LayoutKind.Sequential)]
    //    public struct DEVICE_INTERFACE_DETAIL_HANDLE
    //    {
    //        private IntPtr mPtr;

    //        internal DEVICE_INTERFACE_DETAIL_HANDLE(IntPtr ptrInit) { mPtr = ptrInit; }
    //    }

    //    public static bool Is64Bit
    //    {
    //        get { return (IntPtr.Size == 8); }
    //    }
    //}
}
