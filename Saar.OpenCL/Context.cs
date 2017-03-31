using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Saar.OpenCL {
	unsafe public sealed class Context : ContainerOf<(CommandQueue, Mem, Program, Event)>, IGetInfo, IHandle {
		internal ClContext* context;
		IntPtr IHandle.Handle => (IntPtr) context;

		[Obsolete("在CSharp中不需要使用这个属性")]
		public int RefCount => (int) this.GetUInt32(ContextInfo.ReferenceCount);

		private IReadOnlyList<Device> devices;
		public IReadOnlyList<Device> Devices => this.GetObjects(ref devices, ContextInfo.Devices, ptr => Device.Get((ClDevice*) ptr));

		public IReadOnlyList<CommandQueue> CommandQueues => GetObjects<CommandQueue>();

		public IReadOnlyList<Mem> Mems => GetObjects<Mem>();

		public IReadOnlyList<Program> Programs => GetObjects<Program>();

		private Context(ClContext* context) {
			this.context = context;
		}

		protected override void Dispose(bool disposing) {
			if (context != null) {
				Cl.clReleaseContext(context);
				context = null;
			}
			base.Dispose(disposing);
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetContextInfo(context, (ContextInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		public static Context Create(Platform platform, DeviceType type = DeviceType.Gpu) {
			ClContextProperty* properties = stackalloc ClContextProperty[2];
			properties[0].Key = ContextProperties.Platform;
			properties[0].Value = platform.platform;
			var ctx = Cl.clCreateContextFromType(properties, type, null, IntPtr.Zero, out var err);
			err.Check();
			return new Context(ctx);
		}

		public static Context Create(IEnumerable<Device> devices, DeviceType type = DeviceType.Gpu) {
			var deviceArray = devices.ToArray();
			var devicePtrs = stackalloc ClDevice*[deviceArray.Length];
			for (int i = 0; i < deviceArray.Length; i++) devicePtrs[i] = deviceArray[i].device;
			var ctx = Cl.clCreateContext(null, (uint) deviceArray.Length, devicePtrs, null, IntPtr.Zero, out var err);
			err.Check();
			return new Context(ctx);
		}

		public IReadOnlyList<CommandQueue> CreateCommandQueues(CommandQueueProperties properties = CommandQueueProperties.None) {
			foreach (var device in Devices) {
				CommandQueue.Create(this, device, properties);
			}
			return CommandQueues;
		}


		public Mem CreateBuffer(Size bytes, MemFlags flags = MemFlags.WriteOnly) {
			var ret = Cl.clCreateBuffer(context, flags, bytes, IntPtr.Zero, out var err);
			err.Check();
			return new Mem(ret, this);
		}

		public Mem CreateBuffer(Array input, MemFlags flags = MemFlags.ReadOnly) {
			return CreateBuffer(input, 0, Buffer.ByteLength(input), flags);
		}

		public Mem CreateBuffer(Array input, Size byteOffset, Size byteLength, MemFlags flags = MemFlags.ReadOnly) {
			using (var pin = input.ToPinned()) {
				return CreateBuffer((IntPtr) pin + byteOffset, byteLength, flags);
			}
		}

		public Mem CreateBuffer(IntPtr input, Size byteLength, MemFlags flags = MemFlags.ReadOnly) {
			var ret = Cl.clCreateBuffer(context, flags | MemFlags.CopyHostPtr, byteLength, input, out var err);
			err.Check();
			return new Mem(ret, this);
		}

		public Mem CreateImage2D(ChannelOrder order, ChannelType type, Size width, Size height, IntPtr srcScan0, MemFlags flags = MemFlags.ReadOnly, Size? rowPitch = null) {
			var format = new ClImageFormat {
				Order = order,
				DateType = type
			};
			if (srcScan0 != IntPtr.Zero) flags |= MemFlags.CopyHostPtr;

			var ret = Cl.clCreateImage2D(context, flags, &format, width, height, rowPitch ?? 0, srcScan0, out var err);
			err.Check();
			return new Mem(ret, this);
		}

		public Mem CreateImage2D(ChannelOrder order, ChannelType type, Size width, Size height, Array imageData, MemFlags flags = MemFlags.ReadOnly, Size? rowPitch = null) {
			using (var scan0 = imageData.ToPinned()) {
				return CreateImage2D(order, type, width, height, scan0, flags, rowPitch);
			}
		}

		public Mem CreateImage2D(ChannelOrder order, ChannelType type, Size width, Size height, MemFlags flags = MemFlags.WriteOnly, Size? rowPitch = null) {
			return CreateImage2D(order, type, width, height, IntPtr.Zero, flags, rowPitch);
		}

		public Mem CreateImage3D(ChannelOrder order, ChannelType type, Size width, Size height, Size depth, IntPtr srcScan0, MemFlags flags = MemFlags.ReadOnly, Size? rowPitch = null, Size? slicePitch = null) {
			var format = new ClImageFormat {
				Order = order,
				DateType = type
			};

			var ret = Cl.clCreateImage3D(context, flags | MemFlags.CopyHostPtr, &format, width, height, depth, rowPitch ?? 0, slicePitch ?? 0, srcScan0, out var err);
			err.Check();
			return new Mem(ret, this);
		}

		public Mem CreateImage3D(ChannelOrder order, ChannelType type, Size width, Size height, Size depth, Array imageData, MemFlags flags = MemFlags.ReadOnly, Size? rowPitch = null, Size? slicePitch = null) {
			using (var scan0 = imageData.ToPinned()) {
				return CreateImage3D(order, type, width, height, depth, scan0, flags, rowPitch, slicePitch);
			}
		}

		public Mem CreateImage3D(ChannelOrder order, ChannelType type, Size width, Size height, Size depth, MemFlags flags = MemFlags.WriteOnly, Size? rowPitch = null, Size? slicePitch = null) {
			return CreateImage3D(order, type, width, height, depth, IntPtr.Zero, flags, rowPitch, slicePitch);
		}
	}
}
