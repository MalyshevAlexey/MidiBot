using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using MonoLibUsb;
using MonoLibUsb.Transfer;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;

namespace MidiBot.UsbLib
{

    public class Usb
    {
        const short ABLETON_VENDOR_ID = 0x2982;
        const short PUSH2_PRODUCT_ID = 0x1967;
        const byte PUSH2_BULK_EP_OUT = 0x01;
        const int PUSH2_TRANSFER_TIMEOUT = 1000;

        const int PUSH2_DISPLAY_WIDTH = 960;
        const int PUSH2_DISPLAY_HEIGHT = 160;
        const int PUSH2_DISPLAY_LINE_BUFFER_SIZE = 2048;
        const int PUSH2_DISPLAY_LINE_GUTTER_SIZE = 128;
        const int PUSH2_DISPLAY_MESSAGE_BUFFER_SIZE = 16384;
        const int PUSH2_DISPLAY_IMAGE_BUFFER_SIZE = PUSH2_DISPLAY_LINE_BUFFER_SIZE * PUSH2_DISPLAY_HEIGHT;
        const int PUSH2_DISPLAY_MESSAGES_PER_IMAGE = (PUSH2_DISPLAY_LINE_BUFFER_SIZE * PUSH2_DISPLAY_HEIGHT) / PUSH2_DISPLAY_MESSAGE_BUFFER_SIZE;

        const byte PUSH2_DISPLAY_SHAPING_PATTERN_1 = 0xE7;
        const byte PUSH2_DISPLAY_SHAPING_PATTERN_2 = 0xF3;
        const byte PUSH2_DISPLAY_SHAPING_PATTERN_3 = 0xE7;
        const byte PUSH2_DISPLAY_SHAPING_PATTERN_4 = 0xFF;

        const int PUSH2_DISPLAY_FRAMERATE = 60;

        byte[] frame_header =
        {
            0xff, 0xcc, 0xaa, 0x88,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };

        Bitmap bmp;
        Graphics g;
        Pen pen;
        object locker = new object();
        ushort[] xOrMasks = { 0xf3e7, 0xffe7 };

        public Usb ()
        {
            
            bmp = new Bitmap(PUSH2_DISPLAY_WIDTH, PUSH2_DISPLAY_HEIGHT, PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.DarkRed);
            g.Clear(Color.White);
            g.Flush();
            Thread display = new Thread(Display);
            display.IsBackground = true;
            display.Start();
            int total = 180;
            for (float coef = 1; coef < 1800; coef += 0.02f)
            {
                lock (locker)
                {
                    g.Clear(Color.White);
                    for (int n = 0; n < total; n++)
                        ArcLine(n * 2, n * 2 * coef);
                    g.Flush();
                }
                Thread.Sleep(10);
            }
        }
        int zoom = 50;
        private void ArcLine(float alfa, float beta)
        {
            float x1 = zoom + (float)Math.Cos(alfa / 180.0 * Math.PI) * zoom;
            float y1 = zoom - (float)Math.Sin(alfa / 180.0 * Math.PI) * zoom;
            float x2 = zoom + (float)Math.Cos(beta / 180.0 * Math.PI) * zoom;
            float y2 = zoom - (float)Math.Sin(beta / 180.0 * Math.PI) * zoom;
            g.DrawLine(pen, x1, y1, x2, y2);
        }

        private void Display()
        {
            UsbDevice device;
            device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(ABLETON_VENDOR_ID, PUSH2_PRODUCT_ID));
            Console.WriteLine(device.UsbRegistryInfo.FullName);
            UsbEndpointWriter writer = device.OpenEndpointWriter(WriteEndpointID.Ep01);
            int bytesWritten;
            ErrorCode ec = ErrorCode.None;
            while (true)
            {
                DateTime start = DateTime.Now;



                byte[] frame = new byte[327680];
                lock (locker)
                {
                    BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, 960, 160), ImageLockMode.ReadOnly, bmp.PixelFormat);
                    int numbytes = bmpdata.Stride * bmp.Height;
                    byte[] bytedata = new byte[numbytes];
                    IntPtr ptr = bmpdata.Scan0;
                    Marshal.Copy(ptr, bytedata, 0, numbytes);
                    bmp.UnlockBits(bmpdata);
                    
                    int count = 0;
                    for (int y = 0; y < 160; y++)
                    {
                        for (int x = 0; x < 960; x++)
                        {
                            int next = x + y * 960;
                            int pixel = bytedata[next++] >> 3;
                            pixel <<= 6;
                            pixel += bytedata[next++] >> 2;
                            pixel <<= 5;
                            pixel += bytedata[next++] >> 3;
                            byte[] temp = BitConverter.GetBytes(pixel ^ xOrMasks[x % 2]);
                            frame[count++] = temp[1];
                            frame[count++] = temp[0];
                        }
                        count += 128;
                    }

                }














                //byte[] rawFrame;
                //byte[] frame = new byte[327680];
                //int count = 0;
                //lock (locker)
                //{
                //    using (var ms = new MemoryStream())
                //    {
                //        bmp.Save(ms, ImageFormat.Bmp);
                //        rawFrame = ms.ToArray();
                //    }
                //    for (int y = 0; y < 160; y++)
                //    {
                //        for (int i = 0; i < 1920; i += 4)
                //        {
                //            int next = 66 + i + y * 1920;
                //            frame[count++] = rawFrame[next++] ^= 0xE7;
                //            frame[count++] = rawFrame[next++] ^= 0xF3;
                //            frame[count++] = rawFrame[next++] ^= 0xE7;
                //            frame[count++] = rawFrame[next++] ^= 0xFF;
                //        }
                //        count += 128;
                //    }



                //    //for (int y = 0; y < 160; y++)
                //    //{
                //    //    byte[] data = new byte[2048];
                //    //    int b = 0;
                //    //    for (int x = 0; x < 960; x++)
                //    //    {
                //    //        Color pixel = bmp.GetPixel(x, y);
                //    //        int pixel_r = (pixel.R & 0xF8) >> 3;
                //    //        int pixel_g = (pixel.G & 0xFC) >> 2;
                //    //        int pixel_b = (pixel.B & 0xF8) >> 3;
                //    //        int pixel565 = pixel_r + (pixel_g << 5) + (pixel_b << 11);
                //    //        data[b++] = (byte)(pixel565 & 255); //maybe swap
                //    //        data[b++] = (byte)(pixel565 >> 8);  //maybe swap
                //    //    }
                //    //    for (int i = 0; i < 1920; i += 4)
                //    //    {
                //    //        frame[count++] = data[i + 0] ^= 0xE7;
                //    //        frame[count++] = data[i + 1] ^= 0xF3;
                //    //        frame[count++] = data[i + 2] ^= 0xE7;
                //    //        frame[count++] = data[i + 3] ^= 0xFF;
                //    //    }
                //    //    count += 128;
                //    //}

                //}
                Console.WriteLine(DateTime.Now - start);
                ec = writer.Write(frame_header, 1000, out bytesWritten);
                //
                ec = writer.Write(frame, 1000, out bytesWritten);

                //for (int b = 0; b < 640; b++)
                //{

                //    ec = writer.Write(frame.Skip(b * 512).Take(512).ToArray(), 1, out bytesWritten);

                //}

                //Thread.Sleep(1000 / PUSH2_DISPLAY_FRAMERATE);
            }
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
