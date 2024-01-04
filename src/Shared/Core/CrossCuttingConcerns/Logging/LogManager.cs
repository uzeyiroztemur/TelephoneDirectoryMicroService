using Core.Entities.DTOs;

namespace Core.CrossCuttingConcerns.Logging
{
    public static class LogManager
    {
        public static void HandleResult<T>(this ILogger logger, T item, string message = null)
        {
            if (item is IResult)
            {
                var result = item as IResult;

                if (result.Success)
                {
                    logger.Info($"{message}\tSUCCESS");
                }
                else
                {
                    logger.Warn($"{message}\t{result.Message}");
                }
            }
            else
            {
                if (item == null)
                {
                    logger.Warn($"{message}\tNULL");
                }
            }
        }

        public static void HandleError(this ILogger logger, Exception ex, string message = null)
        {
            if (ex != null)
            {
                var errorMessage = ex.Message;
                if(ex.InnerException != null)
                {
                    errorMessage += $" | {ex.InnerException.Message}";
                }
                errorMessage += $"\t{ex.StackTrace}";

                logger.Error(errorMessage);
            }
            else
            {
                logger.Error(message);
            }
        }        
    }
}
