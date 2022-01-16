using System.Collections.Generic;
using System.Collections.Specialized;

public static class NameValueCollectionExtensions
{
    /// <summary>
    ///     A NameValueCollection extension method that converts the @this to a dictionary.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an IDictionary&lt;string,string&gt;</returns>
    public static IDictionary<string, string> ToDictionary(this NameValueCollection @this)
    {
        var dict = new Dictionary<string, string>();

        foreach (string key in @this.AllKeys)
        {
            dict.Add(key, @this[key]);
        }

        return dict;
    }
}
