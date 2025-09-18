using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Hosting;

namespace SMS.EPA.Services.Interfaces
{
    public interface IHostFacadeService
    {
        public IUserControlSession UserControlSession { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
    }
}
