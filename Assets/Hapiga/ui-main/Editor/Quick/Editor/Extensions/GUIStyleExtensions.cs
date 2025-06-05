using UnityEngine;

namespace QuickEditor.Extensions
{
    public static class GUIStyleExtensions
    {
        public static GUIStyle Copy(this GUIStyle style)
        {
            GUIStyle copy = new GUIStyle
            {
                name = style.name,
                normal = style.normal,
                onNormal = style.onNormal,
                hover = style.hover,
                onHover = style.onHover,
                active = style.active,
                onActive = style.onActive,
                focused = style.focused,
                onFocused = style.onFocused,
                alignment = style.alignment,
                border = style.border,
                clipping = style.clipping,
                contentOffset = style.contentOffset,
                fixedHeight = style.fixedHeight,
                fixedWidth = style.fixedWidth,
                font = style.font,
                fontSize = style.fontSize,
                fontStyle = style.fontStyle,
                imagePosition = style.imagePosition,
                margin = style.margin,
                overflow = style.overflow,
                padding = style.padding,
                richText = style.richText,
                stretchHeight = style.stretchHeight,
                stretchWidth = style.stretchWidth,
                wordWrap = style.wordWrap
            };
            return copy;
        }
    }
}
