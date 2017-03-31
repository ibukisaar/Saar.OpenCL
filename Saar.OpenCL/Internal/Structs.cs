using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	public struct ClEvent { }

	public struct ClContext { }

	public struct ClPlatform { }

	public struct ClProgram { }

	public struct ClDevice { }

	public struct ClKernel { }

	public struct ClCommandQueue { }

	public struct ClMem { }


	[StructLayout(LayoutKind.Sequential)]
	unsafe public struct ClContextProperty {
		public ContextProperties Key;
		public ClPlatform* Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ClImageFormat {
		public ChannelOrder Order;
		public ChannelType DateType;
	}
}
