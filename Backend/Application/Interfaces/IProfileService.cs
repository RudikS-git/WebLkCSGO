using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Interfaces
{
    public interface IProfileService
    {
        public Task<ProfileDTO> GetProfileAsync(int id);
    }
}
