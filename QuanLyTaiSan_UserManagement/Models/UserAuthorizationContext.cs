using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using QuanLyTaiSan_UserManagement.Helper;

namespace QuanLyTaiSan_UserManagement.Models
{
    public class UserAuthorizationContext : DbContext
    {
        public UserAuthorizationContext() : base("IdentityDatabaseEntities")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SystemFeature> SystemFeature { get; set; }
        public DbSet<UserAuthorization> UserAuthorization { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
    }

    
    public class UserAuthorizationDatabseAction
    {
        private readonly UserAuthorizationContext _dbContext = new UserAuthorizationContext();

        public List<SystemFeature> GetAllFeatureRecords()
        {
            return _dbContext.SystemFeature.ToList();
        }

        public List<SystemFeature> GetFeaturesByRoleName(string name)
        {
            var lstId = _dbContext.UserAuthorization.Where(p=>p.RoleName.ToLower().Equals(name.ToLower()))
                .Select(k => k.FeatureId).ToList();
            return _dbContext.SystemFeature.Where(k => lstId.Contains(k.Id)).ToList();
        }

        public int AddNewFeature(SystemFeature user)
        {
            var insertUserId = 0;
            try
            {
                _dbContext.SystemFeature.Add(user);
                _dbContext.SaveChanges();
                insertUserId = _dbContext.SystemFeature.Last().Id;
            }
            catch
            {
                return insertUserId;
            }
            return insertUserId;
        }

        public int UpdateFeature(SystemFeature user)
        {
            var insertUserId = 0;
            try
            {
                var current = _dbContext.SystemFeature.FirstOrDefault(k => k.Id == user.Id);
                if (current != null)
                {
                    current.Name = user.Name;
                    current.ActionName = user.ActionName;
                    current.ControllerName = user.ControllerName;
                    _dbContext.SaveChanges();
                    insertUserId = user.Id;
                }
                else
                {
                    insertUserId = 0;
                }
            }
            catch
            {
                return insertUserId;
            }
            return insertUserId;
        }
        public bool DeleteListFeature(List<int> lstId)
        {
            try
            {
                foreach (var id in lstId)
                {
                    var current = _dbContext.SystemFeature.FirstOrDefault(k => k.Id == id);
                    if (current != null)
                    {
                        _dbContext.SystemFeature.Remove(current);
                    }
                }
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddRangeUserAuthorization(int[] lstId, string roleName)
        {
            try
            {
                var range = lstId.Select(item => new UserAuthorization
                    {
                        Id = String.Empty,
                        RoleName = roleName,
                        FeatureId = item
                    })
                    .ToList();
                _dbContext.UserAuthorization.AddRange(range);
                _dbContext.SaveChanges();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return true;
            }
            return true;
        }

        public bool DeleteUserAuthorizationByRoleName(string roleName)
        {
            _dbContext.Database.ExecuteSqlCommand(@"DELETE FROM [UserAuthorization] WHERE RoleName = @RoleName",
                new SqlParameter("@RoleName", roleName));
            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateUserAuthorizationByRoleName(string roleName, string newRoleName)
        {
            var range = _dbContext.UserAuthorization.Where(k => k.RoleName.ToLower().Equals(roleName.ToLower())).Select(k=>k.FeatureId).ToArray();
            DeleteUserAuthorizationByRoleName(roleName);
            AddRangeUserAuthorization(range, newRoleName);
            return true;
        }


        public AspNetRoles GetRoleById(string id)
        {
            return _dbContext.AspNetRoles.FirstOrDefault(k => k.Id.Equals(id));
        }

        public List<AspNetRoles> GetAllRole()
        {
            return _dbContext.AspNetRoles.ToList();
        }

        

    }
}