using Application.Interfaces.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using Application.ErrorHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            // authentication
            FirebaseAuthConfig config = new FirebaseAuthConfig { 
                ApiKey = _configuration["firebase:apiKey"],
                AuthDomain = _configuration["firebase:authDomain"],
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };
            FirebaseAuthClient authClient = new FirebaseAuthClient(config);
            var credential = await authClient.SignInWithEmailAndPasswordAsync(_configuration["googleAccount:email"], _configuration["googleAccount:password"]);
            var token = await credential.User.GetIdTokenAsync();
            
            // construct firebase storage
            string fileExtension = GetFileExtension(fileName);
            var firebaseStorage = new FirebaseStorage(
                storageBucket: _configuration["firebase:bucket"],
                options: new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(token), // this is where auth token come in
                    ThrowOnCancel = true
                });

            // add file
            var url = await firebaseStorage.Child($"{Guid.NewGuid()}.{fileExtension}").PutAsync(fileStream);
            return url;
        }

        private string GetFileExtension(string fileName)
        {
            string[] list = fileName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            return list.Last();
        }
    }
}
