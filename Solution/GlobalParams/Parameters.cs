using System.Collections.Generic;

namespace GlobalParams
{
	/// <summary>Storage for solution parameters.</summary>
	public static class Parameters
	{
		#region Member Variables

		/// <summary>Private storage for the solution parameters.</summary>
		private static Dictionary<string, object> _parameters = new Dictionary<string, object>();

		#endregion Member Variables

		#region Properties

		#region All
		/// <summary>Return the underlying parameter collection.</summary>
		internal static Dictionary<string, object> All
		{
			get
			{
				return _parameters;
			}
		}
		#endregion All

		#endregion Properties

		#region Methods

		#region Get
		/// <summary>Gets the parameter by the specified <paramref name="key"/>.</summary>
		/// <param name="key">The key.</param>
		/// <returns>The parameter or an empty string.</returns>
		public static object Get(string key)
		{
			object retVal = string.Empty;
			string keyInternal = key.Globalize();

			retVal = HasPrivate(keyInternal) ? _parameters[keyInternal] : string.Empty;

			return retVal;
		}
		#endregion Get

		#region Set
		/// <summary>Sets or the parameter by the specified <paramref name="key"/> to the specified <paramref name="value"/>.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public static void Set(string key, object value)
		{
			string keyInternal = key.Globalize();

			if (Constants.SAFE_PROJECT_KEY.Equals(key) && value != null)
			{
				value = value.ToString().Replace(" ", "_");
			}

			if (!string.IsNullOrEmpty(keyInternal))
			{
				if (HasPrivate(keyInternal)) { _parameters[keyInternal] = value; }
				else { _parameters.Add(keyInternal, value); }
			}
		}
		#endregion Set

		#region Has
		/// <summary>Determines if parameter collection has an entry with the globalized version of the specified <paramref name="key"/>.</summary>
		/// <param name="key">The key.</param>
		/// <returns>True if the an entry was found, otherwise false.</returns>
		public static bool Has(string key)
		{
			return HasPrivate(key.Globalize());
		}
		#endregion Has

		#region HasPrivate
		/// <summary>Determines if there is an entry with the specified <paramref name="key"/> in the parameter collection.</summary>
		/// <param name="key">The key.</param>
		/// <returns>True if the an entry was found, otherwise false.</returns>
		private static bool HasPrivate(string key)
		{
			return (_parameters != null && !string.IsNullOrEmpty(key) && _parameters.ContainsKey(key));
		}
		#endregion HasPrivate

		#region Globalize
		/// <summary>Creates a global parameter key from the specified <paramref name="value"/>.</summary>
		/// <param name="value">The original key to globalize.</param>
		/// <returns>A string value ready to be used as a global parameter key, or null.</returns>
		public static string Globalize(this string value)
		{
			string retVal = string.Empty;

			if (!string.IsNullOrEmpty(value))
			{
				retVal = string.Format(Constants.FORMAT_GLOBAL_KEY, value.TrimStart('$').TrimEnd('$'));
			}

			return retVal;
		}
		#endregion Globalize

		#region Clear
		/// <summary>Clears the underlying parameter collection.</summary>
		internal static void Clear()
		{
			if (_parameters != null)
			{
				_parameters.Clear();
			}
		}
		#endregion Clear

		#endregion Methods
	}
}