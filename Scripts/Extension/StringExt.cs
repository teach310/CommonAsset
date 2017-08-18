public static class StringExt
{
	public static string ToTitleCase(this string self)
	{
		return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(self);
	}
}