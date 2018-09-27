using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.LogTmp
{
    public class InitRepository
    {
        public static ILoggerRepository loggerRepository { get; set; }
    }
    public class LogHelper
    {
        private static readonly ILog logError = LogManager.GetLogger(InitRepository.loggerRepository.Name, "logerror");
        private static readonly ILog logInfo = LogManager.GetLogger(InitRepository.loggerRepository.Name, "loginfo");

        #region ErrorLog
        public static void ErrorLog(string throwMsg, Exception ex)
        {
            var errorMsg = string.Format("【抛出信息】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}", new object[] { throwMsg, ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            logError.Error(errorMsg);
        }
        #endregion

        #region CustomizeWriteLog
        public static void WriteLog(string throwMsg, Exception ex)
        {
            var errorMsg = string.Format("【抛出信息】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}", new object[] { throwMsg, ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            logError.Error(errorMsg);
        }
        #endregion
    }
}
