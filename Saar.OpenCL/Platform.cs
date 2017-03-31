using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Saar.OpenCL {
	unsafe public sealed class Platform : StaticObject<Platform>, IGetInfo {
		public static IReadOnlyList<Platform> Platforms { get; }
		public static Platform MainPlatform { get; }

		internal ClPlatform* platform;

		public string Name { get; }
		public string Version { get; }
		public string Vendor { get; }
		public IReadOnlyList<string> Extensions { get; }
		public IReadOnlyList<Device> Devices { get; }

		private Platform(ClPlatform* platform) {
			this.platform = platform;

			Name = this.GetString(PlatformInfo.Name);
			Version = this.GetString(PlatformInfo.Version);
			Vendor = this.GetString(PlatformInfo.Vendor);
			string extensions = this.GetString(PlatformInfo.Extensions);
			Extensions = Array.AsReadOnly(extensions.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

			Cl.clGetDeviceIDs(this.platform, DeviceType.All, 0, null, out var numDevices);
			if (numDevices == 0) { Devices = Array.Empty<Device>(); return; }

			var devices = stackalloc ClDevice*[(int) numDevices];
			Cl.clGetDeviceIDs(this.platform, DeviceType.All, numDevices, devices, out _);
			var devicesArray = new Device[numDevices];
			for (int i = 0; i < numDevices; i++) {
				devicesArray[i] = Device.Get(devices[i]);
			}
			Devices = Array.AsReadOnly(devicesArray);
		}

		public override string ToString() {
			return Name;
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetPlatformInfo(platform, (PlatformInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		static Platform() {
			Cl.clGetPlatformIDs(0, null, out var num);
			if (num == 0) { Platforms = Array.Empty<Platform>(); return; }
			var platforms = stackalloc ClPlatform*[(int) num];
			Cl.clGetPlatformIDs(num, platforms, out _);

			var platformsArray = new Platform[num];
			for (int i = 0; i < num; i++) {
				platformsArray[i] = Get(platforms[i]);
			}
			Platforms = Array.AsReadOnly(platformsArray);
			MainPlatform = Platforms.FirstOrDefault();
		}

		internal static Platform Get(ClPlatform* internalPlatform) => Get((IntPtr) internalPlatform, ptr => new Platform((ClPlatform*) ptr));
	}
}
