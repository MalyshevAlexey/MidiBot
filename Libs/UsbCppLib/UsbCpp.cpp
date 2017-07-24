#include <iostream>
#include "UsbCpp.h"
#include <windows.h>
//#include "libusb.h"
#using <System.Drawing.dll>

namespace MidiBotCpp
{
	using namespace System;
	using namespace System::Drawing;
	using namespace System::Drawing::Imaging;

	const int PUSH2_DISPLAY_WIDTH = 960;
	const int PUSH2_DISPLAY_HEIGHT = 160;

	void UsbCpp::Show()
	{
		Bitmap^ bmp = gcnew Bitmap(PUSH2_DISPLAY_WIDTH, PUSH2_DISPLAY_HEIGHT, PixelFormat::Format16bppRgb565);
		Graphics^ g = gcnew Graphics->FromImage(bmp);
		Pen^ pen = gcnew Pen(Color::DarkRed);
		g->Clear(Color::White);
		g->Flush();
		printf("UsbCpp Test");
		printf("UsbCpp string");
	}
}