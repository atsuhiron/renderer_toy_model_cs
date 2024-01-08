namespace RendererToyModelCs.Extension
{
    public static class EnumerableExt
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
            where T : class
        {
            if (source == null)
            {
                return Enumerable.Empty<T>();
            }

            return source.Where(x => x != null)!;
        }
    }
}
