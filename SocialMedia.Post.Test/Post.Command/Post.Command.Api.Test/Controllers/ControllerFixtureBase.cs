using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Post.Command.Api.Test.Controllers
{
    public class ControllerFixtureBase
    {
        protected void WithHttpPostAttribute<TController>(string methodName, string? template = null) =>
                       WithHttpAttribute<TController, HttpPostAttribute>(methodName, template);

        protected void WithHttpPutAttribute<TController>(string methodName, string? template = null) =>
                       WithHttpAttribute<TController, HttpPutAttribute>(methodName, template);

        protected void WithHttpGetAttribute<TController>(string methodName, string? template = null) =>
                       WithHttpAttribute<TController, HttpGetAttribute>(methodName, template);

        protected void WithHttpDeleteAttribute<TController>(string methodName, string? template = null) =>
                       WithHttpAttribute<TController, HttpDeleteAttribute>(methodName, template);

        private void WithHttpAttribute<TController, THttpAttribute>(string methodName, string? template = null)
        where THttpAttribute : HttpMethodAttribute
        {
            // Arrange
            var attributeFunc = (THttpAttribute attribute) => attribute != null && attribute.Template == template;

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<TController, THttpAttribute>(attributeFunc, methodName);

            // Assert
            decorated.Should().BeTrue();
        }
    }
}