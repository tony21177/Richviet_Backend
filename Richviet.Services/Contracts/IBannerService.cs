using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IBannerService
    {
        Task<List<string>> GetCaroudsels();
    }
}
