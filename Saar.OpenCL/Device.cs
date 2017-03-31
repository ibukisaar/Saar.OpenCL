using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public sealed class Device : StaticObject<Device>, IGetInfo {
		private Device(ClDevice* device) {
			this.device = device;
		}

		#region Fields

		internal ClDevice* device;

		private Platform platform;
		public Platform Platform => platform ?? (platform = Platform.Get((ClPlatform*) this.GetIntPtr(DeviceInfo.Platform)));

		private DeviceType? type;
		public DeviceType Type => type ?? (type = (DeviceType) this.GetUInt64(DeviceInfo.Type)).Value;

		private uint? vendorId;
		public uint VendorId => this.GetUInt32(ref vendorId, DeviceInfo.VendorId);

		private uint? maxComputeUnits;
		public uint MaxComputeUnits => this.GetUInt32(ref maxComputeUnits, DeviceInfo.MaxComputeUnits);

		private uint? maxWorkItemDimensions;
		public uint MaxWorkItemDimensions => this.GetUInt32(ref maxWorkItemDimensions, DeviceInfo.MaxWorkItemDimensions);

		private Size? maxWorkGroupSize;
		public Size MaxWorkGroupSize => this.GetSize(ref maxWorkGroupSize, DeviceInfo.MaxWorkGroupSize);

		private IReadOnlyList<Size> maxWorkItemSizes;
		public IReadOnlyList<Size> MaxWorkItemSizes => this.GetSizes(ref maxWorkItemSizes, DeviceInfo.MaxWorkItemSizes, MaxWorkItemDimensions);

		private uint? preferredVectorWidthChar;
		public uint PreferredVectorWidthChar => this.GetUInt32(ref preferredVectorWidthChar, DeviceInfo.PreferredVectorWidthChar);

		private uint? preferredVectorWidthShort;
		public uint PreferredVectorWidthShort => this.GetUInt32(ref preferredVectorWidthShort, DeviceInfo.PreferredVectorWidthShort);

		private uint? preferredVectorWidthInt;
		public uint PreferredVectorWidthInt => this.GetUInt32(ref preferredVectorWidthInt, DeviceInfo.PreferredVectorWidthInt);

		private uint? preferredVectorWidthLong;
		public uint PreferredVectorWidthLong => this.GetUInt32(ref preferredVectorWidthLong, DeviceInfo.PreferredVectorWidthLong);

		private uint? preferredVectorWidthFloat;
		public uint PreferredVectorWidthFloat => this.GetUInt32(ref preferredVectorWidthFloat, DeviceInfo.PreferredVectorWidthFloat);

		private uint? preferredVectorWidthDouble;
		public uint PreferredVectorWidthDouble => this.GetUInt32(ref preferredVectorWidthDouble, DeviceInfo.PreferredVectorWidthDouble);

		private uint? maxClockFrequency;
		public uint MaxClockFrequency => this.GetUInt32(ref maxClockFrequency, DeviceInfo.MaxClockFrequency);

		private uint? addressBits;
		public uint AddressBits => this.GetUInt32(ref addressBits, DeviceInfo.AddressBits);

		private uint? maxReadImageArgs;
		public uint MaxReadImageArgs => this.GetUInt32(ref maxReadImageArgs, DeviceInfo.MaxReadImageArgs);

		private uint? maxWriteImageArgs;
		public uint MaxWriteImageArgs => this.GetUInt32(ref maxWriteImageArgs, DeviceInfo.MaxWriteImageArgs);

		private ulong? maxMemAllocSize;
		public ulong MaxMemAllocSize => this.GetUInt64(ref maxMemAllocSize, DeviceInfo.MaxMemAllocSize);

		private Size? image2dMaxWidth;
		public Size Image2dMaxWidth => this.GetSize(ref image2dMaxWidth, DeviceInfo.Image2dMaxWidth);

		private Size? image2dMaxHeight;
		public Size Image2dMaxHeight => this.GetSize(ref image2dMaxHeight, DeviceInfo.Image2dMaxHeight);

		private Size? image3dMaxWidth;
		public Size Image3dMaxWidth => this.GetSize(ref image3dMaxWidth, DeviceInfo.Image3dMaxWidth);

		private Size? image3dMaxHeight;
		public Size Image3dMaxHeight => this.GetSize(ref image3dMaxHeight, DeviceInfo.Image3dMaxHeight);

		private Size? image3dMaxDepth;
		public Size Image3dMaxDepth => this.GetSize(ref image3dMaxDepth, DeviceInfo.Image3dMaxDepth);

		private bool? imageSupport;
		public bool ImageSupport => this.GetBoolean(ref imageSupport, DeviceInfo.ImageSupport);

		private Size? maxParameterSize;
		public Size MaxParameterSize => this.GetSize(ref maxParameterSize, DeviceInfo.MaxParameterSize);

		private uint? maxSamplers;
		public uint MaxSamplers => this.GetUInt32(ref maxSamplers, DeviceInfo.MaxSamplers);

		private uint? memBaseAddrAlign;
		public uint MemBaseAddrAlign => this.GetUInt32(ref memBaseAddrAlign, DeviceInfo.MemBaseAddrAlign);

		private uint? minDataTypeAlignSize;
		public uint MinDataTypeAlignSize => this.GetUInt32(ref minDataTypeAlignSize, DeviceInfo.MinDataTypeAlignSize);

		private ulong? singleFpConfig;
		public ulong SingleFpConfig => this.GetUInt64(ref singleFpConfig, DeviceInfo.SingleFpConfig);

		private uint? globalMemCacheType;
		public uint GlobalMemCacheType => this.GetUInt32(ref globalMemCacheType, DeviceInfo.GlobalMemCacheType);

		private uint? globalMemCachelineSize;
		public uint GlobalMemCachelineSize => this.GetUInt32(ref globalMemCachelineSize, DeviceInfo.GlobalMemCachelineSize);

		private ulong? globalMemCacheSize;
		public ulong GlobalMemCacheSize => this.GetUInt64(ref globalMemCacheSize, DeviceInfo.GlobalMemCacheSize);

		private ulong? globalMemSize;
		public ulong GlobalMemSize => this.GetUInt64(ref globalMemSize, DeviceInfo.GlobalMemSize);

		private ulong? maxConstantBufferSize;
		public ulong MaxConstantBufferSize => this.GetUInt64(ref maxConstantBufferSize, DeviceInfo.MaxConstantBufferSize);

		private uint? maxConstantArgs;
		public uint MaxConstantArgs => this.GetUInt32(ref maxConstantArgs, DeviceInfo.MaxConstantArgs);

		private uint? localMemType;
		public uint LocalMemType => this.GetUInt32(ref localMemType, DeviceInfo.LocalMemType);

		private ulong? localMemSize;
		public ulong LocalMemSize => this.GetUInt64(ref localMemSize, DeviceInfo.LocalMemSize);

		private bool? errorCorrectionSupport;
		public bool ErrorCorrectionSupport => this.GetBoolean(ref errorCorrectionSupport, DeviceInfo.ErrorCorrectionSupport);

		private Size? profilingTimerResolution;
		public Size ProfilingTimerResolution => this.GetSize(ref profilingTimerResolution, DeviceInfo.ProfilingTimerResolution);

		private bool? endianLittle;
		public bool EndianLittle => this.GetBoolean(ref endianLittle, DeviceInfo.EndianLittle);

		private bool? available;
		public bool Available => this.GetBoolean(ref available, DeviceInfo.Available);

		private bool? compilerAvailable;
		public bool CompilerAvailable => this.GetBoolean(ref compilerAvailable, DeviceInfo.CompilerAvailable);

		private ulong? executionCapabilities;
		public ulong ExecutionCapabilities => this.GetUInt64(ref executionCapabilities, DeviceInfo.ExecutionCapabilities);

		private ulong? queueProperties;
		public ulong QueueProperties => this.GetUInt64(ref queueProperties, DeviceInfo.QueueProperties);

		private ulong? queueOnHostProperties;
		public ulong QueueOnHostProperties => this.GetUInt64(ref queueOnHostProperties, DeviceInfo.QueueOnHostProperties);

		private string name;
		public string Name => this.GetString(ref name, DeviceInfo.Name);

		private string vendor;
		public string Vendor => this.GetString(ref vendor, DeviceInfo.Vendor);

		private string profile;
		public string Profile => this.GetString(ref profile, DeviceInfo.Profile);

		private string version;
		public string Version => this.GetString(ref version, DeviceInfo.Version);

		private IReadOnlyList<string> extensions;
		public IReadOnlyList<string> Extensions {
			get {
				if (extensions == null) {
					var s = this.GetString(DeviceInfo.Extensions);
					extensions = Array.AsReadOnly(s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
				}
				return extensions;
			}
		}

		private ulong? doubleFpConfig;
		public ulong DoubleFpConfig => this.GetUInt64(ref doubleFpConfig, DeviceInfo.DoubleFpConfig);

		private uint? preferredVectorWidthHalf;
		public uint PreferredVectorWidthHalf => this.GetUInt32(ref preferredVectorWidthHalf, DeviceInfo.PreferredVectorWidthHalf);

		private bool? hostUnifiedMemory;
		public bool HostUnifiedMemory => this.GetBoolean(ref hostUnifiedMemory, DeviceInfo.HostUnifiedMemory);

		private uint? nativeVectorWidthChar;
		public uint NativeVectorWidthChar => this.GetUInt32(ref nativeVectorWidthChar, DeviceInfo.NativeVectorWidthChar);

		private uint? nativeVectorWidthShort;
		public uint NativeVectorWidthShort => this.GetUInt32(ref nativeVectorWidthShort, DeviceInfo.NativeVectorWidthShort);

		private uint? nativeVectorWidthInt;
		public uint NativeVectorWidthInt => this.GetUInt32(ref nativeVectorWidthInt, DeviceInfo.NativeVectorWidthInt);

		private uint? nativeVectorWidthLong;
		public uint NativeVectorWidthLong => this.GetUInt32(ref nativeVectorWidthLong, DeviceInfo.NativeVectorWidthLong);

		private uint? nativeVectorWidthFloat;
		public uint NativeVectorWidthFloat => this.GetUInt32(ref nativeVectorWidthFloat, DeviceInfo.NativeVectorWidthFloat);

		private uint? nativeVectorWidthDouble;
		public uint NativeVectorWidthDouble => this.GetUInt32(ref nativeVectorWidthDouble, DeviceInfo.NativeVectorWidthDouble);

		private uint? nativeVectorWidthHalf;
		public uint NativeVectorWidthHalf => this.GetUInt32(ref nativeVectorWidthHalf, DeviceInfo.NativeVectorWidthHalf);

		private string openclCVersion;
		public string OpenclCVersion => this.GetString(ref openclCVersion, DeviceInfo.OpenclCVersion);

		private uint? queueOnDeviceMaxSize;
		public uint QueueOnDeviceMaxSize => this.GetUInt32(ref queueOnDeviceMaxSize, DeviceInfo.QueueOnDeviceMaxSize);

		private ulong? svmCapabilities;
		public ulong SvmCapabilities => this.GetUInt64(ref svmCapabilities, DeviceInfo.SvmCapabilities);

		#endregion

		public override string ToString() {
			return Name;
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetDeviceInfo(device, (DeviceInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		internal static Device Get(ClDevice* internalDevice) => Get((IntPtr) internalDevice, ptr => new Device((ClDevice*) ptr));
	}
}
