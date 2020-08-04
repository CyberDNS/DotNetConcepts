#nullable disable warnings

using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace MainContainer.Components
{
    /// <summary>
    /// Displays the specified page component, rendering it inside its layout
    /// and any further nested layouts.
    /// </summary>
    public class FrameView : IComponent
    {
        private RenderHandle _renderHandle;

        /// <summary>
        /// Gets or sets the route data. This determines the page that will be
        /// displayed and the parameter values that will be supplied to the page.
        /// </summary>
        [Parameter]
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the type of a layout to be used if the page does not
        /// declare any layout. If specified, the type must implement <see cref="IComponent"/>
        /// and accept a parameter named <see cref="LayoutComponentBase.Body"/>.
        /// </summary>
        [Parameter]
        public Type Layout { get; set; }


        /// <inheritdoc />
        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        /// <inheritdoc />
        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (Uri == null)
            {
                throw new InvalidOperationException($"The {nameof(FrameView)} component requires a non-null value for the parameter {nameof(Uri)}.");
            }

            RenderFragment rf = builder =>
            {
                builder.OpenComponent(0, Layout);
                builder.AddAttribute(1, nameof(Uri), Uri);
                builder.CloseComponent();
            };

            _renderHandle.Render(rf);

            return Task.CompletedTask;
        }
    }
}

