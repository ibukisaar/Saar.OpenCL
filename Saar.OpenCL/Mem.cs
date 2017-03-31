using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public sealed class Mem : DisposableObject, IGetInfo, IHandle {
		internal ClMem* mem;
		IntPtr IHandle.Handle => (IntPtr) mem;

		public Context Context { get; }

		public MemObjectType Type => (MemObjectType) this.GetUInt32(MemInfo.Type);
		public MemFlags Flags => (MemFlags) this.GetUInt32(MemInfo.Flags);
		public Size Size => this.GetIntPtr(MemInfo.Size);
		public IntPtr HostPtr => this.GetIntPtr(MemInfo.HostPtr);
		public uint MapCount => this.GetUInt32(MemInfo.MapCount);

		[Obsolete("在CSharp中不需要使用这个属性")]
		public int RefCount => (int) this.GetUInt32(MemInfo.ReferenceCount);

		internal Mem(ClMem* mem, Context context) {
			try {
				this.mem = mem;
				Context = context;
				context.AddObject(this);
			} catch {
				Dispose();
				throw;
			}
		}

		protected override void Dispose(bool disposing) {
			if (mem != null) {
				Context.RemoveObject(this);
				Cl.clReleaseMemObject(mem);
				mem = null;
			}
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetMemObjectInfo(mem, (MemInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		public override string ToString() => $"Type: {Type}, Size: {Size}";
	}
}
