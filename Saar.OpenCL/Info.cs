using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe internal static class Info {
		public static bool GetBoolean(this IGetInfo obj, Enum prop) {
			return GetUInt32(obj, prop) != 0;
		}

		public static bool GetBoolean(this IGetInfo obj, ref bool? field, Enum prop)
			=> field ?? (field = GetBoolean(obj, prop)).Value;


		public static uint GetUInt32(this IGetInfo obj, Enum prop) {
			uint ret = 0;
			obj.GetInfo(prop, sizeof(uint), (IntPtr) (&ret), out _);
			return ret;
		}

		public static uint GetUInt32(this IGetInfo obj, ref uint? field, Enum prop)
			=> field ?? (field = GetUInt32(obj, prop)).Value;

		public static int GetInt32(this IGetInfo obj, ref int? field, Enum prop)
			=> field ?? (field = (int) GetUInt32(obj, prop)).Value;


		public static ulong GetUInt64(this IGetInfo obj, Enum prop) {
			ulong ret = 0;
			obj.GetInfo(prop, sizeof(ulong), (IntPtr) (&ret), out _);
			return ret;
		}

		public static ulong GetUInt64(this IGetInfo obj, ref ulong? field, Enum prop)
			=> field ?? (field = GetUInt64(obj, prop)).Value;


		public static IntPtr GetIntPtr(this IGetInfo obj, Enum prop) {
			IntPtr ret = IntPtr.Zero;
			obj.GetInfo(prop, IntPtr.Size, (IntPtr) (&ret), out _);
			return ret;
		}

		public static IntPtr GetIntPtr(this IGetInfo obj, ref IntPtr? field, Enum prop)
			=> field ?? (field = GetIntPtr(obj, prop)).Value;

		public static Size GetSize(this IGetInfo obj, ref Size? field, Enum prop)
			=> field ?? (field = GetIntPtr(obj, prop)).Value;


		public static string GetString(this IGetInfo obj, Enum prop) {
			obj.GetInfo(prop, IntPtr.Zero, IntPtr.Zero, out var strSize);
			if (strSize == 0) return string.Empty;

			if (strSize <= 1024) {
				char* buffer = stackalloc char[(strSize + 1)];
				obj.GetInfo(prop, strSize, (IntPtr) buffer, out _);
				return Marshal.PtrToStringAnsi((IntPtr) buffer);
			} else {
				IntPtr buffer = Marshal.AllocHGlobal((IntPtr) strSize);
				try {
					obj.GetInfo(prop, strSize, buffer, out _);
					return Marshal.PtrToStringAnsi(buffer);
				} finally {
					Marshal.FreeHGlobal(buffer);
				}
			}
		}

		public static string GetString(this IGetInfo obj, ref string field, Enum prop)
			=> field ?? (field = GetString(obj, prop));


		public static IReadOnlyList<T> GetObjects<T>(this IGetInfo obj, Enum prop, Converter<IntPtr, T> converter) {
			obj.GetInfo(prop, IntPtr.Zero, IntPtr.Zero, out var bytes);
			int num = bytes / IntPtr.Size;
			if (num == 0) return Array.Empty<T>();
			IntPtr[] ptrs = new IntPtr[num];
			fixed (IntPtr* pptrs = ptrs) {
				obj.GetInfo(prop, bytes, (IntPtr) pptrs, out _);
			}
			return Array.AsReadOnly(Array.ConvertAll(ptrs, converter));
		}

		public static IReadOnlyList<T> GetObjects<T>(this IGetInfo obj, ref IReadOnlyList<T> field, Enum prop, Converter<IntPtr, T> converter)
			=> field ?? (field = GetObjects(obj, prop, converter));


		public static IReadOnlyList<Size> GetSizes(this IGetInfo obj, Enum prop, Size count) {
			Size[] sizes = new Size[count];
			fixed (Size* ptr = sizes) {
				obj.GetInfo(prop, count * Size.Bytes, (IntPtr) ptr, out _);
			}
			return Array.AsReadOnly(sizes);
		}

		public static IReadOnlyList<Size> GetSizes(this IGetInfo obj, ref IReadOnlyList<Size> field, Enum prop, Size count)
			=> field ?? (field = GetSizes(obj, prop, count));
	}
}
