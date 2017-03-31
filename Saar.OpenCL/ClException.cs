using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	public class ClException : Exception {
		public ErrorCode ErrorCode { get; }

		public ClException(ErrorCode errCode) : base(errCode.ToString()) {
			this.ErrorCode = errCode;
		}

		public ClException(ErrorCode errCode, string message) : base($"{errCode}{System.Environment.NewLine}{message}") {
			this.ErrorCode = errCode;
			Debug.Print(message);
		}
	}
}
