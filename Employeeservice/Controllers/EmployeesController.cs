using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using EmployeeDataAccess;

namespace Employeeservice.Controllers
{
    public class EmployeesController : ApiController
    {
        public HttpResponseMessage Get(string type="All")
        {
            using (CoreDBEntities entities = new CoreDBEntities())
            {
                switch (type.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.EMPLOYEEDETAILS.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.EMPLOYEEDETAILS.Where(e => e.Gender.ToLower() == "m").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.EMPLOYEEDETAILS.Where(e => e.Gender.ToLower() == "f").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "Invalid query string");
                }
                //return entities.EMPLOYEEDETAILS.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (CoreDBEntities entities = new CoreDBEntities())
            {
                var entity = entities.EMPLOYEEDETAILS.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                    return Request.CreateResponse(HttpStatusCode.Found, entity);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Employee not found");
            }
        }

        public HttpResponseMessage Post([FromBody] EMPLOYEEDETAIL employee)
        {
            using (CoreDBEntities ent = new CoreDBEntities())
            {
                try
                {
                    ent.EMPLOYEEDETAILS.Add(employee);
                    ent.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }


        }
    }
}
