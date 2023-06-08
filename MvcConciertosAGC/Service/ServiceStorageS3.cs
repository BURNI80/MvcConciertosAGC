using Amazon.S3;
using Amazon.S3.Model;

namespace MvcConciertosAGC.Service
{
    public class ServiceStorageS3
    {
        //EL NOMBRE DEL BUCKET LO INCLUIREMOS DENTRO DE 
        //APPSETTINGS.JSON
        private string BucketName;
        //S3 TRABAJA CON UNA INTERFACE LLAMADA IAmazonS3
        private IAmazonS3 ClientS3;
        public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clientS3)
        {
            this.BucketName = configuration.GetValue<string>("AWS:BucketName");
            this.ClientS3 = clientS3;
        }

        //COMENZAMOS SUBIENDO FICHEROS AL BUCKET
        //NECESITAMOS FileName, Stream y un Key/Value
        public async Task<bool>UploadFileAsync(string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = this.BucketName
            };
            //DEBEMOS OBTENER UNA RESPUESTA CON EL MISMO TIPO 
            //DE REQUEST
            PutObjectResponse response = await
                this.ClientS3.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //METODO PARA RECUPERAR UN FILE POR CODIGO
        public async Task<Stream> GetFileAsync(string fileName)
        {
            GetObjectResponse response =
                await this.ClientS3.GetObjectAsync(this.BucketName, fileName);
            return response.ResponseStream;
        }
    }

}
