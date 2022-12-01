namespace System
{
    internal class AttributeExtensions
    {
        public static bool IsDecoratedWithAttribute<TType, TAttribute>(Func<TAttribute, bool> attributeFunc)
            where TAttribute : Attribute
            // where TType : class
        {
            var type = typeof(TType);
            var customAttributes = type.GetCustomAttributes(false);
            var attribute = customAttributes.OfType<TAttribute>().FirstOrDefault();
            return attributeFunc(attribute!);
        }

        public static bool IsDecoratedWithAttribute<TType, TAttribute>(Func<TAttribute, bool> attributeFunc, string methodName)
        where TAttribute : Attribute
        // where TType : class
        {
            var type = typeof(TType);
            var methodInfo = type.GetMethod(methodName);
            var customAttributes = methodInfo!.GetCustomAttributes(false);
            var attribute = customAttributes.OfType<TAttribute>().FirstOrDefault();
            return attributeFunc(attribute!);
        }
    }
}