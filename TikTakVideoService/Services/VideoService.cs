using Azure.Storage.Blobs;
using Google.Protobuf;
using Grpc.Core;
using System.IO;
using System.Text;

namespace TikTakVideoService.Services
{
    public class VideoService : Video.VideoBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string containerName = "tiktaks";
        private const int bufferSize = 4096;
        public VideoService(BlobServiceClient client)
        {
            _blobServiceClient = client;
        }

        public override async Task SendManifest(VideoRequest request, IServerStreamWriter<VideoResponse> responseStream, ServerCallContext context)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(request.Id + ".M3U8");


            using (var stream = await blobClient.OpenReadAsync())
            {
                byte[] buffer = new byte[bufferSize];
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await responseStream.WriteAsync(new VideoResponse
                    {
                        Blobdata = ByteString.CopyFrom(buffer, 0, bytesRead)
                    });
                }
            }
        }
    }
}
