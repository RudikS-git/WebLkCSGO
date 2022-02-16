using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;

namespace Application.DTO.Output
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Auth64Id { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
