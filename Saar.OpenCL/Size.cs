using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	[StructLayout(LayoutKind.Sequential)]
	public struct Size {
		private IntPtr value;
		
		private Size(IntPtr value) => this.value = value;

		public override string ToString() => ((ulong) value).ToString();

		public static int Bytes => IntPtr.Size;

		public static implicit operator uint(Size size) => (uint) size.value;
		public static implicit operator ulong(Size size) => (ulong) size.value;
		public static implicit operator int(Size size) => (int) size.value;
		public static implicit operator IntPtr(Size size) => size.value;
		public static implicit operator Size(uint value) => new Size((IntPtr) value);
		public static implicit operator Size(ulong value) => new Size((IntPtr) value);
		public static implicit operator Size(int value) => new Size((IntPtr) value);
		public static implicit operator Size(IntPtr value) => new Size(value);
	}
}
