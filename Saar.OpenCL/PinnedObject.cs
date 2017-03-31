using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe internal struct PinnedObject : IDisposable {
		private GCHandle handle;

		public PinnedObject(object pinnedObject) {
			handle = GCHandle.Alloc(pinnedObject, GCHandleType.Pinned);
		}

		public void Dispose() {
			handle.Free();
		}

		public static implicit operator IntPtr(PinnedObject @this) => @this.handle.AddrOfPinnedObject();
		public static implicit operator void* (PinnedObject @this) => (void*) @this.handle.AddrOfPinnedObject();
	}
}
