using System.Collections.Generic;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Here within contains a list of http content types.
    /// </summary>
    public static class HttpContentType
    {
        #region Fields

        private static readonly IReadOnlyList<string> _aac = new[] { "audio/aac" };
        private static readonly IReadOnlyList<string> _abw = new[] { "application/x-abiword" };
        private static readonly IReadOnlyList<string> _arc = new[] { "application/x-freearc" };
        private static readonly IReadOnlyList<string> _avi = new[] { "video/x-msvideo" };
        private static readonly IReadOnlyList<string> _azw = new[] { "application/vnd.amazon.ebook" };
        private static readonly IReadOnlyList<string> _bin = new[] { "application/octet-stream" };
        private static readonly IReadOnlyList<string> _bmp = new[] { "image/bmp" };
        private static readonly IReadOnlyList<string> _bz = new[] { "application/x-bzip" };
        private static readonly IReadOnlyList<string> _bz2 = new[] { "application/x-bzip2" };
        private static readonly IReadOnlyList<string> _cda = new[] { "application/x-cdf" };
        private static readonly IReadOnlyList<string> _compression7z = new[] { "application/x-7z-compressed" };
        private static readonly IReadOnlyList<string> _csh = new[] { "application/x-csh" };
        private static readonly IReadOnlyList<string> _css = new[] { "text/css" };
        private static readonly IReadOnlyList<string> _csv = new[] { "text/csv" };
        private static readonly IReadOnlyList<string> _doc = new[] { "application/msword" };
        private static readonly IReadOnlyList<string> _docx = new[] { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
        private static readonly IReadOnlyList<string> _eot = new[] { "application/vnd.ms-fontobject" };
        private static readonly IReadOnlyList<string> _epub = new[] { "application/epub+zip" };
        private static readonly IReadOnlyList<string> _formUrlEncoded = new[] { "application/x-www-form-urlencoded" };
        private static readonly IReadOnlyList<string> _gif = new[] { "image/gif" };
        private static readonly IReadOnlyList<string> _gz = new[] { "application/gzip" };
        private static readonly IReadOnlyList<string> _htm = new[] { "text/html" };
        private static readonly IReadOnlyList<string> _html = new[] { "text/html" };
        private static readonly IReadOnlyList<string> _ico = new[] { "image/vnd.microsoft.icon" };
        private static readonly IReadOnlyList<string> _ics = new[] { "text/calendar" };
        private static readonly IReadOnlyList<string> _jar = new[] { "application/java-archive" };
        private static readonly IReadOnlyList<string> _jpeg = new[] { "image/jpeg" };
        private static readonly IReadOnlyList<string> _jpg = new[] { "image/jpeg" };
        private static readonly IReadOnlyList<string> _js = new[] { "text/javascript" };
        private static readonly IReadOnlyList<string> _json = new[] { "application/json" };
        private static readonly IReadOnlyList<string> _jsonld = new[] { "application/ld+json" };
        private static readonly IReadOnlyList<string> _media3g2 = new[] { "video/3gpp2", "audio/3gpp2" };
        private static readonly IReadOnlyList<string> _media3gp = new[] { "video/3gpp", "audio/3gpp" };
        private static readonly IReadOnlyList<string> _mid = new[] { "audio/midi", "audio/x-midi" };
        private static readonly IReadOnlyList<string> _midi = new[] { "audio/midi", "audio/x-midi" };
        private static readonly IReadOnlyList<string> _mjs = new[] { "text/javascript" };
        private static readonly IReadOnlyList<string> _mp3 = new[] { "audio/mpeg" };
        private static readonly IReadOnlyList<string> _mp4 = new[] { "video/mp4" };
        private static readonly IReadOnlyList<string> _mpeg = new[] { "video/mpeg" };
        private static readonly IReadOnlyList<string> _mpkg = new[] { "application/vnd.apple.installer+xml" };
        private static readonly IReadOnlyList<string> _odp = new[] { "application/vnd.oasis.opendocument.presentation" };
        private static readonly IReadOnlyList<string> _ods = new[] { "application/vnd.oasis.opendocument.spreadsheet" };
        private static readonly IReadOnlyList<string> _odt = new[] { "application/vnd.oasis.opendocument.text" };
        private static readonly IReadOnlyList<string> _oga = new[] { "audio/ogg" };
        private static readonly IReadOnlyList<string> _ogv = new[] { "video/ogg" };
        private static readonly IReadOnlyList<string> _ogx = new[] { "application/ogg" };
        private static readonly IReadOnlyList<string> _opus = new[] { "audio/opus" };
        private static readonly IReadOnlyList<string> _otf = new[] { "font/otf" };
        private static readonly IReadOnlyList<string> _pdf = new[] { "application/pdf" };
        private static readonly IReadOnlyList<string> _php = new[] { "application/x-httpd-php" };
        private static readonly IReadOnlyList<string> _png = new[] { "image/png" };
        private static readonly IReadOnlyList<string> _ppt = new[] { "application/vnd.ms-powerpoint" };
        private static readonly IReadOnlyList<string> _pptx = new[] { "application/vnd.openxmlformats-officedocument.presentationml.presentation" };
        private static readonly IReadOnlyList<string> _rar = new[] { "application/vnd.rar" };
        private static readonly IReadOnlyList<string> _rtf = new[] { "application/rtf" };
        private static readonly IReadOnlyList<string> _sh = new[] { "application/x-sh" };
        private static readonly IReadOnlyList<string> _svg = new[] { "image/svg+xml" };
        private static readonly IReadOnlyList<string> _swf = new[] { "application/x-shockwave-flash" };
        private static readonly IReadOnlyList<string> _tar = new[] { "application/x-tar" };
        private static readonly IReadOnlyList<string> _tif = new[] { "image/tiff" };
        private static readonly IReadOnlyList<string> _tiff = new[] { "image/tiff" };
        private static readonly IReadOnlyList<string> _ts = new[] { "video/mp2t" };
        private static readonly IReadOnlyList<string> _ttf = new[] { "font/ttf" };
        private static readonly IReadOnlyList<string> _txt = new[] { "text/plain" };
        private static readonly IReadOnlyList<string> _vsd = new[] { "application/vnd.visio" };
        private static readonly IReadOnlyList<string> _wav = new[] { "audio/wav" };
        private static readonly IReadOnlyList<string> _weba = new[] { "audio/webm" };
        private static readonly IReadOnlyList<string> _webm = new[] { "video/webm" };
        private static readonly IReadOnlyList<string> _webp = new[] { "image/webp" };
        private static readonly IReadOnlyList<string> _woff = new[] { "font/woff" };
        private static readonly IReadOnlyList<string> _woff2 = new[] { "font/woff2" };
        private static readonly IReadOnlyList<string> _xhtml = new[] { "application/xhtml+xml" };
        private static readonly IReadOnlyList<string> _xls = new[] { "application/vnd.ms-excel" };
        private static readonly IReadOnlyList<string> _xlsx = new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
        private static readonly IReadOnlyList<string> _xml = new[] { "application/xml", "text/xml" };
        private static readonly IReadOnlyList<string> _xul = new[] { "application/vnd.mozilla.xul+xml" };
        private static readonly IReadOnlyList<string> _zip = new[] { "application/zip" };

        #endregion Fields

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        #region Properties

        public static IReadOnlyList<string> Aac => _aac;
                                           
        public static IReadOnlyList<string> Abw => _abw;
                                           
        public static IReadOnlyList<string> Arc => _arc;
                                           
        public static IReadOnlyList<string> Avi => _avi;
                                           
        public static IReadOnlyList<string> Azw => _azw;
                                           
        public static IReadOnlyList<string> Bin => _bin;
                                           
        public static IReadOnlyList<string> Bmp => _bmp;

        public static IReadOnlyList<string> Bz => _bz;

        public static IReadOnlyList<string> Bz2 => _bz2;

        public static IReadOnlyList<string> Cda => _cda;

        public static IReadOnlyList<string> Compression7z => _compression7z;

        public static IReadOnlyList<string> Csh => _csh;

        public static IReadOnlyList<string> Css => _css;

        public static IReadOnlyList<string> Csv => _csv;

        public static IReadOnlyList<string> Doc => _doc;

        public static IReadOnlyList<string> Docx => _docx;

        public static IReadOnlyList<string> Eot => _eot;

        public static IReadOnlyList<string> Epub => _epub;

        public static IReadOnlyList<string> FormUrlEncoded => _formUrlEncoded;

        public static IReadOnlyList<string> Gif => _gif;

        public static IReadOnlyList<string> Gz => _gz;

        public static IReadOnlyList<string> Htm => _htm;

        public static IReadOnlyList<string> Html => _html;

        public static IReadOnlyList<string> Ico => _ico;

        public static IReadOnlyList<string> Ics => _ics;

        public static IReadOnlyList<string> Jar => _jar;

        public static IReadOnlyList<string> Jpeg => _jpeg;

        public static IReadOnlyList<string> Jpg => _jpg;

        public static IReadOnlyList<string> Js => _js;

        public static IReadOnlyList<string> Json => _json;

        public static IReadOnlyList<string> Jsonld => _jsonld;

        public static IReadOnlyList<string> Media3g2 => _media3g2;

        public static IReadOnlyList<string> Media3gp => _media3gp;

        public static IReadOnlyList<string> Mid => _mid;

        public static IReadOnlyList<string> Midi => _midi;

        public static IReadOnlyList<string> Mjs => _mjs;

        public static IReadOnlyList<string> Mp3 => _mp3;

        public static IReadOnlyList<string> Mp4 => _mp4;

        public static IReadOnlyList<string> Mpeg => _mpeg;

        public static IReadOnlyList<string> Mpkg => _mpkg;

        public static IReadOnlyList<string> Odp => _odp;

        public static IReadOnlyList<string> Ods => _ods;

        public static IReadOnlyList<string> Odt => _odt;

        public static IReadOnlyList<string> Oga => _oga;

        public static IReadOnlyList<string> Ogv => _ogv;

        public static IReadOnlyList<string> Ogx => _ogx;

        public static IReadOnlyList<string> Opus => _opus;

        public static IReadOnlyList<string> Otf => _otf;

        public static IReadOnlyList<string> Pdf => _pdf;

        public static IReadOnlyList<string> Php => _php;

        public static IReadOnlyList<string> Png => _png;

        public static IReadOnlyList<string> Ppt => _ppt;

        public static IReadOnlyList<string> Pptx => _pptx;

        public static IReadOnlyList<string> Rar => _rar;

        public static IReadOnlyList<string> Rtf => _rtf;

        public static IReadOnlyList<string> Sh => _sh;

        public static IReadOnlyList<string> Svg => _svg;

        public static IReadOnlyList<string> Swf => _swf;

        public static IReadOnlyList<string> Tar => _tar;

        public static IReadOnlyList<string> Tif => _tif;

        public static IReadOnlyList<string> Tiff => _tiff;

        public static IReadOnlyList<string> Ts => _ts;

        public static IReadOnlyList<string> Ttf => _ttf;

        public static IReadOnlyList<string> Txt => _txt;

        public static IReadOnlyList<string> Vsd => _vsd;

        public static IReadOnlyList<string> Wav => _wav;

        public static IReadOnlyList<string> Weba => _weba;

        public static IReadOnlyList<string> Webm => _webm;

        public static IReadOnlyList<string> Webp => _webp;

        public static IReadOnlyList<string> Woff => _woff;

        public static IReadOnlyList<string> Woff2 => _woff2;

        public static IReadOnlyList<string> Xhtml => _xhtml;

        public static IReadOnlyList<string> Xls => _xls;

        public static IReadOnlyList<string> Xlsx => _xlsx;

        public static IReadOnlyList<string> Xml => _xml;

        public static IReadOnlyList<string> Xul => _xul;

        public static IReadOnlyList<string> Zip => _zip;

        #endregion Properties

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}