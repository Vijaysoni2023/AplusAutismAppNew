using aplusautism.Data;
using aplusautism.Data.Models;
using aplusautism.Repository.Repository;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace aplusautism.ExceptionHandler
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public readonly IRepository<ExceptionMessageTable> _ExceptionMessageTable;
        public AplusautismDbContext _dbcontext;

        public ErrorHandlerMiddleware(RequestDelegate next

            )
        {
            _next = next;
            //_ExceptionMessageTable = ExceptionMessageTable;
        }
        public async Task Invoke(HttpContext context, IRepository<ExceptionMessageTable> ExceptionMessageTable, AplusautismDbContext dbcontext)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {


                ExceptionMessageTable Obj = new ExceptionMessageTable();

                var response = context.Response;

                Obj.ExceptionMessage = error.StackTrace.ToString();
               // Obj.statuscode = error..ToString();
                Obj.Message = error.Message.ToString();
                Obj.CreatedDate = DateTime.Now;


                dbcontext.ExceptionMessageTable.Add(Obj);
                dbcontext.SaveChanges();
                //ExceptionMessageTable.Insert(Obj);
                response.ContentType = "application/json";
                //var path = context.Request.Path;
                //if (path.HasValue && path.Value.StartsWith("/imgs"))
                //{
                //    context.Request.Path = " / Home / Logout";
                //    context.Request.QueryString = new QueryString("?title=" + path.Value.Replace("/imgs/", ""));
                //}
             
                    context.Response.Redirect("/Admin/Error/Index");
                return;


                //await _next(context);

                //switch (error)
                //{
                //    case AppException e:
                //        // custom application error
                //        response.StatusCode = (int)HttpStatusCode.BadRequest;
                //        break;
                //    case KeyNotFoundException e:
                //        // not found error
                //        response.StatusCode = (int)HttpStatusCode.NotFound;
                //        break;
                //    default:
                //        // unhandled error
                //        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        break;
                //}

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                
                await response.WriteAsync(result);
               
            }
        }

    }
}
