using Microsoft.AspNetCore.Components.Forms;
using AuctionSite.Data;
using MudBlazor.Utilities;

namespace AuctionSite.Converters
{
    public static class UploadedFileConverter
    {
        const int MAX_FILE_SIZE_BITS = 2048000;

        public static async Task<UploadedFile> ConvertToUploadedFileAsync(this IBrowserFile formFile)
        {
            UploadedFile? file = null;

            using (var ms = new MemoryStream())
            {
                var fileStream = formFile.OpenReadStream(MAX_FILE_SIZE_BITS);
                await fileStream.CopyToAsync(ms);

                file = new UploadedFile()
                {
                    Name = formFile.Name,
                    ContentType = formFile.ContentType,
                    Data = ms.ToArray()
                };
            }

            return file;
        }

        public static string? ConvertToBrowserRenderableFile(this UploadedFile imageFile)
        {
            if(imageFile != null)
            {
                return $"data:{imageFile.ContentType};base64,{Convert.ToBase64String(imageFile.Data)}";
            }
            return null;
        }

    }
}
