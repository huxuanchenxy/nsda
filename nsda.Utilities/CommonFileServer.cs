using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace nsda.Utilities
{
    public class CommonFileServer
    {
        public static string UploadFile(UploadFileRequest request,out string msg)
        {
            msg = string.Empty;
            string fileurl = string.Empty;
            try
            {
                if (request.FileBinary == null || request.FileBinary.Length <= 0)
                {
                    return fileurl;
                }

                decimal size = request.FileBinary.Length;
                decimal fileSize = Math.Round(size / 1024 / 1024, 2);
                if (request.Size < fileSize)
                {
                    msg = $"文件最大支持{request.Size}MB";
                    return fileurl;
                }

                string mapPath = HostingEnvironment.MapPath("~/files");
                string pictureSpaceDir = string.Empty;
                string fileName = GetFileName(request.FileEnum,request.FileName,request.ExtendName,out pictureSpaceDir);
                string directoryFolder = $"{mapPath}{pictureSpaceDir}";

                CreateNewDirectory(directoryFolder);
                string fileUrl = $"{directoryFolder}{fileName}";
                string PictureSpaceUrl = $"/files{pictureSpaceDir.Replace('\\', '/')}{fileName}";

                File.WriteAllBytes(fileUrl, request.FileBinary);

                fileurl = PictureSpaceUrl;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("文件上传",ex);
                msg = "上传文件失败";
            }
            return fileurl;
        }

        private static void CreateNewDirectory(string newMapDirectory)
        {
            if (!Directory.Exists(newMapDirectory))
                Directory.CreateDirectory(newMapDirectory);
        }

        private static string GetFileName(FileEnum fileEnum,string fileName,string extendName,out string dir)
        {
            Guid guid = Guid.NewGuid();
            dir = string.Empty;
            switch (fileEnum)
            {
                case FileEnum.EventAttachment:
                    dir = $"\\eventattachment\\";
                    break;

                case FileEnum.DataAttachment:
                    dir = $"\\datasourceattachment\\";
                    break;

                case FileEnum.EventScoreAttachment:
                    dir = $"\\eventscoreattachment\\";
                    break;
                case FileEnum.MemberHead:
                    fileName = guid.ToString().FileMd5();
                    dir = $"\\memberhead\\";
                    break;
                default:
                    return "";
            }
            return $"{fileName}{extendName}";
        }
    }
}
