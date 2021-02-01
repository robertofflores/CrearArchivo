using BlazorInputFile;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace BlazorFileUpload.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _enviroment;
        public FileUpload(IWebHostEnvironment enviroment)
        {
            _enviroment = enviroment;
        }

        public async Task UploadAsync(IFileListEntry fileEntry)
        {
            //var path = Path.Combine(_enviroment.ContentRootPath, "Upload", fileEntry.Name);
            var path = Path.Combine(_enviroment.ContentRootPath, "\\\\192.168.2.123\\compartir", fileEntry.Name);
            var ms = new MemoryStream();
            await fileEntry.Data.CopyToAsync(ms);
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }
        }


    }
}
