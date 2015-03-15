namespace IWantMoreNames
{
	public static class StringUtils
	{
		public static bool IsNullOrWhitespace(this string s)
		{
			return s == null || s.Trim().Length == 0;
		}
	}
}