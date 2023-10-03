using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Helpers;

namespace EscaladaBot.Persistence;

public sealed class FileStore : IFileStore
{
    private readonly BasicAWSCredentials _awsCredentials;

    public FileStore(BasicAWSCredentials awsCredentials)
    {
        _awsCredentials = awsCredentials;
    }

    public async Task<Guid> SaveFile(Stream stream, Guid folderId, string filePath)
    {
        AmazonS3Config config = new()
        {
            ServiceURL = "https://s3.timeweb.com/escalada_bot/problems/" + folderId
        };

        using var client = new AmazonS3Client(_awsCredentials, config);
        
        var ext = Path.GetExtension(filePath);

        var fileGuid = Guid.NewGuid();
        
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = fileGuid + ext,
            BucketName = SecretsHelper.GetS3Bucket,
            CannedACL = S3CannedACL.PublicRead,
        };

        var fileTransferUtil = new TransferUtility(client);
        await fileTransferUtil.UploadAsync(uploadRequest);

        return fileGuid;
    }

    public async Task<IReadOnlyCollection<Stream>> GetFiles(Guid folderId)
    {
        AmazonS3Config config = new()
        {
            ServiceURL = "https://s3.timeweb.com/"
        };

        using var client = new AmazonS3Client(_awsCredentials, config);

        var request = new ListObjectsRequest
        {
            BucketName = SecretsHelper.GetS3Bucket,
            Prefix = "escalada_bot/problems/" + folderId
        };
        
        var files = await client.ListObjectsAsync(request);

        var streams = new List<Stream>();
        
        foreach (var file in files.S3Objects)
        {
            var obj = 
                await client.GetObjectAsync(
                    SecretsHelper.GetS3Bucket, 
                    file.Key);
            
            streams.Add(obj.ResponseStream);
        }
        
        return streams;
    }
}