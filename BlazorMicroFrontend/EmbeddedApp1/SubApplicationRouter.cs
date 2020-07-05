using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmbeddedApp1
{
    public class SubApplicationRouter : IComponent, IHandleAfterRender, IDisposable
    {
        private RenderHandle _renderHandle;
        private string _baseUri;
        private string _locationAbsolute;

        [Inject] private NavigationManager _navigationManager { get; set; }

        [Parameter] public RenderFragment<RouteData> SubApplication { get; set; }

        [Parameter] public RenderFragment MainApplication { get; set; }


        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _baseUri = _navigationManager.BaseUri;
            _locationAbsolute = _navigationManager.Uri;

            _navigationManager.LocationChanged += OnLocationChanged;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (MainApplication == null)
            {
                throw new InvalidOperationException($"The {nameof(SubApplicationRouter)} component requires a value for the parameter {nameof(MainApplication)}.");
            }

            Refresh();
            return Task.CompletedTask;
        }

        private void Refresh()
        {
            var locationPath = _navigationManager.ToBaseRelativePath(_locationAbsolute);

            if (locationPath.StartsWith("_app/"))
            {
                var match = Regex.Match(locationPath, @"^_app\/(?'key'[^\/]*)(?'suffix'.*)");

                if (match.Success)
                {
                    
                    var suffix = match.Groups["suffix"].Value.TrimStart('/');

                    RouteData routeData = new RouteData(typeof(MainNavigator), new Dictionary<string, object>() { { "Url", locationPath } });

                    _renderHandle.Render(SubApplication(routeData));
                }
            }
            else
            {
                _renderHandle.Render(MainApplication);
            }
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _locationAbsolute = args.Location;
            Refresh();
        }

        public Task OnAfterRenderAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _navigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
