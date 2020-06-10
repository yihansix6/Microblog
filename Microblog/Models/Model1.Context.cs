﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microblog.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MicroblogDBEntities : DbContext
    {
        public MicroblogDBEntities()
            : base("name=MicroblogDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<area> area { get; set; }
        public virtual DbSet<city> city { get; set; }
        public virtual DbSet<Collections> Collections { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<province> province { get; set; }
        public virtual DbSet<Relation> Relation { get; set; }
        public virtual DbSet<Transpond> Transpond { get; set; }
        public virtual DbSet<Userinfo> Userinfo { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    
        public virtual int usp_xiugai(string user_email, Nullable<int> user_id)
        {
            var user_emailParameter = user_email != null ?
                new ObjectParameter("user_email", user_email) :
                new ObjectParameter("user_email", typeof(string));
    
            var user_idParameter = user_id.HasValue ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_xiugai", user_emailParameter, user_idParameter);
        }
    
        public virtual int usp_xiugaier(string user_email, Nullable<int> user_id)
        {
            var user_emailParameter = user_email != null ?
                new ObjectParameter("user_email", user_email) :
                new ObjectParameter("user_email", typeof(string));
    
            var user_idParameter = user_id.HasValue ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_xiugaier", user_emailParameter, user_idParameter);
        }
    
        public virtual int usp_zhucetianjia(string user_email, string user_password, string user_name, Nullable<System.DateTime> user_time)
        {
            var user_emailParameter = user_email != null ?
                new ObjectParameter("user_email", user_email) :
                new ObjectParameter("user_email", typeof(string));
    
            var user_passwordParameter = user_password != null ?
                new ObjectParameter("user_password", user_password) :
                new ObjectParameter("user_password", typeof(string));
    
            var user_nameParameter = user_name != null ?
                new ObjectParameter("user_name", user_name) :
                new ObjectParameter("user_name", typeof(string));
    
            var user_timeParameter = user_time.HasValue ?
                new ObjectParameter("user_time", user_time) :
                new ObjectParameter("user_time", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_zhucetianjia", user_emailParameter, user_passwordParameter, user_nameParameter, user_timeParameter);
        }
    }
}
