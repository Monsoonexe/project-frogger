/// <summary>
/// Pregenerates common integer strings to avoid garbge generation.
/// </summary>
public static class ConstStrings
{
    private static readonly string[] cachedIntStrings;

    /// <summary>
    /// Caches [0, N], N + 1 values of pre-generated strings.
    /// </summary>
    private const int CACHED_INT_COUNT = 100;//100 covers all times, percents.

    static ConstStrings()
    {
        cachedIntStrings = new string[CACHED_INT_COUNT + 1];

        for (var i = 0; i < CACHED_INT_COUNT + 1; ++i)
            cachedIntStrings[i] = i.ToString();//pre gen strings to avoid runtime allocation
    }

    public static string GetCachedString(int val)
    {
        string outStr = string.Empty;

        //hooray collision-free hashing function! (plain-old array access).
        if (val >= 0 && val <= CACHED_INT_COUNT)
            outStr = cachedIntStrings[val]; //string was already gen'd
        else
            outStr = val.ToString(); //can't cache 'em all
        return outStr;
    }
}
