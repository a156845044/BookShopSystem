﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    public class UserMgrEntity
    {
        public long Id { get; set; }

        public string Account { get; set; }

        public string Pwd { get; set; }

        public int RoleType { get; set; }

        public long? ProfessionId { get; set; }

        public string ProfessionName { get; set; }
    }
}
