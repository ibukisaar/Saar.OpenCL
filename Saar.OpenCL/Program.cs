using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Saar.OpenCL {
	unsafe public sealed class Program : ContainerOf<Kernel>, IGetInfo, IHandle {
		public sealed class BuildInfo : IGetInfo {
			public Program Program { get; }
			public Device Device { get; }
			public BuildStatus BuildStatus => (BuildStatus) this.GetUInt32(ProgramBuildInfo.Status);
			public string BuildOptions => this.GetString(ProgramBuildInfo.Options);
			public string BuildLog => this.GetString(ProgramBuildInfo.Log);

			public BuildInfo(Program program, Device device) {
				Program = program;
				Device = device;
			}

			public void GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
				Cl.clGetProgramBuildInfo(Program.program, Device.device, (ProgramBuildInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
			}

			public override string ToString() => BuildStatus.ToString();
		}


		internal ClProgram* program;
		private readonly Dictionary<string, Kernel> nameToKernel = new Dictionary<string, Kernel>();

		IntPtr IHandle.Handle => (IntPtr) program;

		[Obsolete("在CSharp中不需要使用这个属性")]
		public int RefCount => (int) this.GetUInt32(ProgramInfo.ReferenceCount);

		public Context Context { get; }

		private int? numDevices;
		public int NumDevices => this.GetInt32(ref numDevices, ProgramInfo.NumDevices);

		private string source;
		public string Source => this.GetString(ref source, ProgramInfo.Source);

		private IReadOnlyList<byte[]> binaries;
		public IReadOnlyList<byte[]> Binaries {
			get {
				if (binaries == null) {
					int bytesArrCount = NumDevices;
					var binarySizes = this.GetSizes(ProgramInfo.BinarySizes, bytesArrCount);

					byte[][] bytesArr = new byte[bytesArrCount][];
					for (int i = 0; i < bytesArrCount; i++) bytesArr[i] = new byte[binarySizes[i]];

					GCHandle* hanlders = stackalloc GCHandle[bytesArrCount];
					IntPtr* bytesPtrs = stackalloc IntPtr[bytesArrCount];
					try {
						for (int i = 0; i < bytesArrCount; i++) {
							hanlders[i] = GCHandle.Alloc(bytesArr[i], GCHandleType.Pinned);
							bytesPtrs[i] = hanlders[i].AddrOfPinnedObject();
						}
						Cl.clGetProgramInfo(program, ProgramInfo.Binaries, IntPtr.Size * bytesArrCount, (IntPtr) bytesPtrs, out _);
						binaries = Array.AsReadOnly(bytesArr);
					} finally {
						for (int i = 0; i < bytesArrCount; i++) {
							if (hanlders[i].IsAllocated) hanlders[i].Free();
						}
					}
				}
				return binaries;
			}
		}
		

		public IReadOnlyList<Kernel> Kernels => GetObjects<Kernel>();

		public IReadOnlyList<BuildInfo> BuildInfos { get; }

		private Program(ClProgram* program, Context context, IEnumerable<Device> devices = null, string options = null) {
			try {
				this.program = program;
				Context = context;
				context.AddObject(this);

				var devs = (devices ?? context.Devices).ToArray();
				ClDevice** devicesPtr = stackalloc ClDevice*[devs.Length];
				var buildInfos = new BuildInfo[devs.Length];
				for (int i = 0; i < devs.Length; i++) {
					devicesPtr[i] = devs[i].device;
					buildInfos[i] = new BuildInfo(this, devs[i]);
				}
				BuildInfos = Array.AsReadOnly(buildInfos);

				var err = Cl.clBuildProgram(program, (uint) devs.Length, devicesPtr, options, null, IntPtr.Zero);
				if (err != ErrorCode.Success) {
					string errorLog = string.Join(System.Environment.NewLine, buildInfos.Select(info => info.BuildLog));
					throw new ClException(err, errorLog);
				}
			} catch {
				Dispose();
				throw;
			}
		}

		protected override void Dispose(bool disposing) {
			if (program != null) {
				Context.RemoveObject(this);
				Cl.clReleaseProgram(program);
				program = null;
			}
			base.Dispose(disposing);
		}

		public static Program Create(Context context, string[] clCodes, IEnumerable<Device> devices, string options) {
			var program = Cl.clCreateProgramWithSource(context.context, (uint) clCodes.Length, clCodes, Array.ConvertAll(clCodes, s => (Size) s.Length), out var err);
			err.Check();
			return new Program(program, context, devices, options);
		}

		public static Program Create(Context context, params string[] clCodes) {
			return Create(context, clCodes, null, null);
		}

		public IReadOnlyDictionary<string, Kernel> CreateKernels() {
			Cl.clCreateKernelsInProgram(program, 0, null, out var numKernels);
			ClKernel** kernelsPtr = stackalloc ClKernel*[(int) numKernels];
			Cl.clCreateKernelsInProgram(program, numKernels, kernelsPtr, out _);
			for (int i = 0; i < numKernels; i++) new Kernel(kernelsPtr[i], this); // add kernel to program
			return new ReadOnlyDictionary<string, Kernel>(nameToKernel);
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetProgramInfo(program, (ProgramInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		protected override void OnAddObject(IHandle obj) {
			if (obj is Kernel kernel) nameToKernel.Add(kernel.FunctionName, kernel);
			base.OnAddObject(obj);
		}

		protected override bool OnRemoveObject(IHandle obj) {
			if (obj is Kernel kernel) nameToKernel.Remove(kernel.FunctionName);
			return base.OnRemoveObject(obj);
		}

		public Kernel this[string kernelName] {
			get {
				if (nameToKernel.TryGetValue(kernelName, out var kernel)) {
					return kernel;
				}
				return Kernel.Create(this, kernelName);
			}
		}
	}
}
