using Microsoft.AspNetCore.Components;

namespace SecureHR.WebUI.Components.QuickActionGrid
{
    public class ActionCardModel
    {
        public required string Title { get; set; }
        public required string Subtitle { get; set; }
        public required RenderFragment IconSvg { get; set; }
        public required string IconBgCssClass { get; set; }
        public required string IconColorCssClass { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
