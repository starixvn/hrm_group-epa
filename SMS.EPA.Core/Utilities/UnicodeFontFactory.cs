namespace SMS.EPA.Core.Utilities
{
    using Microsoft.AspNetCore.Http;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class UnicodeFontFactory : FontFactoryImp
    {
        private readonly string _fontPath;

        private readonly BaseFont _baseFont;

        public UnicodeFontFactory(IHttpContextAccessor httpContextAccessor)
        {
            var webRootPath = httpContextAccessor.HttpContext?.RequestServices
                .GetService(typeof(Microsoft.AspNetCore.Hosting.IWebHostEnvironment)) as Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
            
            _fontPath = System.IO.Path.Combine(webRootPath?.WebRootPath ?? "", "Content", "Fonts", "ARIALUNI.TTF");
            _baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, true);
        }

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
        {
            return new Font(_baseFont, size, style, color);
        }
    }
}