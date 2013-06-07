// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��05��13�� 18:15
//  
//  �޸��ڣ�2013��05��13�� 18:20
//  �ļ�����FrameworkOnly/Dev.Comm.Web.Mvc/WatchAttribute.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Dev.Comm.Web.Mvc.Filter
{
    class WatchData
    {
        public string Name { get; set; }
        public long OnActionExecuting { get; set; }
        public long OnActionExecuted { get; set; }
        public long OnResultExecuted { get; set; }
        public long OnResultExecuting { get; set; }
        public long All
        {
            get
            {
                return OnResultExecuted - OnActionExecuting;
            }
        }
        public bool IsChild { get; set; }


        public string Report()
        {
            var info = "Action��:=>" + this.Name;

            info += " ȫ��ִ��=>" + All / 10000 + "ms";

            info += " Action=> " + (this.OnActionExecuted - this.OnActionExecuting) / 10000 + " ms";

            info += " Result=> " + (this.OnResultExecuted - this.OnResultExecuting) / 10000 + " ms";



            if (IsChild)
                info += " IsChild Action";

            return info + "<br/>";
        }
    }
    /// <summary>
    /// ����MVC Action Result ��������ʱ��
    /// </summary>
    public class TraceRunAttribute : ActionFilterAttribute
    {

        public TraceRunAttribute()
        {
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public string TitleTemple { get; set; }
        /// <summary>
        /// ��ģ��
        /// </summary>
        public string ItemTemple { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string name = filterContext.ActionDescriptor.ActionName;
            var context = filterContext.HttpContext;

            var item = this.CheckItem(context, name);
            item.OnActionExecuted = System.DateTime.Now.Ticks;
            this.UpdateItem(filterContext.HttpContext, name, item);


            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string name = filterContext.ActionDescriptor.ActionName;
            var context = filterContext.HttpContext;

            var item = this.CheckItem(context, name);
            item.OnActionExecuting = System.DateTime.Now.Ticks;
            this.UpdateItem(filterContext.HttpContext, name, item);


            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string name = filterContext.RouteData.Values["action"].ToString();
            var context = filterContext.HttpContext;

            var item = this.CheckItem(context, name);
            item.OnResultExecuted = System.DateTime.Now.Ticks;
            item.IsChild = filterContext.IsChildAction;
            this.UpdateItem(filterContext.HttpContext, name, item);


            if (!filterContext.IsChildAction && filterContext.Result is ViewResult)
            {
                WriteReport(context);
            }


            base.OnResultExecuted(filterContext);
        }

        private static void WriteReport(HttpContextBase context)
        {
            context.Response.Write("<b>������Ϣ</b><br/>");

            var list = new List<WatchData>();

            foreach (var contextitem in context.Items.Values)
            {
                var result = contextitem as WatchData;

                if (result != null)
                    list.Add(result);

            }

            list.OrderByDescending(x => x.All).ForEach(x => context.Response.Write(x.Report()));

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string name = filterContext.RouteData.Values["action"].ToString();
            var context = filterContext.HttpContext;

            var item = this.CheckItem(context, name);
            item.OnResultExecuting = System.DateTime.Now.Ticks;
            this.UpdateItem(filterContext.HttpContext, name, item);

            base.OnResultExecuting(filterContext);
        }

        private WatchData CheckItem(HttpContextBase httpcontext, string name)
        {
            if (!httpcontext.Items.Contains(name))
            {
                httpcontext.Items.Add(name, new WatchData());
            }
            var watch = httpcontext.Items[name] as WatchData;

            watch.Name = name;

            return watch;
        }


        private void UpdateItem(HttpContextBase httpcontext, string name, WatchData watchTemp)
        {
            httpcontext.Items[name] = watchTemp;
        }
    }
}