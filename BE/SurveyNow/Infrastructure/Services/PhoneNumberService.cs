using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Services
{
    public class PhoneNumberService: IPhoneNumberService
    {
        private readonly IConfiguration _configuration;

        public PhoneNumberService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendSmsAsync(string message, string phoneNumber)
        {
            TwilioClient.Init(_configuration["sms:accountId"], _configuration["sms:authToken"]);
            await MessageResource.CreateAsync(body: message, from: new PhoneNumber(_configuration["sms:phoneNumber"]), to: new PhoneNumber(GlobalizePhoneNumber(phoneNumber)));
        }

        private string GlobalizePhoneNumber(string phoneNumber)
        {
            if(Regex.IsMatch(phoneNumber, @"^(84|0[3|5|7|8|9])[0-9]{8}$"))
            {
                phoneNumber = Regex.Replace(phoneNumber, @"^(84|0)", "+84");
            }
            return phoneNumber;
        }
    }
}
