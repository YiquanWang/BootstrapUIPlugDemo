﻿/*
 * 程序中文名称: 残友内容管理系统通用版
 * 
 * 程序英文名称: CanYouCMS
 * 
 * 程序版本: 0.X
 * 
 * 程序编写: CanYou
 * 
 * 官方网站: http://www.canyousoftware.com/
 * 
 * 商业服务: http://www.canyousoftware.com/
 * 
 */

using System;
using System.Text;
using System.Web;

namespace FileUploadWeb.Common
{
    /// <summary>
    /// pdf2swf-0.9.1.exe调用
    /// </summary>
    public static class SwftoolsHelp
    {
        /// <summary>
        /// 转换所有的页，图片质量80%
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <param name="swfPath"></param>
        /// <returns></returns>
        public static bool PDF2Swf(string pdfPath, string swfPath)
        {
            return PDF2Swf(pdfPath, swfPath, 1, GetPageCount(HttpContext.Current.Server.MapPath(pdfPath)), 80);
        }
        /// <summary>
        /// 转换前N页，图片质量80%
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <param name="swfPath"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static bool PDF2Swf(string pdfPath, string swfPath, int page)
        {
            return PDF2Swf(pdfPath, swfPath, 1, page, 80);
        }
        /// <summary>
        /// PDF格式转为SWF
        /// </summary>
        /// <param name="pdfPath">原视频文件地址，如/a/b/c.pdf</param>
        /// <param name="swfPath">生成后的FLV文件地址，如/a/b/c.swf</param>
        /// <param name="beginpage">转换开始页</param>
        /// <param name="endpage">转换结束页</param>
        /// <param name="photoQuality"></param>
        /// <returns></returns>
        public static bool PDF2Swf(string pdfPath, string swfPath, int beginpage, int endpage, int photoQuality)
        {
            const string tooldir = "~/Utils/PDFtoSWF/";//PDFtoSWF工具目录地址
            string exe = HttpContext.Current.Server.MapPath(tooldir + "pdf2swf.exe");
            pdfPath = HttpContext.Current.Server.MapPath(pdfPath);
            swfPath = HttpContext.Current.Server.MapPath(swfPath);
            if (!System.IO.File.Exists(exe) || !System.IO.File.Exists(pdfPath))
            {
                return false;
            }
            if (System.IO.File.Exists(swfPath))
                System.IO.File.Delete(swfPath);
            StringBuilder sb = new StringBuilder();
            sb.Append(" \"" + pdfPath + "\"");//input
            sb.Append(" -o \"" + swfPath + "\"");//output
            //sb.Append(" -z");
            sb.Append(" -s flashversion=9");//flash version
            //sb.Append(" -s disablelinks");//禁止PDF里面的链接
            if (endpage > GetPageCount(pdfPath)) endpage = GetPageCount(pdfPath);
            sb.Append(" -p " + "\"" + beginpage + "" + "-" + endpage + "\"");//page range
            sb.Append(" -j " + photoQuality);//SWF中的图片质量
            string Command = sb.ToString();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = Command;
            p.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath(tooldir);
            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序 启动 线程
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,pdf2swf-0.9.1.exe的所有输出信息,都为错误输出流,用 StandardOutput是捕获不到任何消息的...
            p.StartInfo.CreateNoWindow = true;//不创建进程窗口
            p.Start();//启动线程
            p.BeginErrorReadLine();//开始异步读取
            p.WaitForExit();//等待完成
            //p.StandardError.ReadToEnd();//开始同步读取
            p.Close();//关闭进程
            p.Dispose();//释放资源
            if (!System.IO.File.Exists(swfPath))
                return false;
            else
                return true;
        }


        public static int GetPageCount(string pdfPath)
        {
            try
            {
                byte[] buffer = System.IO.File.ReadAllBytes(pdfPath);
                int length = buffer.Length;
                if (buffer == null)
                    return -1;
                if (buffer.Length <= 0)
                    return -1;
                string pdfText = Encoding.Default.GetString(buffer);
                var rx1 = new System.Text.RegularExpressions.Regex(@"/Type\s*/Page[^s]");
                var matches = rx1.Matches(pdfText);
                return matches.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
