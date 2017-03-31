using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public sealed class CommandQueue : DisposableObject, IGetInfo, IHandle {
		private delegate ErrorCode OperateImageHandle(ClCommandQueue* commandQueue, ClMem* image, bool blocking, Size* origin, Size* region, Size rowPitch, Size slicePitch, IntPtr ptr, uint numEventsIntWaitList, ClEvent** eventWaitList, ClEvent** e);

		internal ClCommandQueue* commandQueue;
		IntPtr IHandle.Handle => (IntPtr) commandQueue;

		public Context Context { get; }

		public Device Device { get; }

		public CommandQueueProperties Properties { get; }

		[Obsolete("在CSharp中不需要使用这个属性")]
		public int RefCount => (int) this.GetUInt32(CommandQueueInfo.ReferenceCount);


		private CommandQueue(ClCommandQueue* commandQueue, Context context, Device device, CommandQueueProperties properties) {
			try {
				Context = context;
				this.commandQueue = commandQueue;
				Properties = properties;
				Device = device;

				context.AddObject(this);
			} catch {
				Dispose();
				throw;
			}
		}

		protected override void Dispose(bool disposing) {
			if (commandQueue != null) {
				Context.RemoveObject(this);
				Cl.clReleaseCommandQueue(commandQueue);
				commandQueue = null;
			}
		}

		public static CommandQueue Create(Context context, Device device, CommandQueueProperties properties = CommandQueueProperties.None) {
			var commandQueue = Cl.clCreateCommandQueue(context.context, device.device, properties, out var err);
			err.Check();
			return new CommandQueue(commandQueue, context, device, properties);
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetCommandQueueInfo(commandQueue, (CommandQueueInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		public override string ToString() => Device.ToString();

		public Event ReadBuffer(Mem src, Size srcByteOffset, IntPtr dst, Size byteLength, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);
			ClEvent* @event;
			Cl.clEnqueueReadBuffer(commandQueue, src.mem, false, srcByteOffset, byteLength, dst, (uint) events.Length, (ClEvent**) ptrs, &@event).Check();
			return new Event(@event, Context);
		}

		public Event ReadBuffer(Mem src, Size srcByteOffset, Array dst, Size dstByteOffset, Size byteLength, params Event[] events) {
			using (var pin = dst.ToPinned()) {
				return ReadBuffer(src, srcByteOffset, (IntPtr) pin + dstByteOffset, byteLength, events);
			}
		}

		public Event ReadBuffer(Mem src, Array dst, params Event[] events) {
			return ReadBuffer(src, 0, dst, 0, src.Size, events);
		}

		public void ReadBufferBlocking(Mem src, Size srcByteOffset, IntPtr dst, Size byteLength, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);
			Cl.clEnqueueReadBuffer(commandQueue, src.mem, true, srcByteOffset, byteLength, dst, (uint) events.Length, (ClEvent**) ptrs, null).Check();
		}

		public void ReadBufferBlocking(Mem src, Size srcByteOffset, Array dst, Size dstByteOffset, Size byteLength, params Event[] events) {
			using (var pin = dst.ToPinned()) {
				ReadBufferBlocking(src, srcByteOffset, (IntPtr) pin + dstByteOffset, byteLength, events);
			}
		}

		public void ReadBufferBlocking(Mem src, Array dst, params Event[] events) {
			ReadBufferBlocking(src, 0, dst, 0, src.Size, events);
		}

		public Event WriteBuffer(Mem dst, Size dstByteOffset, IntPtr src, Size byteLength, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);
			ClEvent* @event;
			Cl.clEnqueueWriteBuffer(commandQueue, dst.mem, false, dstByteOffset, byteLength, src, (uint) events.Length, (ClEvent**) ptrs, &@event).Check();
			return new Event(@event, Context);
		}

		public Event WriteBuffer(Mem dst, Size dstByteOffset, Array src, Size srcByteOffset, Size byteLength, params Event[] events) {
			using (var pin = src.ToPinned()) {
				return WriteBuffer(dst, dstByteOffset, (IntPtr) pin + srcByteOffset, byteLength, events);
			}
		}

		public Event WriteBuffer(Mem dst, Array src, params Event[] events) {
			return WriteBuffer(dst, 0, src, 0, dst.Size, events);
		}

		public void WriteBufferBlocking(Mem dst, Size dstByteOffset, IntPtr src, Size byteLength, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);
			Cl.clEnqueueWriteBuffer(commandQueue, dst.mem, true, dstByteOffset, byteLength, src, (uint) events.Length, (ClEvent**) ptrs, null).Check();
		}

		public void WriteBufferBlocking(Mem dst, Size dstByteOffset, Array src, Size srcByteOffset, Size byteLength, params Event[] events) {
			using (var pin = src.ToPinned()) {
				WriteBufferBlocking(dst, dstByteOffset, (IntPtr) pin + srcByteOffset, byteLength, events);
			}
		}

		public void WriteBufferBlocking(Mem dst, Array src, params Event[] events) {
			WriteBufferBlocking(dst, 0, src, 0, dst.Size, events);
		}

		private Event OperateImage(Mem image, (Size, Size, Size) size, IntPtr scan0, (Size, Size, Size) offset, Size? rowPitch, Size? slicePitch, Event[] events, bool blocking, OperateImageHandle handle) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);

			ClEvent* @event;
			Size* origin = stackalloc Size[3];
			Size* region = stackalloc Size[3];
			(origin[0], origin[1], origin[2]) = offset;
			(region[0], region[1], region[2]) = size;

			if (blocking) {
				handle(commandQueue, image.mem, true, origin, region, rowPitch ?? 0, slicePitch ?? 0, scan0, (uint) events.Length, (ClEvent**) ptrs, null);
				return null;
			} else {
				handle(commandQueue, image.mem, false, origin, region, rowPitch ?? 0, slicePitch ?? 0, scan0, (uint) events.Length, (ClEvent**) ptrs, &@event);
				return new Event(@event, Context);
			}
		}

		private Event OperateImage(Mem image, (Size Width, Size Height) size, IntPtr scan0, (Size X, Size Y) offset, Size? rowPitch, Event[] events, bool blocking, OperateImageHandle handle) {
			return OperateImage(image, (size.Width, size.Height, 1), scan0, (offset.X, offset.Y, 0), rowPitch, 0, events, blocking, handle);
		}

		public Event ReadImage2D(Mem srcImage, (Size Width, Size Height) size, IntPtr dst, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			return OperateImage(srcImage, size, dst, offset ?? (0, 0), rowPitch, events, false, Cl.clEnqueueReadImage);
		}

		public Event ReadImage2D(Mem srcImage, (Size Width, Size Height) size, Array dst, Size dstByteOffset, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			using (var pin = dst.ToPinned()) {
				return ReadImage2D(srcImage, size, (IntPtr) pin + dstByteOffset, offset, rowPitch, events);
			}
		}

		public void ReadImage2DBlocking(Mem srcImage, (Size Width, Size Height) size, IntPtr dst, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			OperateImage(srcImage, size, dst, offset ?? (0, 0), rowPitch, events, true, Cl.clEnqueueReadImage);
		}

		public void ReadImage2DBlocking(Mem srcImage, (Size Width, Size Height) size, Array dst, Size dstByteOffset, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			using (var pin = dst.ToPinned()) {
				ReadImage2DBlocking(srcImage, size, (IntPtr) pin + dstByteOffset, offset, rowPitch, events);
			}
		}

		public Event ReadImage3D(Mem srcImage, (Size Width, Size Height, Size Depth) size, IntPtr dst, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			return OperateImage(srcImage, size, dst, offset ?? (0, 0, 0), rowPitch, slicePitch, events, false, Cl.clEnqueueReadImage);
		}

		public Event ReadImage3D(Mem srcImage, (Size Width, Size Height, Size Depth) size, Array dst, Size dstByteOffset, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			using (var pin = dst.ToPinned()) {
				return ReadImage3D(srcImage, size, (IntPtr) pin + dstByteOffset, offset, rowPitch, slicePitch, events);
			}
		}

		public void ReadImage3DBlocking(Mem srcImage, (Size Width, Size Height, Size Depth) size, IntPtr dst, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			OperateImage(srcImage, size, dst, offset ?? (0, 0, 0), rowPitch, slicePitch, events, true, Cl.clEnqueueReadImage);
		}

		public void ReadImage3DBlocking(Mem srcImage, (Size Width, Size Height, Size Depth) size, Array dst, Size dstByteOffset, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			using (var pin = dst.ToPinned()) {
				ReadImage3DBlocking(srcImage, size, (IntPtr) pin + dstByteOffset, offset, rowPitch, slicePitch, events);
			}
		}

		public Event WriteImage2D(Mem dstImage, (Size Width, Size Height) size, IntPtr src, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			return OperateImage(dstImage, size, src, offset ?? (0, 0), rowPitch, events, false, Cl.clEnqueueWriteImage);
		}

		public Event WriteImage2D(Mem dstImage, (Size Width, Size Height) size, Array src, Size srcByteOffset, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			using (var pin = src.ToPinned()) {
				return WriteImage2D(dstImage, size, (IntPtr) pin + srcByteOffset, offset, rowPitch, events);
			}
		}

		public void WriteImage2DBlocking(Mem dstImage, (Size Width, Size Height) size, IntPtr src, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			OperateImage(dstImage, size, src, offset ?? (0, 0), rowPitch, events, true, Cl.clEnqueueWriteImage);
		}

		public void WriteImage2DBlocking(Mem dstImage, (Size Width, Size Height) size, Array src, Size srcByteOffset, (Size X, Size Y)? offset = null, Size? rowPitch = null, params Event[] events) {
			using (var pin = src.ToPinned()) {
				WriteImage2DBlocking(dstImage, size, (IntPtr) pin + srcByteOffset, offset, rowPitch, events);
			}
		}

		public Event WriteImage3D(Mem dstImage, (Size Width, Size Height, Size Depth) size, IntPtr src, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			return OperateImage(dstImage, size, src, offset ?? (0, 0, 0), rowPitch, slicePitch, events, true, Cl.clEnqueueWriteImage);
		}

		public Event WriteImage3D(Mem dstImage, (Size Width, Size Height, Size Depth) size, Array src, Size srcByteOffset, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			using (var pin = src.ToPinned()) {
				return WriteImage3D(dstImage, size, (IntPtr) pin + srcByteOffset, offset, rowPitch, rowPitch, events);
			}
		}

		public void WriteImage3DBlocking(Mem dstImage, (Size Width, Size Height, Size Depth) size, IntPtr src, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			OperateImage(dstImage, size, src, offset ?? (0, 0, 0), rowPitch, slicePitch, events, false, Cl.clEnqueueWriteImage);
		}

		public void WriteImage3DBlocking(Mem dstImage, (Size Width, Size Height, Size Depth) size, Array src, Size srcByteOffset, (Size X, Size Y, Size Z)? offset = null, Size? rowPitch = null, Size? slicePitch = null, params Event[] events) {
			using (var pin = src.ToPinned()) {
				WriteImage3DBlocking(dstImage, size, (IntPtr) pin + srcByteOffset, offset, rowPitch, rowPitch, events);
			}
		}

		public Event RunNDRangeKernel(Kernel kernel, int workDim, Size[] globalWorkSizes, Size[] globalWorkOffsets = null, Size[] localWorkSizes = null, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);

			ClEvent* @event;
			Cl.clEnqueueNDRangeKernel(commandQueue, kernel.kernel, workDim, globalWorkOffsets, globalWorkSizes, localWorkSizes, events.Length, (ClEvent**) ptrs, &@event).Check();
			return new Event(@event, Context);
		}

		public Event Run1DRangeKernel(Kernel kernel, Size globalWorkSize, Size? globalWorkOffset = null, Size? localWorkSize = null, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);

			ClEvent* @event;
			Size gWorkOffset = globalWorkOffset ?? 0;
			Size lWorkOffset = localWorkSize ?? 0;
			Cl.clEnqueueNDRangeKernel(commandQueue, kernel.kernel, 1, &gWorkOffset, &globalWorkSize, localWorkSize.HasValue ? &lWorkOffset : null, (uint) events.Length, (ClEvent**) ptrs, &@event).Check();
			return new Event(@event, Context);
		}

		public Event Run2DRangeKernel(Kernel kernel, (Size X, Size Y) globalWorkSizes, (Size X, Size Y)? globalWorkOffsets = null, (Size X, Size Y)? localWorkSizes = null, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);

			ClEvent* @event;
			Size* pGwo = stackalloc Size[2];
			Size* pGws = stackalloc Size[2];
			Size* pLwo = stackalloc Size[2];
			if (globalWorkOffsets.HasValue) (pGwo[0], pGwo[1]) = globalWorkOffsets.Value;
			(pGws[0], pGws[1]) = globalWorkSizes;
			if (localWorkSizes.HasValue) (pLwo[0], pLwo[1]) = localWorkSizes.Value;

			Cl.clEnqueueNDRangeKernel(commandQueue, kernel.kernel, 2, globalWorkOffsets.HasValue ? pGwo : null, pGws, localWorkSizes.HasValue ? pLwo : null, (uint) events.Length, (ClEvent**) ptrs, &@event).Check();
			return new Event(@event, Context);
		}

		public Event Run3DRangeKernel(Kernel kernel, (Size X, Size Y, Size Z) globalWorkSizes, (Size X, Size Y, Size Z)? globalWorkOffsets = null, (Size X, Size Y, Size Z)? localWorkSizes = null, params Event[] events) {
			IntPtr* ptrs = stackalloc IntPtr[events.Length];
			events.FillPoints(ptrs, e => (IntPtr) e.@event);

			ClEvent* @event;
			Size* pGwo = stackalloc Size[3];
			Size* pGws = stackalloc Size[3];
			Size* pLwo = stackalloc Size[3];
			if (globalWorkOffsets.HasValue) (pGwo[0], pGwo[1], pGwo[2]) = globalWorkOffsets.Value;
			(pGws[0], pGws[1], pGws[2]) = globalWorkSizes;
			if (localWorkSizes.HasValue) (pLwo[0], pLwo[1], pLwo[2]) = localWorkSizes.Value;

			Cl.clEnqueueNDRangeKernel(commandQueue, kernel.kernel, 3, globalWorkOffsets.HasValue ? pGwo : null, pGws, localWorkSizes.HasValue ? pLwo : null, (uint) events.Length, (ClEvent**) ptrs, &@event).Check();
			return new Event(@event, Context);
		}
	}
}
