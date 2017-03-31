using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	public enum ProfilingInfo : int {
		Queued = 0x1280,
		Submit = 0x1281,
		Start = 0x1282,
		End = 0x1283,
	}

	public enum SamplerInfo : uint {
		ReferenceCount = 0x1150,
		Context = 0x1151,
		NormalizedCoords = 0x1152,
		AddressingMode = 0x1153,
		FilterMode = 0x1154,
	}

	public enum FilterMode : uint {
		Nearest = 0x1140,
		Linear = 0x1141,
	}

	public enum AddressingMode : uint {
		None = 0x1130,
		ClampToEdge = 0x1131,
		Clamp = 0x1132,
		Repeat = 0x1133,
	}

	public enum ExecutionStatus : int {
		Complete = 0x0,
		Running = 0x1,
		Submitted = 0x2,
		Queued = 0x3,
	}

	public enum EventInfo : int {
		CommandQueue = 0x11D0,
		CommandType = 0x11D1,
		ReferenceCount = 0x11D2,
		CommandExecutionStatus = 0x11D3,
	}

	[Flags]
	public enum MapFlags : int {
		Read = (1 << 0),
		Write = (1 << 1),
	}

	public enum KernelWorkGroupInfo : int {
		WorkGroupSize = 0x11B0,
		CompileWorkGroupSize = 0x11B1,
		LocalMemSize = 0x11B2
	}

	public enum KernelInfo : int {
		FunctionName = 0x1190,
		NumArgs = 0x1191,
		ReferenceCount = 0x1192,
		Context = 0x1193,
		Program = 0x1194
	}

	public enum CommandQueueInfo : int {
		Context = 0x1090,
		Device = 0x1091,
		ReferenceCount = 0x1092,
		Properties = 0x1093,
		Size = 0x1094,
	}

	[Flags]
	public enum CommandQueueProperties : ulong {
		None = 0,
		OutOfOrderExecModeEnable = (1 << 0),
		ProfilingEnable = (1 << 1)
	}

	public enum BuildStatus : int {
		Success = 0,
		None = -1,
		Error = -2,
		InProgress = -3
	}

	public enum ProgramBuildInfo : uint {
		Status = 0x1181,
		Options = 0x1182,
		Log = 0x1183,
	}

	public enum ProgramInfo : uint {
		ReferenceCount = 0x1160,
		Context = 0x1161,
		NumDevices = 0x1162,
		Devices = 0x1163,
		Source = 0x1164,
		BinarySizes = 0x1165,
		Binaries = 0x1166,
		NumKernels = 0x1167,
		KernelNames = 0x1168,
	}

	public enum ImageInfo : uint {
		Format = 0x1110,
		ElementSize = 0x1111,
		RowPitch = 0x1112,
		SlicePitch = 0x1113,
		Width = 0x1114,
		Height = 0x1115,
		Depth = 0x1116,
	}

	public enum MemInfo : uint {
		Type = 0x1100,
		Flags = 0x1101,
		Size = 0x1102,
		HostPtr = 0x1103,
		MapCount = 0x1104,
		ReferenceCount = 0x1105,
		Context = 0x1106,
	}

	public enum MemObjectType : uint {
		Buffer = 0x10F0,
		Image2D = 0x10F1,
		Image3D = 0x10F2,
	}

	[Flags]
	public enum MemFlags : ulong {
		None = 0,
		ReadWrite = (1 << 0),
		WriteOnly = (1 << 1),
		ReadOnly = (1 << 2),
		UseHostPtr = (1 << 3),
		AllocHostPtr = (1 << 4),
		CopyHostPtr = (1 << 5),
	}

	public enum ContextInfo : uint {
		ReferenceCount = 0x1080,
		Devices = 0x1081,
		Properties = 0x1082,
	}

	public enum ChannelType : uint {
		/// <summary>
		/// Each channel component is a normalized signed 8-bit integer value.
		/// </summary>
		NormSignedInt8 = 0x10D0,
		/// <summary>
		/// Each channel component is a normalized signed 16-bit integer value.
		/// </summary>
		NormSignedInt16 = 0x10D1,
		/// <summary>
		/// Each channel component is a normalized unsigned 8-bit integer value.
		/// </summary>
		NormUnsignedInt8 = 0x10D2,
		/// <summary>
		/// Each channel component is a normalized unsigned 16-bit integer value.
		/// </summary>
		NormUnsignedInt16 = 0x10D3,
		/// <summary>
		/// Represents a normalized 5-6-5 3-channel RGB image. The channel order must be CL_RGB or CL_RGBx.
		/// </summary>
		NormUnsignedShort565 = 0x10D4,
		/// <summary>
		/// Represents a normalized x-5-5-5 4-channel xRGB image. The channel order must be CL_RGB or CL_RGBx.
		/// </summary>
		NormUnsignedShort555 = 0x10D5,
		/// <summary>
		/// Represents a normalized x-10-10-10 4-channel xRGB image. The channel order must be CL_RGB or CL_RGBx.
		/// </summary>
		NormUnsignedInt101010 = 0x10D6,
		/// <summary>
		/// Each channel component is an unnormalized signed 8-bit integer value.
		/// </summary>
		SignedInt8 = 0x10D7,
		/// <summary>
		/// Each channel component is an unnormalized signed 16-bit integer value.
		/// </summary>
		SignedInt16 = 0x10D8,
		/// <summary>
		/// Each channel component is an unnormalized signed 32-bit integer value.
		/// </summary>
		SignedInt32 = 0x10D9,
		/// <summary>
		/// Each channel component is an unnormalized unsigned 8-bit integer value.
		/// </summary>
		UnsignedInt8 = 0x10DA,
		/// <summary>
		/// Each channel component is an unnormalized unsigned 16-bit integer value.
		/// </summary>
		UnsignedInt16 = 0x10DB,
		/// <summary>
		/// Each channel component is an unnormalized unsigned 32-bit integer value.
		/// </summary>
		UnsignedInt32 = 0x10DC,
		/// <summary>
		/// Each channel component is a 16-bit half-float value.
		/// </summary>
		HalfFloat = 0x10DD,
		/// <summary>
		/// Each channel component is a single precision floating-point value.
		/// </summary>
		Float = 0x10DE,
	}

	public enum ChannelOrder : uint {
		R = 0x10B0,
		A = 0x10B1,
		RG = 0x10B2,
		RA = 0x10B3,
		RGB = 0x10B4,
		RGBA = 0x10B5,
		BGRA = 0x10B6,
		ARGB = 0x10B7,
		Intensity = 0x10B8,
		Luminance = 0x10B9,
	}

	public enum ContextProperties : uint {
		Platform = 0x1084,
	}

	public enum DeviceInfo : uint {
		Type = 0x1000,
		VendorId = 0x1001,
		MaxComputeUnits = 0x1002,
		MaxWorkItemDimensions = 0x1003,
		MaxWorkGroupSize = 0x1004,
		MaxWorkItemSizes = 0x1005,
		PreferredVectorWidthChar = 0x1006,
		PreferredVectorWidthShort = 0x1007,
		PreferredVectorWidthInt = 0x1008,
		PreferredVectorWidthLong = 0x1009,
		PreferredVectorWidthFloat = 0x100A,
		PreferredVectorWidthDouble = 0x100B,
		MaxClockFrequency = 0x100C,
		AddressBits = 0x100D,
		MaxReadImageArgs = 0x100E,
		MaxWriteImageArgs = 0x100F,
		MaxMemAllocSize = 0x1010,
		Image2dMaxWidth = 0x1011,
		Image2dMaxHeight = 0x1012,
		Image3dMaxWidth = 0x1013,
		Image3dMaxHeight = 0x1014,
		Image3dMaxDepth = 0x1015,
		ImageSupport = 0x1016,
		MaxParameterSize = 0x1017,
		MaxSamplers = 0x1018,
		MemBaseAddrAlign = 0x1019,
		MinDataTypeAlignSize = 0x101A,
		SingleFpConfig = 0x101B,
		GlobalMemCacheType = 0x101C,
		GlobalMemCachelineSize = 0x101D,
		GlobalMemCacheSize = 0x101E,
		GlobalMemSize = 0x101F,
		MaxConstantBufferSize = 0x1020,
		MaxConstantArgs = 0x1021,
		LocalMemType = 0x1022,
		LocalMemSize = 0x1023,
		ErrorCorrectionSupport = 0x1024,
		ProfilingTimerResolution = 0x1025,
		EndianLittle = 0x1026,
		Available = 0x1027,
		CompilerAvailable = 0x1028,
		ExecutionCapabilities = 0x1029,
		QueueProperties = 0x102A,
		QueueOnHostProperties = 0x102A,
		Name = 0x102B,
		Vendor = 0x102C,
		Profile = 0x102E,
		Version = 0x102F,
		Extensions = 0x1030,
		Platform = 0x1031,
		DoubleFpConfig = 0x1032,
		/// <summary>
		/// reserved
		/// </summary>
		HalfFpConfig = 0x1033,
		PreferredVectorWidthHalf = 0x1034,
		HostUnifiedMemory = 0x1035,
		NativeVectorWidthChar = 0x1036,
		NativeVectorWidthShort = 0x1037,
		NativeVectorWidthInt = 0x1038,
		NativeVectorWidthLong = 0x1039,
		NativeVectorWidthFloat = 0x103A,
		NativeVectorWidthDouble = 0x103B,
		NativeVectorWidthHalf = 0x103C,
		OpenclCVersion = 0x103D,
		LinkerAvailable = 0x103E,
		BuiltInKernels = 0x103F,
		ImageMaxBufferSize = 0x1040,
		ImageMaxArraySize = 0x1041,
		ParentDevice = 0x1042,
		PartitionMaxSubDevices = 0x1043,
		PartitionProperties = 0x1044,
		PartitionAffinityDomain = 0x1045,
		PartitionType = 0x1046,
		ReferenceCount = 0x1047,
		PreferredInteropUserSync = 0x1048,
		PrintfBufferSize = 0x1049,
		ImagePitchAlignment = 0x104A,
		ImageBaseAddressAlignment = 0x104B,
		MaxReadWriteImageArgs = 0x104C,
		MaxGlobalVariableSize = 0x104D,
		QueueOnDeviceProperties = 0x104E,
		QueueOnDevicePreferredSize = 0x104F,
		QueueOnDeviceMaxSize = 0x1050,
		MaxOnDeviceQueues = 0x1051,
		MaxOnDeviceEvents = 0x1052,
		SvmCapabilities = 0x1053,
		GlobalVariablePreferredTotalSize = 0x1054,
		MaxPipeArgs = 0x1055,
		PipeMaxActiveReservations = 0x1056,
		PipeMaxPacketSize = 0x1057,
		PreferredPlatformAtomicAlignment = 0x1058,
		PreferredGlobalAtomicAlignment = 0x1059,
		PreferredLocalAtomicAlignment = 0x105A,
	}

	[Flags]
	public enum DeviceType : ulong {
		Default = (1 << 0),
		Cpu = (1 << 1),
		Gpu = (1 << 2),
		Accelerator = (1 << 3),
		All = 0xFFFFFFFF,
	}

	public enum PlatformInfo : uint {
		Profile = 0x0900,
		Version = 0x0901,
		Name = 0x0902,
		Vendor = 0x0903,
		Extensions = 0x0904,
	}

	public enum ErrorCode : int {
		Unknown = 1,

		Success = 0,
		DeviceNotFound = -1,
		DeviceNotAvailable = -2,
		CompilerNotAvailable = -3,
		MemObjectAllocationFailure = -4,
		OutOfResources = -5,
		OutOfHostMemory = -6,
		ProfilingInfoNotAvailable = -7,
		MemCopyOverlap = -8,
		ImageFormatMismatch = -9,
		ImageFormatNotSupported = -10,
		BuildProgramFailure = -11,
		MapFailure = -12,

		InvalidValue = -30,
		InvalidDeviceType = -31,
		InvalidPlatform = -32,
		InvalidDevice = -33,
		InvalidContext = -34,
		InvalidQueueProperties = -35,
		InvalidCommandQueue = -36,
		InvalidHostPtr = -37,
		InvalidMemObject = -38,
		InvalidImageFormatDescriptor = -39,
		InvalidImageSize = -40,
		InvalidSampler = -41,
		InvalidBinary = -42,
		InvalidBuildOptions = -43,
		InvalidProgram = -44,
		InvalidProgramExecutable = -45,
		InvalidKernelName = -46,
		InvalidKernelDefinition = -47,
		InvalidKernel = -48,
		InvalidArgIndex = -49,
		InvalidArgValue = -50,
		InvalidArgSize = -51,
		InvalidKernelArgs = -52,
		InvalidWorkDimension = -53,
		InvalidWorkGroupSize = -54,
		InvalidWorkItemSize = -55,
		InvalidGlobalOffset = -56,
		InvalidEventWaitList = -57,
		InvalidEvent = -58,
		InvalidOperation = -59,
		InvalidGlObject = -60,
		InvalidBufferSize = -61,
		InvalidMipLevel = -62,
	}

	public enum CommandType : uint {
		NDRangeKernel = 0x11F0,
		Task = 0x11F1,
		NativeKernel = 0x11F2,
		ReadBuffer = 0x11F3,
		WriteBuffer = 0x11F4,
		CopyBuffer = 0x11F5,
		ReadImage = 0x11F6,
		WriteImage = 0x11F7,
		CopyImage = 0x11F8,
		CopyImageToBuffer = 0x11F9,
		CopyBufferToImage = 0x11FA,
		MapBuffer = 0x11FB,
		MapImage = 0x11FC,
		UnmapMemObject = 0x11FD,
		Marker = 0x11FE,
		AcquireGlObjects = 0x11FF,
		ReleaseGlObjects = 0x1200,
		ReadBufferRect = 0x1201,
		WriteBufferRect = 0x1202,
		CopyBufferRect = 0x1203,
		User = 0x1204,
		Barrier = 0x1205,
		MigrateMemObjects = 0x1206,
		FillBuffer = 0x1207,
		FillImage = 0x1208,
		SvmFree = 0x1209,
		SvmMemcpy = 0x120A,
		SvmMemfill = 0x120B,
		SvmMap = 0x120C,
	}

	public enum KernelArgInfo : uint {
		AddressQualifier = 0x1196,
		AccessQualifier = 0x1197,
		TypeName = 0x1198,
		TypeQualifier = 0x1199,
		Name = 0x119A,
	}

	public enum KernelArgAddressQualifier : uint {
		Global = 0x119B,
		Local = 0x119C,
		Constant = 0x119D,
		Private = 0x119E,
	}

	public enum KernelArgAccessQualifier : uint {
		ReadOnly = 0x11A0,
		WriteOnly = 0x11A1,
		ReadWrite = 0x11A2,
		None = 0x11A3,
	}

	[Flags]
	public enum KernelArgTypeQualifier : uint {
		None = 0,
		Const = 1 << 0,
		Restrict = 1 << 1,
		Volatile = 1 << 2,
		Pipe = 1 << 3,
	}
}
