using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IPhoneNumberService
    {
        Task SendSmsAsync(string message, string phoneNumber);
    }
}
