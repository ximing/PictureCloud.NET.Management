using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public class ThumbnailSize
    {
        public ThumbnailSize()
        {
            this.Quality = 88;
            this.Mode = ThumbModeEnum.Cut;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Quality { get; set; }
        public ThumbModeEnum Mode { get; set; }
    }
    public enum ThumbModeEnum
    {
        HW,
        W,
        H,
        Cut,
        Fit
    }

}
