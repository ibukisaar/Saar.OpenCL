using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public interface IHandle : IDisposable {
		IntPtr Handle { get; }
	}
}
