﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Leaf.Model
{
    public class User
    {
        //Id，主键，自增
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        //用户名，不为空，唯一
        [Unique,NotNull]
        public string Username { get; set; }

        //密码，不为空
        [NotNull]
        public string Password { get; set; }

        //管理员标识
        [NotNull]
        public int Admin { get; set; }
    }
}