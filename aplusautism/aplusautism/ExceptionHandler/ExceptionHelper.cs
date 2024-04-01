
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.ExceptionHandler
{
    public class ExceptionHelper
    { /// <summary>
      /// Frame the detail message from exception
      /// </summary>
      /// <param name="ex"></param>
      /// <returns></returns>
        public static string GetDetailMessage(Exception ex)
        {
            StringBuilder messageBuilder = new StringBuilder();
            Type attributeType = typeof(MethodDescription);

            int step = 1;

            var attributes = GetStackTraceSteps<MethodDescription>(ex);
            if (attributes != null && attributes.Count > 0)
                messageBuilder.AppendLine(string.Format
                ("Sorry there is a problem while processing step {0}:", attributes.Count));
            foreach (var attribute in attributes)
            {
                messageBuilder.Append(string.Format
                ("Step {0}: {1}", step.ToString(), attribute.Description));
                messageBuilder.AppendLine();
                step++;
            }
            messageBuilder.AppendLine();

            string formatedMessage = string.Format("{0}Error Description : {1}",
                                                    messageBuilder.ToString(),
                                                    ex.Message
                                                    );

            return formatedMessage;
        }


        /// <summary>
        /// Extrace the custom attribute details from the stacktrace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static List<T> GetStackTraceSteps<T>(Exception ex)
        {
            List<T> traceSteps = new List<T>();
            Type attributeType = typeof(T);
            StackTrace st = new StackTrace(ex);
            if (st != null && st.FrameCount > 0)
            {

                for (int index = st.FrameCount - 1; index >= 0; index--)
                {
                    var attribute = st.GetFrame(index).
                    GetMethod().GetCustomAttributes(attributeType, false).FirstOrDefault();
                    if (attribute != null)
                    {
                        traceSteps.Add((T)attribute);

                    }
                }
            }

            return traceSteps;
        }
    }
}
