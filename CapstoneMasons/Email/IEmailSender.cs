﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}