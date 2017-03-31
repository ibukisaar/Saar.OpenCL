using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace Saar.OpenCL {
	/// <summary>
	/// 可通过句柄索引的静态对象
	/// </summary>
	/// <typeparam name="T"></typeparam>
	unsafe public abstract class StaticObject<T> where T : StaticObject<T> {
		private static readonly Dictionary<IntPtr, T> PtrToObject = new Dictionary<IntPtr, T>();

		protected static T Get(IntPtr internalIntPtr, Func<IntPtr, T> ctor) {
			if (PtrToObject.TryGetValue(internalIntPtr, out var ret)) {
				return ret;
			}
			ret = ctor(internalIntPtr);
			PtrToObject.Add(internalIntPtr, ret);
			return ret;
		}
	}
}
