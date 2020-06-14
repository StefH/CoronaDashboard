using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CoronaDashboard.Services
{
    public class JavaScriptInteropService
    {
        private readonly IJSRuntime _runtime;
        private readonly DotNetObjectReference<JavaScriptInteropService> _dotNetObjectReference;

        public JavaScriptInteropService(IJSRuntime runtime)
        {
            _runtime = runtime;

            _dotNetObjectReference = DotNetObjectReference.Create(this);
        }

        public event Func<Task> OnResizeOrRotate;

        [JSInvokable]
        public async Task OnBrowserResizeOrRotate()
        {
            if (OnResizeOrRotate != null)
            {
                await OnResizeOrRotate.Invoke();
            }
        }

        public ValueTask SetEventListener()
        {
            return _runtime.InvokeVoidAsync("setEventListeners", _dotNetObjectReference);
        }

        public async Task<int> GetClientHeight(ElementReference element)
        {
            return await _runtime.InvokeAsync<int>("getClientHeight", element);
        }

        public async Task<int> GetTop(ElementReference element)
        {
            return await _runtime.InvokeAsync<int>("getTop", element);
        }

        public async Task SetClientHeight(ElementReference element, int? height = null)
        {
            await _runtime.InvokeVoidAsync("setClientHeight", element, height != null ? $"{height}px" : "auto");
        }
    }
}