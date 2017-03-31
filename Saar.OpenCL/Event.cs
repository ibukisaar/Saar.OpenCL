using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public sealed class Event : DisposableObject, IGetInfo, IHandle {
		internal ClEvent* @event;
		IntPtr IHandle.Handle => (IntPtr) @event;

		public Context Context { get; }

		[Obsolete("在CSharp中不需要使用这个属性")]
		public int RefCount => (int) this.GetUInt32(EventInfo.ReferenceCount);

		public CommandType CommandType => (CommandType) this.GetUInt32(EventInfo.CommandType);

		public CommandQueue CommandQueue => Context.Get<CommandQueue>(this.GetIntPtr(EventInfo.CommandQueue));

		public ExecutionStatus CommandExecutionStatus => (ExecutionStatus) this.GetUInt32(EventInfo.CommandExecutionStatus);

		internal Event(ClEvent* @event, Context context) {
			try {
				this.@event = @event;
				Context = context;
				context.AddObject(this);
			} catch {
				Dispose();
				throw;
			}
		}

		protected override void Dispose(bool disposing) {
			if (@event != null) {
				Context.RemoveObject(this);
				Cl.clReleaseEvent(@event);
				@event = null;
			}
		}

		public static Event Create(Context context) {
			var @event = Cl.clCreateUserEvent(context.context, out var err);
			err.Check();
			return new Event(@event, context);
		}

		void IGetInfo.GetInfo(Enum prop, Size paramValueSize, IntPtr paramValue, out Size paramValueSizeRet) {
			Cl.clGetEventInfo(@event, (EventInfo) prop, paramValueSize, paramValue, out paramValueSizeRet);
		}

		public void Wait() {
			fixed (ClEvent** e = &@event) {
				Cl.clWaitForEvents(1, e).Check();
			}
		}
	}
}
