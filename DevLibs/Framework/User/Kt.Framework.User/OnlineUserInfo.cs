﻿// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：OnlineUserInfo.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;

namespace Dev.Framework.User
{
    /// <summary>
    ///     在线用户信息
    /// </summary>
    public class OnlineUserInfo
    {
        public DateTime LASTActive { get; set; }
        public decimal Uid { get; set; }
    }
}