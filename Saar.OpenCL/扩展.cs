using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe internal static class 扩展 {
		public static void Check(this ErrorCode errCode) {
			if (errCode != ErrorCode.Success) throw new ClException(errCode);
		}

		public static PinnedObject ToPinned(this object obj) {
			return new PinnedObject(obj);
		}

		public static Points<T> ToPoints<T>(this IEnumerable<T> objs, Converter<T, IntPtr> converter) {
			return new Points<T>(objs, converter);
		}

		public static void FillPoints<T>(this T[] objs, IntPtr* ptrs, Converter<T, IntPtr> converter) {
			for (int i = 0; i < objs.Length; i++) {
				ptrs[i] = converter(objs[i]);
			}
		}
	}
}
