using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer
{
    public abstract class FrameComponentBase : ComponentBase
    {
        internal const string UriPropertyName = nameof(Uri);

        /// <summary>
        /// Gets the content to be rendered inside the layout.
        /// </summary>
        [Parameter]
        public Uri Uri { get; set; }
    }
}
