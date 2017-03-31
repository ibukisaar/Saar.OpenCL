using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public struct Points<T> : IDisposable {
		private GCHandle handle;
		private IntPtr[] points;

		internal Points(IEnumerable<T> objs, Converter<T, IntPtr> converter) {
			var array = objs.ToArray();
			points = new IntPtr[array.Length];
			for (int i = 0; i < array.Length; i++) {
				points[i] = converter(array[i]);
			}
			handle = GCHandle.Alloc(points, GCHandleType.Pinned);
		}

		public void Dispose() {
			handle.Free();
		}

		public IntPtr Ptrs => handle.AddrOfPinnedObject();
	}
}
