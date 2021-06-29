using System.Collections.Generic;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Here within contains a list of http content types.
    /// </summary>
    public static class HttpContentType
    {
        #region Fields

        private static readonly IList<string> aac = new[] { "audio/aac" };
        private static readonly IList<string> abw = new[] { "application/x-abiword" };
        private static readonly IList<string> arc = new[] { "application/x-freearc" };
        private static readonly IList<string> avi = new[] { "video/x-msvideo" };
        private static readonly IList<string> azw = new[] { "application/vnd.amazon.ebook" };
        private static readonly IList<string> bin = new[] { "application/octet-stream" };
        private static readonly IList<string> bmp = new[] { "image/bmp" };
        private static readonly IList<string> bz = new[] { "application/x-bzip" };
        private static readonly IList<string> bz2 = new[] { "application/x-bzip2" };
        private static readonly IList<string> cda = new[] { "application/x-cdf" };
        private static readonly IList<string> compression7z = new[] { "application/x-7z-compressed" };
        private static readonly IList<string> csh = new[] { "application/x-csh" };
        private static readonly IList<string> css = new[] { "text/css" };
        private static readonly IList<string> csv = new[] { "text/csv" };
        private static readonly IList<string> doc = new[] { "application/msword" };
        private static readonly IList<string> docx = new[] { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
        private static readonly IList<string> eot = new[] { "application/vnd.ms-fontobject" };
        private static readonly IList<string> epub = new[] { "application/epub+zip" };
        private static readonly IList<string> formUrlEncoded = new[] { "application/x-www-form-urlencoded" };
        private static readonly IList<string> gif = new[] { "image/gif" };
        private static readonly IList<string> gz = new[] { "application/gzip" };
        private static readonly IList<string> htm = new[] { "text/html" };
        private static readonly IList<string> html = new[] { "text/html" };
        private static readonly IList<string> ico = new[] { "image/vnd.microsoft.icon" };
        private static readonly IList<string> ics = new[] { "text/calendar" };
        private static readonly IList<string> jar = new[] { "application/java-archive" };
        private static readonly IList<string> jpeg = new[] { "image/jpeg" };
        private static readonly IList<string> jpg = new[] { "image/jpeg" };
        private static readonly IList<string> js = new[] { "text/javascript" };
        private static readonly IList<string> json = new[] { "application/json" };
        private static readonly IList<string> jsonld = new[] { "application/ld+json" };
        private static readonly IList<string> media3g2 = new[] { "video/3gpp2", "audio/3gpp2" };
        private static readonly IList<string> media3gp = new[] { "video/3gpp", "audio/3gpp" };
        private static readonly IList<string> mid = new[] { "audio/midi", "audio/x-midi" };
        private static readonly IList<string> midi = new[] { "audio/midi", "audio/x-midi" };
        private static readonly IList<string> mjs = new[] { "text/javascript" };
        private static readonly IList<string> mp3 = new[] { "audio/mpeg" };
        private static readonly IList<string> mp4 = new[] { "video/mp4" };
        private static readonly IList<string> mpeg = new[] { "video/mpeg" };
        private static readonly IList<string> mpkg = new[] { "application/vnd.apple.installer+xml" };
        private static readonly IList<string> odp = new[] { "application/vnd.oasis.opendocument.presentation" };
        private static readonly IList<string> ods = new[] { "application/vnd.oasis.opendocument.spreadsheet" };
        private static readonly IList<string> odt = new[] { "application/vnd.oasis.opendocument.text" };
        private static readonly IList<string> oga = new[] { "audio/ogg" };
        private static readonly IList<string> ogv = new[] { "video/ogg" };
        private static readonly IList<string> ogx = new[] { "application/ogg" };
        private static readonly IList<string> opus = new[] { "audio/opus" };
        private static readonly IList<string> otf = new[] { "font/otf" };
        private static readonly IList<string> pdf = new[] { "application/pdf" };
        private static readonly IList<string> php = new[] { "application/x-httpd-php" };
        private static readonly IList<string> png = new[] { "image/png" };
        private static readonly IList<string> ppt = new[] { "application/vnd.ms-powerpoint" };
        private static readonly IList<string> pptx = new[] { "application/vnd.openxmlformats-officedocument.presentationml.presentation" };
        private static readonly IList<string> rar = new[] { "application/vnd.rar" };
        private static readonly IList<string> rtf = new[] { "application/rtf" };
        private static readonly IList<string> sh = new[] { "application/x-sh" };
        private static readonly IList<string> svg = new[] { "image/svg+xml" };
        private static readonly IList<string> swf = new[] { "application/x-shockwave-flash" };
        private static readonly IList<string> tar = new[] { "application/x-tar" };
        private static readonly IList<string> tif = new[] { "image/tiff" };
        private static readonly IList<string> tiff = new[] { "image/tiff" };
        private static readonly IList<string> ts = new[] { "video/mp2t" };
        private static readonly IList<string> ttf = new[] { "font/ttf" };
        private static readonly IList<string> txt = new[] { "text/plain" };
        private static readonly IList<string> vsd = new[] { "application/vnd.visio" };
        private static readonly IList<string> wav = new[] { "audio/wav" };
        private static readonly IList<string> weba = new[] { "audio/webm" };
        private static readonly IList<string> webm = new[] { "video/webm" };
        private static readonly IList<string> webp = new[] { "image/webp" };
        private static readonly IList<string> woff = new[] { "font/woff" };
        private static readonly IList<string> woff2 = new[] { "font/woff2" };
        private static readonly IList<string> xhtml = new[] { "application/xhtml+xml" };
        private static readonly IList<string> xls = new[] { "application/vnd.ms-excel" };
        private static readonly IList<string> xlsx = new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
        private static readonly IList<string> xml = new[] { "application/xml", "text/xml" };
        private static readonly IList<string> xul = new[] { "application/vnd.mozilla.xul+xml" };
        private static readonly IList<string> zip = new[] { "application/zip" };

        #endregion Fields

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        #region Properties

        public static IList<string> Aac => aac;

        public static IList<string> Abw => abw;

        public static IList<string> Arc => arc;

        public static IList<string> Avi => avi;

        public static IList<string> Azw => azw;

        public static IList<string> Bin => bin;

        public static IList<string> Bmp => bmp;

        public static IList<string> Bz => bz;

        public static IList<string> Bz2 => bz2;

        public static IList<string> Cda => cda;

        public static IList<string> Compression7z => compression7z;

        public static IList<string> Csh => csh;

        public static IList<string> Css => css;

        public static IList<string> Csv => csv;

        public static IList<string> Doc => doc;

        public static IList<string> Docx => docx;

        public static IList<string> Eot => eot;

        public static IList<string> Epub => epub;

        public static IList<string> FormUrlEncoded => formUrlEncoded;

        public static IList<string> Gif => gif;

        public static IList<string> Gz => gz;

        public static IList<string> Htm => htm;

        public static IList<string> Html => html;

        public static IList<string> Ico => ico;

        public static IList<string> Ics => ics;

        public static IList<string> Jar => jar;

        public static IList<string> Jpeg => jpeg;

        public static IList<string> Jpg => jpg;

        public static IList<string> Js => js;

        public static IList<string> Json => json;

        public static IList<string> Jsonld => jsonld;

        public static IList<string> Media3g2 => media3g2;

        public static IList<string> Media3gp => media3gp;

        public static IList<string> Mid => mid;

        public static IList<string> Midi => midi;

        public static IList<string> Mjs => mjs;

        public static IList<string> Mp3 => mp3;

        public static IList<string> Mp4 => mp4;

        public static IList<string> Mpeg => mpeg;

        public static IList<string> Mpkg => mpkg;

        public static IList<string> Odp => odp;

        public static IList<string> Ods => ods;

        public static IList<string> Odt => odt;

        public static IList<string> Oga => oga;

        public static IList<string> Ogv => ogv;

        public static IList<string> Ogx => ogx;

        public static IList<string> Opus => opus;

        public static IList<string> Otf => otf;

        public static IList<string> Pdf => pdf;

        public static IList<string> Php => php;

        public static IList<string> Png => png;

        public static IList<string> Ppt => ppt;

        public static IList<string> Pptx => pptx;

        public static IList<string> Rar => rar;

        public static IList<string> Rtf => rtf;

        public static IList<string> Sh => sh;

        public static IList<string> Svg => svg;

        public static IList<string> Swf => swf;

        public static IList<string> Tar => tar;

        public static IList<string> Tif => tif;

        public static IList<string> Tiff => tiff;

        public static IList<string> Ts => ts;

        public static IList<string> Ttf => ttf;

        public static IList<string> Txt => txt;

        public static IList<string> Vsd => vsd;

        public static IList<string> Wav => wav;

        public static IList<string> Weba => weba;

        public static IList<string> Webm => webm;

        public static IList<string> Webp => webp;

        public static IList<string> Woff => woff;

        public static IList<string> Woff2 => woff2;

        public static IList<string> Xhtml => xhtml;

        public static IList<string> Xls => xls;

        public static IList<string> Xlsx => xlsx;

        public static IList<string> Xml => xml;

        public static IList<string> Xul => xul;

        public static IList<string> Zip => zip;

        #endregion Properties

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}