using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	public static class TypeSize<T> where T : struct {
		public static readonly Size Value = Marshal.SizeOf<T>();
	}
}
