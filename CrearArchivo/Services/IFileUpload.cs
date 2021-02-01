using BlazorInputFile;
using System.Threading.Tasks;


namespace BlazorFileUpload.Service
{
    interface IFileUpload
    {
        Task UploadAsync(IFileListEntry file);
    }
}
