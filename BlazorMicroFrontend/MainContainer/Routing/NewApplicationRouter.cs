#nullable disable warnings

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace MainContainer.Routing
{
    public class NewApplicationRouter : IComponent, IHandleAfterRender, IDisposable
    {
        private RenderHandle _renderHandle;
        private string _baseUri;
        private string _locationAbsolute;
        private bool _navigationInterceptionEnabled;
        private ILogger<NewApplicationRouter> _logger;

        private CancellationTokenSource _onNavigateCts;

        private Task _previousOnNavigateTask = Task.CompletedTask;

        private readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();

        private bool _onNavigateCalled = false;

        [Inject] public ISubApplicationManager SubApplicationManager { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private INavigationInterception NavigationInterception { get; set; }

        [Inject] private ILoggerFactory LoggerFactory { get; set; }

        [Parameter] public Assembly AppAssembly { get; set; }

        [Parameter] public IEnumerable<Assembly> AdditionalAssemblies { get; set; }


        [Parameter] public RenderFragment<Uri> SubApplication { get; set; }

        [Parameter] public RenderFragment NotFound { get; set; }

        [Parameter] public RenderFragment<RouteData> Found { get; set; }

        [Parameter] public RenderFragment? Navigating { get; set; }

        [Parameter] public Func<NavigationContext, Task>? OnNavigateAsync { get; set; }

        private RouteTable Routes { get; set; }


        public void Attach(RenderHandle renderHandle)
        {
            _logger = LoggerFactory.CreateLogger<NewApplicationRouter>();
            _renderHandle = renderHandle;
            _baseUri = NavigationManager.BaseUri;
            _locationAbsolute = NavigationManager.Uri;
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        public async Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (NotFound == null)
            {
                throw new InvalidOperationException($"The {nameof(NewApplicationRouter)} component requires a value for the parameter {nameof(NotFound)}.");
            }

            if (!_onNavigateCalled)
            {
                _onNavigateCalled = true;
                await RunOnNavigateWithRefreshAsync(NavigationManager.ToBaseRelativePath(_locationAbsolute), isNavigationIntercepted: false);
                return;
            }

            Refresh(isNavigationIntercepted: false);
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        internal virtual void Refresh(bool isNavigationIntercepted)
        {
            if (_previousOnNavigateTask.Status != TaskStatus.RanToCompletion)
            {
                if (Navigating != null)
                {
                    _renderHandle.Render(Navigating);
                }
                return;
            }

            var locationPath = NavigationManager.ToBaseRelativePath(_locationAbsolute);

            if (locationPath.StartsWith("app/"))
            {
                var match = Regex.Match(locationPath, @"^app\/(?'key'[^\/]*)(?'suffix'.*)");

                if (match.Success)
                {
                    var app = SubApplicationManager.SubApplications.Where(a => a.Key == match.Groups["key"].Value).Single();
                    var suffix = match.Groups["suffix"].Value.TrimStart('/');

                    Uri uri = new Uri($"{app.Uri}{suffix}");

                    Log.NavigatingToUri(_logger, uri, locationPath, _baseUri);

                    _renderHandle.Render(SubApplication(uri));
                }
            }
            else
            {
                if (!isNavigationIntercepted)
                {
                    Log.DisplayingNotFound(_logger, locationPath, _baseUri);
                    _renderHandle.Render(NotFound);
                }
                else
                {
                    Log.NavigatingToExternalUri(_logger, _locationAbsolute, locationPath, _baseUri);
                    NavigationManager.NavigateTo(_locationAbsolute, forceLoad: true);
                }
            }
        }

        private async ValueTask<bool> RunOnNavigateAsync(string path, Task previousOnNavigate)
        {
            _onNavigateCts?.Cancel();

            await previousOnNavigate;

            if (OnNavigateAsync == null)
            {
                return true;
            }

            _onNavigateCts = new CancellationTokenSource();
            var navigateContext = new NavigationContext(path, _onNavigateCts.Token);

            try
            {
                if (Navigating != null)
                {
                    _renderHandle.Render(Navigating);
                }
                await OnNavigateAsync(navigateContext);
                return true;
            }
            catch (OperationCanceledException e)
            {
                if (e.CancellationToken != navigateContext.CancellationToken)
                {
                    var rethrownException = new InvalidOperationException("OnNavigateAsync can only be cancelled via NavigateContext.CancellationToken.", e);
                    _renderHandle.Render(builder => ExceptionDispatchInfo.Throw(rethrownException));
                }
            }
            catch (Exception e)
            {
                _renderHandle.Render(builder => ExceptionDispatchInfo.Throw(e));
            }

            return false;
        }

        internal async Task RunOnNavigateWithRefreshAsync(string path, bool isNavigationIntercepted)
        {
            var previousTask = _previousOnNavigateTask;
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            _previousOnNavigateTask = tcs.Task;

            var shouldRefresh = await RunOnNavigateAsync(path, previousTask);
            tcs.SetResult(null);
            if (shouldRefresh)
            {
                Refresh(isNavigationIntercepted);
            }

        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _locationAbsolute = args.Location;
            if (_renderHandle.IsInitialized)
            {
                _ = RunOnNavigateWithRefreshAsync(NavigationManager.ToBaseRelativePath(_locationAbsolute), args.IsNavigationIntercepted);
            }
        }

        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }

        private static class Log
        {
            private static readonly Action<ILogger, string, string, Exception> _displayingNotFound =
                LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(1, "DisplayingNotFound"), $"Displaying {nameof(NotFound)} because path '{{Path}}' with base URI '{{BaseUri}}' does not match any component route");

            private static readonly Action<ILogger, string, string, string, Exception> _navigatingToExternalUri =
                LoggerMessage.Define<string, string, string>(LogLevel.Debug, new EventId(3, "NavigatingToExternalUri"), "Navigating to non-component URI '{ExternalUri}' in response to path '{Path}' with base URI '{BaseUri}'");

            private static readonly Action<ILogger, Uri, string, string, Exception> _navigatingToUri =
    LoggerMessage.Define<Uri, string, string>(LogLevel.Debug, new EventId(2, "NavigatingToUri"), "Navigating to uri {Uri} in response to path '{Path}' with base URI '{BaseUri}'");



            internal static void DisplayingNotFound(ILogger logger, string path, string baseUri)
            {
                _displayingNotFound(logger, path, baseUri, null);
            }

            internal static void NavigatingToUri(ILogger logger, Uri uri, string path, string baseUri)
            {
                _navigatingToUri(logger, uri, path, baseUri, null);
            }

            internal static void NavigatingToExternalUri(ILogger logger, string externalUri, string path, string baseUri)
            {
                _navigatingToExternalUri(logger, externalUri, path, baseUri, null);
            }
        }
    }
}
