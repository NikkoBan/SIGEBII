﻿namespace SIGEBI.Web.Services
{
    public interface INotificationService
    {
        void Success(string message);
        void Error(string message);
        void Info(string message);
        void Warning(string message);
    }
}
