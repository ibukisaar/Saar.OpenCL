using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	unsafe public delegate void BuildProgramCallback(ClProgram* program, IntPtr userData);
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	unsafe public delegate void CreateContextCallback(string error, IntPtr privateInfo, Size cb, IntPtr userData);
}
