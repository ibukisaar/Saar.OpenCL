using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saar.OpenCL {
	unsafe public abstract class ContainerOf<TClass_or_TValueTuple> : DisposableObject {
		private static readonly HashSet<Type> ValidTypes;

		static ContainerOf() {
			var T = typeof(TClass_or_TValueTuple);

			if (!T.IsGenericType && T.IsClass && typeof(IHandle).IsAssignableFrom(T)) {
				ValidTypes = new HashSet<Type>() { T };
				return;
			}

			if (!T.IsGenericType || T.IsClass || !T.Name.StartsWith("ValueTuple")) goto Fail;

			var types = T.GetGenericArguments();
			if (types.Distinct().Count() != types.Length || types.Any(t => !typeof(IHandle).IsAssignableFrom(t))) goto Fail;

			ValidTypes = new HashSet<Type>(types);
			return;
Fail:
			throw new TypeInitializationException(
				nameof(ContainerOf<TClass_or_TValueTuple>),
				new Exception($"{T}必须是class或者ValueTuple。class：实现 {typeof(IHandle)} 接口；ValueTuple：每个类型参数要实现 {typeof(IHandle)} 接口。")
				);
		}


		private Dictionary<IntPtr, IHandle> objectDict = new Dictionary<IntPtr, IHandle>();
		private Dictionary<Type, IEnumerable> cache = new Dictionary<Type, IEnumerable>();

		protected IReadOnlyList<T> GetObjects<T>() {
			if (cache.TryGetValue(typeof(T), out var ret)) {
				return ret as IReadOnlyList<T>;
			}
			ret = Array.AsReadOnly(objectDict.Values.OfType<T>().ToArray());
			cache.Add(typeof(T), ret);
			return ret as IReadOnlyList<T>;
		}

		internal void AddObject(IHandle obj) {
			CheckObject(obj);
			OnAddObject(obj);
		}

		internal bool RemoveObject(IHandle obj) {
			CheckObject(obj);
			return OnRemoveObject(obj);
		}

		private void CheckObject(IHandle obj) {
			if (obj.GetType() is var type && !ValidTypes.Contains(type))
				throw new InvalidOperationException($"无法添加或移除 {type} 类型的对象");
		}

		protected virtual void OnAddObject(IHandle obj) {
			objectDict.Add(obj.Handle, obj);
			OnCollectionChanged(obj);
		}

		protected virtual bool OnRemoveObject(IHandle obj) {
			if (objectDict.Remove(obj.Handle)) {
				OnCollectionChanged(obj);
				return true;
			}
			return false;
		}

		protected virtual void OnCollectionChanged(IHandle obj) {
			cache.Remove(obj.GetType());
		}

		internal protected T Get<T>(IntPtr key) where T : IHandle => (T) objectDict[key];

		protected override void Dispose(bool disposing) {
			if (disposing) {
				foreach (var obj in objectDict.Values.ToArray()/*copy values*/) {
					obj.Dispose();
				}
			}
		}
	}
}
