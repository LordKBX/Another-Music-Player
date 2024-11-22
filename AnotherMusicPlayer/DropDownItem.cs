using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherMusicPlayer
{
    public class DropDownItem
    {
        public string? Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public string? Content
        {
            get { return value; }
            set { this.value = value; }
        }
        private string? value;

        public string? AltValue
        {
            get { return altValue; }
            set { this.altValue = value; }
        }
        public string? AltContent
        {
            get { return altValue; }
            set { this.altValue = value; }
        }
        private string? altValue;

        public object? Tag
        {
            get { return tag; }
            set { this.tag = value; }
        }
        private object? tag;

        public object? Data
        {
            get { return data; }
            set { this.data = value; }
        }
        private object? data;

        public string? ToolTip
        {
            get { return (toolTip == null || toolTip.Length == 0) ? value : toolTip; }
            set { this.toolTip = value; }
        }
        private string? toolTip;

        public Image? Image
        {
            get { return img; }
            set { img = value; }
        }
        private Image? img;

        public DropDownItem() : this("", null)
        { }

        public DropDownItem(string? val, Image? im)
        {
            value = val;
            this.img = im;
        }

        public override string? ToString()
        {
            return value;
        }
    }
}
