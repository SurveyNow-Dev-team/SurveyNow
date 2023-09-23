using Application.Interfaces.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileService: IFileService
    {
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadFileAsync()
        {
            //var auth = new FirebaseAuthProvider(new FirebaseAuthConfig)
            //var task = new FirebaseStorage(_configuration["firebase:bucket"],
            //    new FirebaseStorageOptions
            //    {
            //        AuthTokenAsyncFactory =
            //    }
            //    );

            //var config = new FirebaseAuthConfig()
            //{
            //    ApiKey = _configuration["firebase:apiKey"],
            //    AuthDomain =
            //};
            //var auth = FirebaseAuthProvider.;
        }
    }
}
