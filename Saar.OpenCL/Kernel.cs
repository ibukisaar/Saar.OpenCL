using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public sealed class Kernel : DisposableObject, IGetInfo, IHandle {
		public class Arg : IGetInfo {
			public Kernel Kernel { get; }
			public int Index { get; }

			private string name;
			public string Name => this.GetString(ref name, KernelArgInfo.Name);

			private string typeName;
			public string TypeName => this.GetString(ref typeName, KernelArgInfo.TypeName);

			private KernelArgAddressQualifier? addressQualifier;
			public KernelArgAddressQualifier AddressQualifier => addressQualifier ?? (addressQualifier = (KernelArgAddressQualifier) this.GetUInt32(KernelArgInfo.AddressQualifier)).Value;

			private KernelArgAccessQualifier? accessQualifier;
			public KernelArgAccessQualifier AccessQualifier => accessQualifier ?? (accessQualifier = (KernelArgAccessQualifier) this.GetUInt32(KernelArgInfo.AccessQualifier)).Value;

			private KernelArgTypeQualifier? typeQualifier;
			public KernelArgTypeQualifier TypeQualifier => typeQualifier ?? (typeQualifier = (KernelArgTypeQualifier) this.GetUInt32(KernelArgInfo.TypeQualifier)).Value;

			internal Arg(Kernel kernel, int index) {
				Kernel = kernel;
				Index = index;
			}

			void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
				Cl.clGetKernelArgInfo(Kernel.kernel, Index, (KernelArgInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
			}

			public override string ToString()
				=> $"{TypeName} {Name}";
		}


		internal ClKernel* kernel;
		IntPtr IHandle.Handle => (IntPtr) kernel;

		public Program Program { get; }

		public Context Context { get; }

		public int RefCount => (int) this.GetUInt32(KernelInfo.ReferenceCount);

		private string functionName;
		public string FunctionName => this.GetString(ref functionName, KernelInfo.FunctionName);

		public IReadOnlyList<Arg> Args;

		internal Kernel(ClKernel* kernel, Program program) {
			try {
				Cl.clRetainKernel(kernel);
				this.kernel = kernel;
				Program = program;
				Context = program.Context;

				program.AddObject(this);

				int numArgs = (int) this.GetUInt32(KernelInfo.NumArgs);
				var args = new Arg[numArgs];
				for (int i = 0; i < numArgs; i++) {
					args[i] = new Arg(this, i);
				}
				Args = Array.AsReadOnly(args);
			} catch {
				Dispose();
				throw;
			}
		}

		protected override void Dispose(bool disposing) {
			if (kernel != null) {
				Program.RemoveObject(this);
				Cl.clReleaseKernel(kernel);
				kernel = null;
			}
		}

		public static Kernel Create(Program program, string kernelName) {
			var kernel = Cl.clCreateKernel(program.program, kernelName, out var err);
			err.Check();
			return new Kernel(kernel, program);
		}

		public override string ToString() => $"{FunctionName}({string.Join(", ", Args)})";

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetKernelInfo(kernel, (KernelInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		public void SetArg(int index, Mem mem) {
			fixed (ClMem** memPtr = &mem.mem) {
				Cl.clSetKernelArg(kernel, (uint) index, IntPtr.Size, memPtr).Check();
			}
		}

		public void SetArg<T>(int index, T obj) where T : struct {
			using (var pin = obj.ToPinned()) {
				Cl.clSetKernelArg(kernel, (uint) index, TypeSize<T>.Value, pin).Check();
			}
		}

		public void SetArgs(params (int Index, Mem Mem)[] args) {
			foreach (var arg in args) SetArg(arg.Index, arg.Mem);
		}

		public void SetArgs<T>(params (int Index, T Arg)[] args) where T : struct {
			foreach (var arg in args) SetArg(arg.Index, arg.Arg);
		}

		public void SetArgs(params object[] args) {
			for (int i = 0; i < args.Length; i++) {
				if (args[i] is Mem mem) {
					SetArg(i, mem);
				} else {
					using (var pin = args[i].ToPinned()) {
						Cl.clSetKernelArg(kernel, (uint) i, Marshal.SizeOf(args[i]), pin).Check();
					}
				}
			}
		}
	}
}
