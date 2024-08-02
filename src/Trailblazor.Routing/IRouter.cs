using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing;

public interface IRouter : IComponent, IHandleAfterRender, IDisposable
{
}
