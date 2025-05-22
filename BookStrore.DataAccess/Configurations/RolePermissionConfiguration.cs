using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Core.Enums;
using BookStrore.DataAccess.Entities;
using BookStrore.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStrore.DataAccess.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly AuthorizationOptions _authorization;
        public RolePermissionConfiguration(AuthorizationOptions authorization)
        {
            _authorization = authorization;
        }
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(r => new { r.RoleId, r.PermissionId });

            builder.HasData(new RolePermissionEntity { RoleId = 1, PermissionId = 1 },
                new RolePermissionEntity { RoleId = 1, PermissionId = 2 },
                new RolePermissionEntity { RoleId = 1, PermissionId = 3 },
                new RolePermissionEntity { RoleId = 1, PermissionId = 4 },
                new RolePermissionEntity { RoleId = 2, PermissionId = 1 });
        }

        private List<RolePermissionEntity> ParseRolePermissions()
        {
            return _authorization.RolePermissions
                .SelectMany(rp => rp.Permissions
                    .Select(p => new RolePermissionEntity
                    {
                        RoleId = (int)Enum.Parse<Role>(rp.Role),
                        PermissionId = (int)Enum.Parse<Permission>(p)
                    }))
                .ToList();
        }
    }
}
