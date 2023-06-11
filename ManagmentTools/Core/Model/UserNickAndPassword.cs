using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class UserNickAndPassword
    {
        [Key]
        public int IdUserNickAndPassword { get; set; }

        public string? NickName { get; set; }

        public string? Password { get; set; }

       
    }
}
