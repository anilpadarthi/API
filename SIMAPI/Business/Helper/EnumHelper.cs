using System.ComponentModel;

namespace SIMAPI.Business.Helper
{
	public static class EnumHelper
	{
		public static string? GetName(this Enum e)
		{
			return Enum.GetName(e.GetType(), e);
		}

		public static string? GetDescription(this Enum e)
		{
			var fieldInfo = e.GetType().GetField(e.GetName() ?? string.Empty);
			if (fieldInfo is null)
				return e.GetName();

			var descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
			return descriptionAttribute == null ? e.GetName() : descriptionAttribute.Description;
		}

		public static bool IsEqualInt(this Enum e, int? value)
		{
			if (Convert.ToInt32(e) == value)
			{
				return true;
			}
			return false;
		}

		public static bool IsEqualLong(this Enum e, long? value)
		{
			if (Convert.ToInt64(e) == value)
			{
				return true;
			}
			return false;
		}
	}
}
