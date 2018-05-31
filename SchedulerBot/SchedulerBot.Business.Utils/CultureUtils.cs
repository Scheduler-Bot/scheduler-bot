using System.Globalization;

namespace SchedulerBot.Business.Utils
{
	/// <summary>
	/// Provides utility methods for working with culture and localization.
	/// </summary>
	public static class CultureUtils
	{
		private static readonly CultureInfo DefaultCultureInfo = CultureInfo.GetCultureInfo("en-US");

		/// <summary>
		/// Gets the culture information by the specified name or returns the default culture if nothing is found by the name.
		/// </summary>
		/// <param name="name">The culture name (e.g. en-US).</param>
		/// <returns>The <see cref="CultureInfo"/> instance found by the name or the default culture.</returns>
		public static CultureInfo GetCultureInfoOrDefault(string name)
		{
			CultureInfo cultureInfo;

			if (string.IsNullOrWhiteSpace(name))
			{
				cultureInfo = DefaultCultureInfo;
			}
			else
			{
				try
				{
					cultureInfo = CultureInfo.GetCultureInfo(name);
				}
				catch (CultureNotFoundException)
				{
					cultureInfo = DefaultCultureInfo;
				}
			}

			return cultureInfo;
		}
	}
}
