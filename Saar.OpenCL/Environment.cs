using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	public sealed class Environment : DisposableObject {
		public Context Context { get; }
		public IReadOnlyList<CommandQueue> CommandQueues { get; }
		public CommandQueue MainCommandQueue { get; }
		public Program Program { get; }

		/// <summary>
		/// Alias of <see cref="Context"/>.
		/// </summary>
		public Context Ctx => Context;
		/// <summary>
		/// Alias of <see cref="MainCommandQueue"/>.
		/// </summary>
		public CommandQueue Cmd => MainCommandQueue;


		public Environment(string code, DeviceType deviceType = DeviceType.Gpu, string options = null) {
			try {
				Context = Context.Create(Platform.MainPlatform, deviceType);
				CommandQueues = Context.CreateCommandQueues();
				MainCommandQueue = CommandQueues.FirstOrDefault();
				Program = Program.Create(Context, new string[] { code }, Context.Devices, options);
			} catch {
				Dispose();
				throw;
			}
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				Context.Dispose();
			}
		}

		public Kernel this[string kernelName] => Program[kernelName];
	}
}
