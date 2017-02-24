﻿using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Decision.DataLayer.Context;
using Decision.DomainClasses.Users;
using Decision.Services.EntityFramework.Messages;
using Decision.Services.EntityFramework.Security;
using Decision.Services.EntityFramework.Users;
using Decision.Services.Interfaces.Security;
using Decision.Services.Interfaces.Users;
using StructureMap;
using StructureMap.Web;

namespace Decision.Web.Infrastructure.IocConfig
{
    public class IdentityRegistry : Registry
    {
        public IdentityRegistry()
        {
            For<IUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<ApplicationDbContext>();

            For<ApplicationDbContext>()
                .HybridHttpOrThreadLocalScoped()
                .Use(context => (ApplicationDbContext) context.GetInstance<IUnitOfWork>());

            For<DbContext>()
                .HybridHttpOrThreadLocalScoped()
                .Use(context => (ApplicationDbContext) context.GetInstance<IUnitOfWork>());

            For<IUserStore<User, long>>()
                .HybridHttpOrThreadLocalScoped()
                .Use<UserStore>();

            For<IRoleStore<Role, long>>()
                .HybridHttpOrThreadLocalScoped()
                .Use<RoleStore<Role, long, UserRole>>();

            For<IAuthenticationManager>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<ISignInService>()
                .HybridHttpOrThreadLocalScoped()
                .Use<SignInService>();

            For<IRoleService>()
                .HybridHttpOrThreadLocalScoped()
                .Use<RoleService>();

            For<IIdentityMessageService>().Use<SmsService>();
            For<IIdentityMessageService>().Use<EmailService>();

            For<IUserService>()
                .HybridHttpOrThreadLocalScoped()
                .Use<UserService>()
                .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
                .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
                .Setter(userManager => userManager.SmsService).Is<SmsService>()
                .Setter(userManager => userManager.EmailService).Is<EmailService>();

            For<UserService>()
                .HybridHttpOrThreadLocalScoped()
                .Use(context => (UserService) context.GetInstance<IUserService>());

        }
    }
}