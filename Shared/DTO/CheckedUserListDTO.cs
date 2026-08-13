using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class CheckedUserListDTO : UserListDTO
    {
        public CheckedUserListDTO() { }
        public CheckedUserListDTO(UserListDTO user, bool isChecked) : base(user)
        {

            IsChecked = isChecked;

        }

        public int ProductId { get; set; }

        public bool IsChecked { get; set; }
    }
}
