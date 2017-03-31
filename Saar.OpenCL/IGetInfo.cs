using System;
namespace Saar.OpenCL {
	public interface IGetInfo {
		void GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
	}
}