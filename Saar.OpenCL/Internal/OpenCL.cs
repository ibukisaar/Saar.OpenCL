using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Saar.OpenCL {
#pragma warning disable IDE1006 // 命名样式
	unsafe public static partial class Cl {
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clBuildProgram(ClProgram* program, uint numDevices, ClDevice** deviceList, string options, BuildProgramCallback pfnNotify, IntPtr userData);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClMem* clCreateBuffer(ClContext* context, MemFlags flags, Size size, IntPtr hostPtr, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClCommandQueue* clCreateCommandQueue(ClContext* context, ClDevice* device, CommandQueueProperties properties, out ErrorCode error);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClContext* clCreateContext(ClContextProperty* properties, uint numDevices, ClDevice** devices, CreateContextCallback pfnNotify, IntPtr userData, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClContext* clCreateContextFromType(ClContextProperty* properties, DeviceType deviceType, CreateContextCallback pfnNotify, IntPtr userData, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClMem* clCreateImage2D(ClContext* context, MemFlags flags, ClImageFormat* imageFormat, Size imageWidth, Size imageHeight, Size imageRowPitch, IntPtr hostPtr, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClMem* clCreateImage3D(ClContext* context, MemFlags flags, ClImageFormat* imageFormat, Size imageWidth, Size imageHeight, Size imageDepth, Size imageRowPitch, Size imageSlicePitch, IntPtr hostPtr, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClKernel* clCreateKernel(ClProgram* program, string kernelName, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clCreateKernelsInProgram(ClProgram* program, uint numKernels, ClKernel** kernels, out uint numKernelsRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClProgram* clCreateProgramWithBinary(ClContext* context, uint numDevices, ClDevice** deviceList, IntPtr* lengths, IntPtr binaries, IntPtr binaryStatus, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClProgram* clCreateProgramWithSource(ClContext* context, uint count, string[] strings, Size[] lengths, out ErrorCode errcodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr clCreateSampler(ClContext* context, bool normalizedCoords, AddressingMode addressingMode, FilterMode filterMode, out ErrorCode errCodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ClEvent* clCreateUserEvent(ClContext* context, out ErrorCode err);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueBarrier(ClCommandQueue* commandQueue);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueCopyBuffer(ClCommandQueue* commandQueue, ClMem* srcBuffer, ClMem* dstBuffer, Size srcOffset, Size dstOffset, Size cb, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueCopyBufferToImage(ClCommandQueue* commandQueue, ClMem* srcBuffer, ClMem* dstImage, Size srcOffset, Size* dstOrigin, Size* region, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueCopyImage(ClCommandQueue* commandQueue, ClMem* srcImage, ClMem* dstImage, Size* srcOrigin, Size* dstOrigin, Size* region, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueCopyImageToBuffer(ClCommandQueue* commandQueue, ClMem* srcImage, ClMem* dstBuffer, Size* srcOrigin, Size* region, Size dstOffset, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr clEnqueueMapBuffer(ClCommandQueue* commandQueue, ClMem* buffer, bool blockingMap, MapFlags mapFlags, Size offset, Size cb, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e, out ErrorCode errCodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr clEnqueueMapImage(ClCommandQueue* commandQueue, ClMem* image, bool blockingMap, MapFlags mapFlags, Size* origin, Size* region, ref Size imageRowPitch, ref Size imageSlicePitch, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e, out ErrorCode errCodeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueMarker(ClCommandQueue* commandQueue, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueNDRangeKernel(ClCommandQueue* commandQueue, ClKernel* kernel, uint workDim, Size* globalWorkOffset, Size* globalWorkSize, Size* localWorkSize, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueNDRangeKernel(ClCommandQueue* commandQueue, ClKernel* kernel, int workDim, Size[] globalWorkOffset, Size[] globalWorkSize, Size[] localWorkSize, int numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueReadBuffer(ClCommandQueue* commandQueue, ClMem* buffer, bool blockingRead, Size offsetInBytes, Size lengthInBytes, IntPtr ptr, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueReadImage(ClCommandQueue* commandQueue, ClMem* image, bool blockingRead, Size* origin, Size* region, Size rowPitch, Size slicePitch, IntPtr ptr, uint numEventsIntWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueTask(ClCommandQueue* commandQueue, ClKernel* kernel, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueUnmapMemObject(ClCommandQueue* commandQueue, ClMem* memObj, IntPtr mappedPtr, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueWaitForEvents(ClCommandQueue* commandQueue, uint numEventsInWaitList, ClEvent** eventWaitList);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueWriteBuffer(ClCommandQueue* commandQueue, ClMem* buffer, bool blockingWrite, Size offsetInBytes, Size lengthInBytes, IntPtr ptr, uint numEventsInWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clEnqueueWriteImage(ClCommandQueue* commandQueue, ClMem* image, bool blockingWrite, Size* origin, Size* region, Size rowPitch, Size slicePitch, IntPtr ptr, uint numEventsIntWaitList, ClEvent** eventWaitList, ClEvent** e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clFinish(ClCommandQueue* commandQueue);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clFlush(ClCommandQueue* commandQueue);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetCommandQueueInfo(ClCommandQueue* commandQueue, CommandQueueInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetContextInfo(ClContext* context, ContextInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetDeviceIDs(ClPlatform* platform, DeviceType deviceType, uint numEntries, ClDevice** devices, out uint numDevices);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetDeviceInfo(ClDevice* device, DeviceInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetDeviceInfo(ClDevice* device, DeviceInfo paramName, Size paramValueSize, StringBuilder paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetEventInfo(ClEvent* e, EventInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetEventProfilingInfo(ClEvent* e, ProfilingInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr clGetExtensionFunctionAddress(string funcName);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetImageInfo(ClMem* image, ImageInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetKernelInfo(ClKernel* kernel, KernelInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetKernelArgInfo(ClKernel* kernel, int argIndex, KernelArgInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetKernelWorkGroupInfo(ClKernel* kernel, ClDevice* device, KernelWorkGroupInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetMemObjectInfo(ClMem* memObj, MemInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetPlatformIDs(uint numEntries, ClPlatform** platforms, out uint numPlatforms);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetPlatformInfo(ClPlatform* platform, PlatformInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetPlatformInfo(ClPlatform* platform, PlatformInfo paramName, Size paramValueSize, StringBuilder paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetProgramBuildInfo(ClProgram* program, ClDevice* device, ProgramBuildInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetProgramInfo(ClProgram* program, ProgramInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetSamplerInfo(IntPtr sampler, SamplerInfo paramName, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clGetSupportedImageFormats(ClContext* context, MemFlags flags, MemObjectType imageType, uint numEntries, ClImageFormat* imageFormats, out uint numImageFormats);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseCommandQueue(ClCommandQueue* commandQueue);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseContext(ClContext* context);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseEvent(ClEvent* e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseKernel(ClKernel* kernel);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseMemObject(ClMem* memObj);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseProgram(ClProgram* program);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clReleaseSampler(IntPtr sampler);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainCommandQueue(ClCommandQueue* commandQueue);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainContext(ClContext* context);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainEvent(ClEvent* e);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainKernel(ClKernel* kernel);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainMemObject(ClMem* memObj);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainProgram(ClProgram* program);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clRetainSampler(IntPtr sampler);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clSetCommandQueueProperty(ClCommandQueue* commandQueue, CommandQueueProperties properties, bool enable, out CommandQueueProperties oldProperties);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clSetKernelArg(ClKernel* kernel, uint argIndex, Size argSize, void* argValue);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clSetUserEventStatus(ClEvent* ev, int executionStatus);
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clUnloadCompiler();
		[DllImport(Dll_OpenCL, CallingConvention = Convention)]
		[SuppressUnmanagedCodeSecurity]
		public static extern ErrorCode clWaitForEvents(uint numEvents, ClEvent** eventWaitList);
	}
#pragma warning restore IDE1006 // 命名样式
}
