﻿/*
    Copyright (C) 2014-2015 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ICSharpCode.ILSpy.AsmEditor.Resources;

using WF = System.Windows.Forms;

namespace ICSharpCode.ILSpy.AsmEditor.ViewHelpers
{
	sealed class EditImage : IEdit<ImageVM>
	{
		readonly Window ownerWindow;

		public EditImage()
			: this(null)
		{
		}

		public EditImage(Window ownerWindow)
		{
			this.ownerWindow = ownerWindow;
		}

		public ImageVM Edit(string title, ImageVM mo)
		{
			var dlg = new WF.OpenFileDialog {
				RestoreDirectory = true,
				Multiselect = false,
				Filter = "Images|*.png;*.gif;*.bmp;*.dib;*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi;*.ico|All files (*.*)|*.*",
			};
			if (dlg.ShowDialog() != WF.DialogResult.OK)
				return null;

			Stream imgStream = null;
			try {
				var bimg = new BitmapImage();
				bimg.BeginInit();
				bimg.StreamSource = imgStream = File.OpenRead(dlg.FileName);
				bimg.EndInit();

				mo.ImageSource = bimg;
				return mo;
			}
			catch (Exception ex) {
				if (imgStream != null)
					imgStream.Dispose();
				MainWindow.Instance.ShowMessageBox(string.Format("Could not open file or it's not an image. Error: {0}", ex.Message), MessageBoxButton.OK, ownerWindow);
				return null;
			}
		}
	}
}
